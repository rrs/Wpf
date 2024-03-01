using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Rrs.Wpf.Navigation.Transitions;

public enum SlideDirection { Left, Right, Up, Down }

/// <summary>
/// Inspired from https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
/// </summary>
public class SlideWipe : MarkupExtension, ITransition
{
    public EasingFunctionBase Easing { get; set; } = new SineEase();
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);
    public SlideDirection Direction { get; set; } = SlideDirection.Left;

    public void Transition(FrameworkElement fromSlide, FrameworkElement toSlide, Point origin, Action onComplete)
    {
        if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
        if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));

        // Set up time points
        var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
        var endKeyTime = KeyTime.FromTimeSpan(Duration);

        // Set up coordinates
        double fromStartX = 0, fromEndX = 0, toStartX = 0, toEndX = 0;
        double fromStartY = 0, fromEndY = 0, toStartY = 0, toEndY = 0;

        if (Direction == SlideDirection.Left)
        {
            fromEndX = -fromSlide.ActualWidth;
            toStartX = toSlide.ActualWidth;
        }
        else if (Direction == SlideDirection.Right)
        {
            fromEndX = fromSlide.ActualWidth;
            toStartX = -toSlide.ActualWidth;
        }
        else if (Direction == SlideDirection.Up)
        {
            fromEndY = -fromSlide.ActualHeight;
            toStartY = toSlide.ActualHeight;
        }
        else if (Direction == SlideDirection.Down)
        {
            fromEndY = fromSlide.ActualHeight;
            toStartY = -toSlide.ActualHeight;
        }

        // From
        var fromTransform = new TranslateTransform(fromStartX, fromStartY);
        fromSlide.RenderTransform = fromTransform;
        var fromXAnimation = new DoubleAnimationUsingKeyFrames();
        fromXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartX, zeroKeyTime));
        fromXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndX, endKeyTime, Easing));
        var fromYAnimation = new DoubleAnimationUsingKeyFrames();
        fromYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartY, zeroKeyTime));
        fromYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndY, endKeyTime, Easing));

        // To
        var toTransform = new TranslateTransform(toStartX, toStartY);
        toSlide.RenderTransform = toTransform;
        var toXAnimation = new DoubleAnimationUsingKeyFrames();
        toXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartX, zeroKeyTime));
        toXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndX, endKeyTime, Easing));
        var toYAnimation = new DoubleAnimationUsingKeyFrames();
        toYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartY, zeroKeyTime));
        toYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndY, endKeyTime, Easing));

        // Set up events
        fromXAnimation.Completed += (sender, args) =>
        {
            fromTransform.BeginAnimation(TranslateTransform.XProperty, null);
            fromTransform.X = fromEndX;
            fromSlide.RenderTransform = null;
            if (Direction == SlideDirection.Left || Direction == SlideDirection.Right) onComplete();
        };
        fromYAnimation.Completed += (sender, args) =>
        {
            fromTransform.BeginAnimation(TranslateTransform.YProperty, null);
            fromTransform.Y = fromEndY;
            fromSlide.RenderTransform = null;
            if (Direction == SlideDirection.Up || Direction == SlideDirection.Down) onComplete();
        };
        toXAnimation.Completed += (sender, args) =>
        {
            toTransform.BeginAnimation(TranslateTransform.XProperty, null);
            toTransform.X = toEndX;
            toSlide.RenderTransform = null;
        };
        toYAnimation.Completed += (sender, args) =>
        {
            toTransform.BeginAnimation(TranslateTransform.YProperty, null);
            toTransform.Y = toEndY;
            toSlide.RenderTransform = null;
        };

        // Animate
        fromTransform.BeginAnimation(TranslateTransform.XProperty, fromXAnimation);
        fromTransform.BeginAnimation(TranslateTransform.YProperty, fromYAnimation);
        toTransform.BeginAnimation(TranslateTransform.XProperty, toXAnimation);
        toTransform.BeginAnimation(TranslateTransform.YProperty, toYAnimation);
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}