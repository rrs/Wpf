namespace Rrs.Wpf.Navigation;

internal interface IApplyPageAction
{
    void ApplyPageAction(Action<object>? pageAction);
}
