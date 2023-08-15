namespace Rrs.Wpf;

public class ValidationViewModel : ViewModel
{
    private bool _supressValidation = true;
    
    public bool SupressValidation
    {
        get => _supressValidation;
        protected set
        {
            _supressValidation = value;
            OnPropertyChanged(nameof(SupressValidation));
        }
    }
}
