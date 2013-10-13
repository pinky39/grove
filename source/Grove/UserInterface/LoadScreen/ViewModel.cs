namespace Grove.UserInterface.LoadScreen
{
  using System.Threading.Tasks;

  public class ViewModel : ViewModelBase
  {
    public string LoadingMessage { get { return ThinkingMessages.GetRandom(); } }

    public override void Initialize()
    {
      Task.Factory.StartNew(() =>
        {
          MediaLibrary.LoadResources();
          InitializeCardDatabase();
        })
        .ContinueWith(tsk =>
          {
            var startScreen = ViewModels.StartScreen.Create();
            Shell.ChangeScreen(startScreen);
          }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void InitializeCardDatabase()
    {
      var cards = CardFactory.CreateAll();
      CardDatabase.Initialize(cards);
    }
  }
}