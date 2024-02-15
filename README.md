# Rrs WPF Library

A few useful pieces of wpf that can be used across many domains. Including a Binding Proxy, some Converters and Navigation.


### XAML

To use the library in Xaml make sure to include

```
xmlns:rrs="https://github.com/rrs/wpf"
```

## Navigation

The navigation framework allows for MVVM or MVPVM styles and comes with some built in annimations. I drew heavily from the transitioner in https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit.

At the core of it is the NavigationArea. This will only keep 1 view loaded at a time, when it is paging it will retain the previous view until it has finished animating then it gets removed from the visual tree

```
<rrs:NavigationArea ItemsSource="{Binding}">
</rrs:NavigationArea>
```

Here it is binding directly to the datacontext which comes from the window just like in the TestApp

```
var w = new MainWindow
{
    DataContext = new List<object>
    {
        new Page1Presenter(),
        new Page2Presenter(),
        new Page3Presenter(),
        new Page4Presenter(),
        new Page5Presenter(),
        new Page5View(),
        new SecretPageView()
    }
};

w.ShowDialog();
```

It can take 3 object types to navigate to / from. Those that implement IPresenter, those that inherit from FrameworkElement, or just objects, the expectation is that those objects will be ViewModels and have an appropriate DataTemplate defined somewhere in resources. e.g.

```
<DataTemplate DataType="{x:Type viewModels:VmBasedPage1ViewModel}">
    <views:VmBasedPage1View />
</DataTemplate>
```

Where the NavigationArea is dealing with either FrameworkElements or ViewModels, it will use those instances each time the view is paged to, however IPresenter implementations will serve a new instance whenever paging _forwards_.

Navigation between pages is done primarily with RoutedCommands it is currently using the built in NavigationCommands.

### Indexed based Navigation 

If no parameters are provided the NavigationArea will page to the next index

```
<Button Command="NavigationCommands.PreviousPage">Back</Button>
<Button Command="NavigationCommands.NextPage">Next</Button>
```

### Page Type based Navigation

Both of the following show navigation by page type, if the commandparameter is just a string it will try and use that as the page type, but if you need to have more control you can use the Navigation MarkupExtension you can also specify the type explicitly

```
<Button Command="NavigationCommands.NextPage" CommandParameter="Page5View">Next</Button>

<Button Command="NavigationCommands.NextPage" CommandParameter="{rrs:Navigation Page1Presenter, AddCurrentPageToHistory=False}">Door 1</Button>

<Button Command="NavigationCommands.NextPage" CommandParameter="{rrs:Navigation PageType={x:Type viewModels:VmBasedPage2ViewModel}}">Next</Button>
```

### View Model based Navigation

If you have a command in your view model such as

```
<Button Command="{Binding NextPageCommand}">Next</Button>
```

Then you can navigate using an INavigator, the framework has one which uses the focused element as the root of the command. It can be used like so, this is also passing the method parameter to the presenter using 'With'

```
public Page2ViewModel(INavigator navigator)
{
    NextPageCommand = new RelayCommand(_ => navigator.With(Cookies).NextPage<Page3Presenter>());
    ...
}
```

### Passing data between pages

Generally you would only pass data forwards, and if you did need to pass any back you can pass forward a common object, that the former page has access to.

You can pass data using either .With() as in the pevious example or if using viewmodels on their own without a presenter you can use 

```
NextPageCommand = new RelayCommand(_ => navigator.NextPage<VmBasedPage4ViewModel>(o =>
{
    o.TimesPagedTo++;
}));
```

In Xaml, it is a shame but MarkupExtension does not inherit from dependencyobject so it can't naturally do binding, however you can use the following, which uses a binding proxy to bind to a data object

```
<rrs:BindingProxy In="{Binding Text, ElementName=MaName}" Out="{Binding Name, ElementName=Page4Data}" />
<StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right">
    <Button Command="NavigationCommands.PreviousPage">Back</Button>
    <Button Command="NavigationCommands.NextPage" Content="Next">
        <Button.CommandParameter>
            <rrs:Navigation PageType="{x:Type presenters:Page4Presenter}">
                <rrs:Navigation.Parameter>
                    <o:Page4Data x:Name="Page4Data" />
                </rrs:Navigation.Parameter>
            </rrs:Navigation>
        </Button.CommandParameter>
    </Button>
</StackPanel>
```
alternatively you can create the object as a static resource and bind to that like so

```
...
<UserControl.Resources>
    <o:Page5Data x:Key="Page5Data" />
</UserControl.Resources>
...
<DatePicker  VerticalAlignment="Bottom" HorizontalAlignment="Left" Margin="8" SelectedDate="{Binding Date, Source={StaticResource Page5Data}}" />
...
<Button Command="NavigationCommands.NextPage" CommandParameter="{rrs:Navigation Page5Presenter, Parameter={StaticResource Page5Data}}" VerticalAlignment="Center">Next</Button>
```


## Binding Proxy


## Converters
