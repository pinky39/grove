namespace Grove.UserInterface
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Interactivity;
  using System.Windows.Media.Animation;

  public class AnimationBehaviour : Behavior<FrameworkElement>
  {
    public static readonly DependencyProperty AnimationControllerProperty =
      DependencyProperty.Register(
        "Animation",
        typeof (Animation),
        typeof (AnimationBehaviour),
        new PropertyMetadata(null, null));

    public static readonly DependencyProperty StoryProperty =
      DependencyProperty.Register(
        "Storyboard",
        typeof (Storyboard),
        typeof (AnimationBehaviour),
        new PropertyMetadata(null, null));

    public Animation Animation { get { return (Animation) GetValue(AnimationControllerProperty); } set { SetValue(AnimationControllerProperty, value); } }
    public Storyboard Story { get { return (Storyboard) GetValue(StoryProperty); } set { SetValue(StoryProperty, value); } }

    protected override void OnAttached()
    {
      Story.Completed += OnStoryBoardCompleted;


      AssociatedObject.Loaded += delegate
        {
          var ipc = ((INotifyPropertyChanged) Animation);
          ipc.PropertyChanged += delegate { Story.Begin(); };
        };
    }

    private void OnStoryBoardCompleted(object sender, EventArgs e)
    {
      Animation.Stop();
    }
  }
}