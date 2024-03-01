using System.Windows.Media.Animation;

namespace Rrs.Wpf.Navigation.Transitions;

internal static class AnimationHelper
{
    public static DoubleAnimationUsingKeyFrames GenerateOpacityAnimation(TimeSpan duration)
    {
        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new EasingDoubleKeyFrame(1, TimeSpan.Zero));
        animation.KeyFrames.Add(new EasingDoubleKeyFrame(1, TimeSpan.FromTicks(duration.Ticks / 2)));
        animation.KeyFrames.Add(new EasingDoubleKeyFrame(0, duration));
        return animation;
    }

    public static DoubleAnimationUsingKeyFrames GenerateScaleAnimation(TimeSpan duration)
    {
        var scaleAnimation = new DoubleAnimationUsingKeyFrames();
        scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.Zero));
        scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, duration));
        return scaleAnimation;
    }
}
