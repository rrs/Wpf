using Rrs.Wpf.Navigation.Transitions;
using System.Windows;

namespace Rrs.Wpf.Navigation;

public static class TransitionAssist
{
    public static readonly DependencyProperty BackwardsToMeTransitionProperty
        = DependencyProperty.RegisterAttached("BackwardsToMeTransition", typeof(ITransition), typeof(TransitionAssist), new PropertyMetadata(default));

    public static ITransition GetBackwardsToMeTransition(DependencyObject element)
        => (ITransition)element.GetValue(BackwardsToMeTransitionProperty);

    public static void SetBackwardsToMeTransition(DependencyObject element, ITransition value)
        => element.SetValue(BackwardsToMeTransitionProperty, value);

    public static readonly DependencyProperty ForwardsFromMeTransitionProperty
        = DependencyProperty.RegisterAttached("ForwardsFromMeTransition", typeof(ITransition), typeof(TransitionAssist), new PropertyMetadata(default));

    public static ITransition GetForwardsFromMeTransition(DependencyObject element)
        => (ITransition)element.GetValue(ForwardsFromMeTransitionProperty);

    public static void SetForwardsFromMeTransition(DependencyObject element, ITransition value)
        => element.SetValue(ForwardsFromMeTransitionProperty, value);
}
