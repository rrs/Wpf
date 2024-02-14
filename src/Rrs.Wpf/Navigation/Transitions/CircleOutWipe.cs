using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Rrs.Wpf.Navigation.Transitions;

/// <summary>
/// Inspired from https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
/// </summary>
public class CircleOutWipe : MarkupExtension, ITransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(3000);

    public void Transition(FrameworkElement fromSlide, FrameworkElement toSlide, Point origin, Action onComplete)
    {
        if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
        if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));

        double horizontalProportion = Math.Max(1.0 - origin.X, 1.0 * origin.X);
        double verticalProportion = Math.Max(1.0 - origin.Y, 1.0 * origin.Y);
        double radius = Math.Sqrt(Math.Pow(fromSlide.ActualWidth * horizontalProportion, 2) + Math.Pow(fromSlide.ActualHeight * verticalProportion, 2));

        var scaleTransform = new ScaleTransform(0, 0);
        var translateTransform = new TranslateTransform(fromSlide.ActualWidth * origin.X, fromSlide.ActualHeight * origin.Y);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translateTransform);

        var rectangleGeometry = new RectangleGeometry()
        {
            Rect = new Rect(0, 0, fromSlide.ActualWidth, fromSlide.ActualHeight)
        };

        var ellipseGeometry = new EllipseGeometry()
        {
            RadiusX = radius,
            RadiusY = radius,
            Transform = transformGroup
        };

        var currentClip = fromSlide.GetValue(UIElement.ClipProperty) as Geometry;

        var combinedGeometry = new CombinedGeometry(GeometryCombineMode.Exclude, currentClip ?? rectangleGeometry, ellipseGeometry);
        fromSlide.SetCurrentValue(UIElement.ClipProperty, combinedGeometry);

        var opacityAnimation = AnimationHelper.GenerateOpacityAnimation(Duration);
        fromSlide.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

        var scaleXAnimation = AnimationHelper.GenerateScaleAnimation(Duration);

        var scaleYAnimation = scaleXAnimation.Clone();

        scaleXAnimation.Completed += (s, e) =>
        {
            onComplete();
        };
        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}

