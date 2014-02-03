namespace Grove.UserInterface.LoadScreen
{
  using System.Threading.Tasks;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    public string LoadingMessage { get { return ThinkingMessages.GetRandom(); } }
    public virtual long Completed { get; protected set; }

    public override void Initialize()
    {
      Task.Factory.StartNew(() =>
        {
          MediaLibrary.LoadAll(ShowProgress);
          InitializeCardDatabase();
        })
        .ContinueWith(tsk =>
          {
            var startScreen = ViewModels.StartScreen.Create();
            Shell.ChangeScreen(startScreen);
          }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void ShowProgress(long current, long total)
    {
      var completed = current * 100/total;
      
      if (completed > Completed)
      {
        Completed = completed;
      }      
    }

    private void InitializeCardDatabase()
    {
      var cards = CardFactory.CreateAll();
      CardDatabase.Initialize(cards);
    }
  }
}