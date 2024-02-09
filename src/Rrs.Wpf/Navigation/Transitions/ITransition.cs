using System.Windows;

namespace Rrs.Wpf.Navigation.Transitions;

public interface ITransition
{
    void Transition(FrameworkElement fromSlide, FrameworkElement toSlide, Point origin, Action onComplete);
}
