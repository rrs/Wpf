namespace Rrs.Wpf.Converters;

public class NotConverter : BooleanConverter<bool>
{
    public NotConverter() : base(false, true) { }
}
