namespace Grove.Ui.Priority
{
  using Core.Controllers.Results;
  using Infrastructure;
  using Shell;

  public class ViewModel : IReceive<PlayableSelected>
  {
    private readonly IShell _shell;

    public ViewModel(IShell shell)
    {
      _shell = shell;
    }

    public Playable Playable { get; private set; }

    public void Receive(PlayableSelected message)
    {
      Playable = message.Playable;
      this.Close();
    }

    public void PassPriority()
    {
      if (_shell.HasFocus(this) == false)
        return;

      Playable = new Pass();
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}