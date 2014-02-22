using System.Threading.Tasks;

namespace Grove.UserInterface.LoadScreen
{
  using Gameplay;

  public class ViewModel : ViewModelBase
  {
    public string LoadingMessage
    {
      get { return ThinkingMessages.GetRandom(); }
    }

    public virtual long Completed { get; protected set; }

    public override void Initialize()
    {
      Task.Factory.StartNew(() => MediaLibrary.LoadAll(ShowProgress))
          .ContinueWith(tsk =>
            {
              var startScreen = ViewModels.StartScreen.Create();
              Shell.ChangeScreen(startScreen);
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void ShowProgress(long current, long total)
    {
      var completed = current*100/total;

      if (completed > Completed)
      {
        Completed = completed;
      }
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}