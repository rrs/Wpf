using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsPrefix("https://github.com/rrs/wpf", "rrsWpf")]
[assembly: XmlnsDefinition("https://github.com/rrs/wpf", "Rrs.Wpf")]
[assembly: XmlnsDefinition("https://github.com/rrs/wpf", "Rrs.Wpf.Converters")]
[assembly: XmlnsDefinition("https://github.com/rrs/wpf", "Rrs.Wpf.Navigation")]
[assembly: XmlnsDefinition("https://github.com/rrs/wpf", "Rrs.Wpf.Navigation.Transitions")]
[assembly: XmlnsDefinition("https://github.com/rrs/wpf", "Rrs.Wpf.ValidationRules")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
