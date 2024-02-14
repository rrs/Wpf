# Rrs WPF Library

A few useful pieces of wpf that can be used across many domains. Including a Binding Proxy, some Converters and Navigation.


### XAML

To use the library in Xaml make sure to include

```
xmlns:rrs="https://github.com/rrs/wpf"
```

## Navigation

The navigation framework allows for MVVM or MVPVM styles and comes with some built in annimations. I drew heavily from the transitioner in https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit.

At the core of it is the NavigationArea.

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

## Binding Proxy


## Converters
