using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Rrs.Wpf.Navigation.Transitions;

/// <summary>
/// Inspired from https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
/// </summary>
public class CircleWipe : MarkupExtension, ITransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);

    public void Transition(FrameworkElement fromSlide, FrameworkElement toSlide, Point origin, Action onComplete)
    {
        if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
        if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));

        // clear old opacity animation
        toSlide.BeginAnimation(UIElement.OpacityProperty, null);

        double horizontalProportion = Math.Max(1.0 - origin.X, 1.0 * origin.X);
        double verticalProportion = Math.Max(1.0 - origin.Y, 1.0 * origin.Y);
        double radius = Math.Sqrt(Math.Pow(toSlide.ActualWidth * horizontalProportion, 2) + Math.Pow(toSlide.ActualHeight * verticalProportion, 2));

        var scaleTransform = new ScaleTransform(0, 0);
        var translateTransform = new TranslateTransform(toSlide.ActualWidth * origin.X, toSlide.ActualHeight * origin.Y);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translateTransform);
        var ellipseGeometry = new EllipseGeometry()
        {
            RadiusX = radius,
            RadiusY = radius,
            Transform = transformGroup
        };

        var currentClip = toSlide.GetValue(UIElement.ClipProperty) as Geometry;

        Geometry clipGeometry = ellipseGeometry;
        if (currentClip != null)
        {
            clipGeometry = new CombinedGeometry(GeometryCombineMode.Union, currentClip, ellipseGeometry);
        }

        toSlide.SetCurrentValue(UIElement.ClipProperty, clipGeometry);

        var rectangleGeometry = new RectangleGeometry()
        {
            Rect = new Rect(0, 0, fromSlide.ActualWidth, fromSlide.ActualHeight)
        };

        var opacityBrush = new SolidColorBrush(Colors.Black);
        var drawingGroup = new DrawingGroup()
        {
            ClipGeometry = rectangleGeometry
        };
        drawingGroup.Children.Add(new GeometryDrawing
        {
            Brush = opacityBrush,
            Geometry = rectangleGeometry
        });
        drawingGroup.Children.Add(new GeometryDrawing
        {
            Brush = Brushes.Black,
            Geometry = ellipseGeometry
        });

        var drawingBrush = new DrawingBrush(drawingGroup) { Stretch = Stretch.None };
        fromSlide.SetCurrentValue(UIElement.OpacityMaskProperty, drawingBrush);

        var opacityAnimation = AnimationHelper.GenerateOpacityAnimation(Duration);
        opacityAnimation.Completed += (s, e) =>
        {
            fromSlide.SetCurrentValue(UIElement.OpacityMaskProperty, null);
        };
        opacityBrush.BeginAnimation(Brush.OpacityProperty, opacityAnimation);

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
