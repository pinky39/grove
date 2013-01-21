namespace Grove.Core.Decisions.Results
{
  using Infrastructure;

  [Copyable]
  public abstract class Playable
  {
    public ActivationParameters ActivationParameters = new ActivationParameters();
    public Card Card;
    public int Index;
    public virtual bool WasPriorityPassed { get { return false; } }
    public Player Controller { get { return Card.Controller; } }

    public virtual bool CanPlay()
    {
      return true;
    }

    public virtual void Play() {}
  }
}