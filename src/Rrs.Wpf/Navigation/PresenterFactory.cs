namespace Rrs.Wpf.Navigation;

internal class PresenterFactory : IPresenterFactory
{
    public Type PresenterType { get; }

    public IPresenter CreatePresenter() => _presenter;

    private readonly IPresenter _presenter;

    public PresenterFactory(IPresenter presenter)
    {
        _presenter = presenter;
        PresenterType = presenter.GetType();
    }
}
