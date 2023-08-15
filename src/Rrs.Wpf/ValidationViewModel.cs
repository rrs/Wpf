namespace Rrs.Wpf;

public class ValidationViewModel : ViewModel
{
    private bool _suppressValidation = true;
    
    public bool SuppressValidation
    {
        get => _suppressValidation;
        protected set
        {
            _suppressValidation = value;
            OnPropertyChanged(nameof(SuppressValidation));
        }
    }
}
