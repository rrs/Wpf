﻿using Rrs.Wpf.Navigation.Transitions;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Rrs.Wpf.Navigation;

[TemplatePart(Name = PART_Grid, Type = typeof(Grid))]
public class NavigationArea : Selector
{
    private const string PART_Grid = "PART_Grid";

    static NavigationArea()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationArea), new FrameworkPropertyMetadata(typeof(NavigationArea)));
    }

    public static readonly DependencyProperty HistoryEnabledProperty = DependencyProperty.Register(
        nameof(HistoryEnabled), typeof(bool), typeof(NavigationArea), new PropertyMetadata(true));

    public bool HistoryEnabled
    {
        get => (bool)GetValue(HistoryEnabledProperty);
        set => SetValue(HistoryEnabledProperty, value);
    }

    public static readonly DependencyProperty BackwardsTransitionProperty = DependencyProperty.Register(
        nameof(BackwardsTransition), typeof(ITransition), typeof(NavigationArea), new PropertyMetadata(new CircleOutWipe()));

    public ITransition BackwardsTransition
    {
        get => (ITransition)GetValue(BackwardsTransitionProperty);
        set => SetValue(BackwardsTransitionProperty, value);
    }

    public static readonly DependencyProperty ForwardsTransitionProperty = DependencyProperty.Register(
        nameof(ForwardsTransition), typeof(ITransition), typeof(NavigationArea), new PropertyMetadata(new CircleWipe()));

    public ITransition ForwardsTransition
    {
        get => (ITransition)GetValue(ForwardsTransitionProperty);
        set => SetValue(ForwardsTransitionProperty, value);
    }

    public static readonly DependencyProperty NavigatorProperty = DependencyProperty.Register(nameof(Navigator), typeof(NavigationAreaNavigator), typeof(NavigationArea), new FrameworkPropertyMetadata(null, NavigatorPropertyChanged));

    private static void NavigatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var navigationArea = (NavigationArea)d;

        if (e.NewValue is not NavigationAreaNavigator navigator) return;

        navigationArea.Navigator = navigator;
        navigator?.Attach(navigationArea);

        var oldNavigator = e.OldValue as NavigationAreaNavigator;
        oldNavigator?.Detach(navigationArea);
    }

    public NavigationAreaNavigator Navigator
    {
        get => (NavigationAreaNavigator)GetValue(NavigatorProperty);
        set => SetValue(NavigatorProperty, value);
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);
        HandleItemsChanged();
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        HandleItemsChanged();
    }

    private static readonly Point DefaultTransitionOrigin = new(0.5, 0.5);
    private readonly Stack<ViewInfo> _history = new();
    private readonly Dictionary<FrameworkElement, Guid?> _toRemove = [];
    private readonly Dictionary<TypeAndNameKey, PresenterInfo> _presenters = [];

    private Grid? _grid;
    private ViewInfo? _currentView;

    public NavigationArea()
    {
        CommandBindings.Add(new CommandBinding(NavigationCommands.NextPage, NextPageExecuted, CanExecuteNextPage));
        CommandBindings.Add(new CommandBinding(NavigationCommands.PreviousPage, PreviousPageExecuted, CanExecutePreviousPage));
        CommandBindings.Add(new CommandBinding(NavigationCommands.FirstPage, FirstPageExecuted, CanExecutePreviousPage));
        var initialised = false;
        Loaded += (sender, args) =>
        {
            if (initialised) return;
            if (Items.Count > 0)
            {
                if (ItemsSource == null)
                {
                    foreach (var item in Items)
                    {
                        RemoveLogicalChild(item);
                    }
                }
                HandleItemsChanged();
            }
            initialised = true;
        };
        Unloaded += (sender, args) =>
        {
            if (_currentView == null) return;
            TryOnNavigateFromView(_currentView.View);
        };
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var grid = GetTemplateChild(PART_Grid) as Grid;
        _grid = grid ?? throw new Exception("NavigationArea needs a Grid in the template to work");
        AddLogicalChild(grid);
    }

    private void HandleItemsChanged()
    {
        if (_grid == null) return;
        _presenters.Clear();

        var index = 0;

        foreach (var o in Items)
        {
            var type = o.GetType();
            TypeAndNameKey key;
            IPresenter presenter;
            if (o is IPresenter p)
            {
                key = new TypeAndNameKey(type);
                presenter = p;
            }
            else if (o is FrameworkElement e)
            {
                key = new TypeAndNameKey(type, CoerseFrameworkElementName(e));
                presenter = new PresenterViewAdapter(e);
            }
            else
            {
                key = new TypeAndNameKey(type);
                presenter = new PresenterViewModelAdapter(o);
            }

            _presenters.Add(key, new PresenterInfo(presenter, index++));
        }

        if (Items.Count > 0 && _currentView == null)
        {
            var first = Items[0];
            string? name = first is FrameworkElement e ? CoerseFrameworkElementName(e) : null;
            var presenterInfo = _presenters[new TypeAndNameKey(first.GetType(), name)];
            var view = presenterInfo.Presenter.PresentView();
            _currentView = new ViewInfo(presenterInfo, view);
            _grid.Children.Add(view);
            SelectedIndex = 0;
        }

        if (_currentView != null)
        {
            Dispatcher.InvokeAsync(() =>
            {
                TryOnNavigateToView(_currentView.View);
            }, System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        static string? CoerseFrameworkElementName(FrameworkElement e) => e.Name == string.Empty ? null : e.Name;
    }

    private void CanExecuteNextPage(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = e.Parameter != null || SelectedIndex < Items.Count - 1;
        e.Handled = true;
    }

    private void CanExecutePreviousPage(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _history.Any();
        e.Handled = true;
    }

    private void NextPageExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        var eventOrigin = GetNavigationSourcePoint(e);
        NextPage(e.Parameter, eventOrigin);
    }

    internal void NextPage(object? parameter = null, Point? eventOrigin = null)
    {
        var navigationParameters = CoerceNavigationParameter(parameter);
        TypeAndNameKey pageKey;
        if (navigationParameters == null)
        {
            pageKey = TypeAndNameKeyFromIndex();
            navigationParameters = new NavigationParameters(pageKey.Type, pageKey.Name);
        }
        else if (navigationParameters.PageType == null && navigationParameters.PageTypeName == null)
        {
            pageKey = TypeAndNameKeyFromIndex();
        }
        else
        {
            var pageType = navigationParameters.PageType ??
                GetPageTypeFromName(navigationParameters.PageTypeName) ??
                throw new Exception($"No page found for type {navigationParameters.PageType?.ToString() ?? navigationParameters.PageTypeName}");
            pageKey = new TypeAndNameKey(pageType, navigationParameters.FrameworkElementName);
        }

        var presenterInfo = _presenters[pageKey];
        var nextPageParams = new NextPageParameters(presenterInfo, eventOrigin,
            navigationParameters.AddCurrentPageToHistory,
            navigationParameters.PresenterArgs,
            navigationParameters.ViewModelPageAction,
            navigationParameters.BackwardsTransition,
            navigationParameters.ForwardsTransition);
        NextPage(nextPageParams);

        TypeAndNameKey TypeAndNameKeyFromIndex()
        {
            var next = Items[SelectedIndex + 1];
            string? name = next is FrameworkElement e ? e.Name : null;
            var pageType = next.GetType();
            return new TypeAndNameKey(pageType, name);
        }
    }

    private NavigationParameters? CoerceNavigationParameter(object? parameter)
    {
        if (parameter is string s) return GetNavigationParametersFromTypeName(s);
        return parameter as NavigationParameters;
    }

    private Type? GetPageTypeFromName(string? name)
    {
        foreach (var item in Items)
        {
            var itemType = item.GetType();
            if (itemType.Name == name) return itemType;
        }
        return null;
    }

    private NavigationParameters? GetNavigationParametersFromTypeName(string name)
    {
        foreach (var item in Items)
        {
            var itemType = item.GetType();
            if (item is FrameworkElement e && e.Name == name)
            {
                return new NavigationParameters(itemType, name);
            }
            if (itemType.Name == name)
            {
                return new NavigationParameters(itemType);
            }
        }

        throw new Exception($"ViewModel {name} does not exist in this NavigationArea");
    }

    private Guid SwapView(ViewInfo nextViewInfo, ViewInfo oldViewInfo)
    {
        _currentView = nextViewInfo;
        SelectedIndex = nextViewInfo.PresenterInfo.Index;

        oldViewInfo.View.IsHitTestVisible = false;
        nextViewInfo.View.IsHitTestVisible = true;

        TryOnNavigateFromView(oldViewInfo.View);
        TryOnNavigateToView(nextViewInfo.View);

        var contextId = Guid.NewGuid();
        _toRemove[oldViewInfo.View] = contextId;

        return contextId;
    }

    private static void TryOnNavigateFromView(FrameworkElement view)
    {
        TryOnNavigateFrom(view);
        TryOnNavigateFrom(view.DataContext);

        static void TryOnNavigateFrom(object? o)
        {
            if (o is INavigateFrom n) n.OnNavigatedFrom();
        }
    }

    private static void TryOnNavigateToView(FrameworkElement view)
    {
        TryOnNavigateTo(view);
        TryOnNavigateTo(view.DataContext);

        static void TryOnNavigateTo(object? o)
        {
            if (o is INavigateTo n) n.OnNavigatedTo();
        }
    }

    private void NextPage(NextPageParameters nextPageParameters)
    {
        if (_grid == null) return;
        var view = nextPageParameters.PresenterInfo.Presenter.PresentView(nextPageParameters.PresenterArgs);

        var viewInfo = new ViewInfo(nextPageParameters.PresenterInfo, view);
        if (viewInfo == _currentView) return;

        if (viewInfo.PresenterInfo.Presenter is PresenterViewModelAdapter pvm)
            pvm.ApplyPageAction(nextPageParameters.VmPageAction);

        view.Visibility = Visibility.Hidden;

        if (_toRemove.ContainsKey(view))
        {
            _toRemove[view] = null;
        }
        else
        {
            _grid.Children.Add(view);
        }

        if (_currentView == null)
        {
            view.Visibility = Visibility.Visible;
            _currentView = viewInfo;
            return;
        }

        var oldViewInfo = _currentView;
        var contextId = SwapView(viewInfo, oldViewInfo);
        oldViewInfo.BackwardsToMeTransition = nextPageParameters.BackwardsTransition;
        if (HistoryEnabled && nextPageParameters.AddCurrentPageToHistory)
            _history.Push(oldViewInfo);

        var transition = nextPageParameters.ForwardsTransition ??
            TransitionAssist.GetForwardsFromMeTransition(oldViewInfo.View) ??
            ForwardsTransition;
        Dispatcher.InvokeAsync(() =>
        {
            view.Visibility = Visibility.Visible;
            view.IsHitTestVisible = true;
            transition.Transition(oldViewInfo.View, view, nextPageParameters.EventOrigin ?? DefaultTransitionOrigin, () =>
            {
                if (_toRemove[oldViewInfo.View] == contextId)
                {
                    RemoveView(_grid, _toRemove, oldViewInfo.View);
                    ClearAnimations(oldViewInfo.View);
                }
            });
        }, System.Windows.Threading.DispatcherPriority.ContextIdle);
    }

    private void PreviousPageExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        var eventOrigin = GetNavigationSourcePoint(e);
        PreviousPage(eventOrigin);
    }

    internal void PreviousPage(Point? eventOrigin = null)
    {
        if (_grid == null) return;
        if (_currentView == null) return;
        if (!_history.Any()) return;

        var previousViewInfo = _history.Pop();
        var oldViewInfo = _currentView;

        var contextId = SwapView(previousViewInfo, oldViewInfo);

        if (_toRemove.ContainsKey(previousViewInfo.View))
        {
            _toRemove[previousViewInfo.View] = null;
        }
        else
        {
            _grid.Children.Insert(0, previousViewInfo.View);
        }

        var transition = previousViewInfo.BackwardsToMeTransition ??
            TransitionAssist.GetBackwardsToMeTransition(previousViewInfo.View) ??
            BackwardsTransition;
        Dispatcher.InvokeAsync(() =>
        {
            transition.Transition(oldViewInfo.View, previousViewInfo.View, eventOrigin ?? DefaultTransitionOrigin, () =>
            {
                if (_toRemove[oldViewInfo.View] == contextId)
                {
                    RemoveView(_grid, _toRemove, oldViewInfo.View);
                    ClearAnimations(oldViewInfo.View);
                }
            });
        }, System.Windows.Threading.DispatcherPriority.ContextIdle);
    }

    private void FirstPageExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        var eventOrigin = GetNavigationSourcePoint(e);
        FirstPage(eventOrigin);
    }

    internal void FirstPage(Point? eventOrigin = null)
    {
        if (_grid == null) return;
        if (_currentView == null) return;
        if (!_history.Any()) return;

        var deadPages = new List<ViewInfo>();
        while (_history.Count > 1)
        {
            deadPages.Add(_history.Pop());
        }

        var previousViewInfo = _history.Pop();
        var oldViewInfo = _currentView;

        var contextId = SwapView(previousViewInfo, oldViewInfo);

        if (_toRemove.ContainsKey(previousViewInfo.View))
        {
            _toRemove[previousViewInfo.View] = null;
        }
        else
        {
            _grid.Children.Insert(0, previousViewInfo.View);
        }

        var transition = previousViewInfo.BackwardsToMeTransition ??
            TransitionAssist.GetBackwardsToMeTransition(previousViewInfo.View) ??
            BackwardsTransition;
        Dispatcher.InvokeAsync(() =>
        {
            transition.Transition(oldViewInfo.View, previousViewInfo.View, eventOrigin ?? DefaultTransitionOrigin, () =>
            {
                foreach (var deadViewInfo in deadPages)
                {
                    var deadView = deadViewInfo.View;
                    RemoveView(_grid, _toRemove, deadView);
                }
                RemoveView(_grid, _toRemove, oldViewInfo.View);
                ClearAnimations(oldViewInfo.View);
            });
        }, System.Windows.Threading.DispatcherPriority.ContextIdle);
    }

    private static void RemoveView(Grid grid, IDictionary<FrameworkElement, Guid?> toRemove, FrameworkElement view)
    {
        toRemove.Remove(view);
        grid.Children.Remove(view);
    }

    private static void ClearAnimations(FrameworkElement view)
    {
        view.SetCurrentValue(ClipProperty, null);
        view.SetCurrentValue(OpacityMaskProperty, null);
        view.BeginAnimation(OpacityProperty, null);
    }

    private Point? GetNavigationSourcePoint(RoutedEventArgs executedRoutedEventArgs)
    {
        var sourceElement = executedRoutedEventArgs.OriginalSource as FrameworkElement;
        if (sourceElement == null || !IsAncestorOf(sourceElement) || !IsSafePositive(ActualWidth) ||
            !IsSafePositive(ActualHeight) || !IsSafePositive(sourceElement.ActualWidth) ||
            !IsSafePositive(sourceElement.ActualHeight)) return null;

        var transitionOrigin = sourceElement.TranslatePoint(new Point(sourceElement.ActualWidth / 2, sourceElement.ActualHeight / 2), this);
        transitionOrigin = new Point(transitionOrigin.X / ActualWidth, transitionOrigin.Y / ActualHeight);
        return transitionOrigin;
    }

    private static bool IsSafePositive(double @double)
        => !double.IsNaN(@double) && !double.IsInfinity(@double) && @double > 0.0;

    public record TypeAndNameKey(Type Type, string? Name = null);
    public record PresenterInfo(IPresenter Presenter, int Index);
    public record ViewInfo(PresenterInfo PresenterInfo, FrameworkElement View)
    {
        public ITransition? BackwardsToMeTransition { get; set; }
    }
    public record NextPageParameters(PresenterInfo PresenterInfo, Point? EventOrigin, bool AddCurrentPageToHistory, object? PresenterArgs, Action<object>? VmPageAction, ITransition? BackwardsTransition, ITransition? ForwardsTransition);
}

