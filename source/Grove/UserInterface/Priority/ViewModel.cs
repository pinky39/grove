namespace Grove.UserInterface.Priority
{
  using Gameplay.Decisions.Results;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IReceive<PlayableSelected>
  {
    public Playable Playable { get; private set; }

    public void Receive(PlayableSelected message)
    {
      Playable = message.Playable;
      this.Close();
    }

    public void PassPriority()
    {
      if (Shell.HasFocus(this) == false)
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