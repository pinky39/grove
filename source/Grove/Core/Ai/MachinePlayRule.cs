namespace Grove.Core.Ai
{
  public abstract class MachinePlayRule : GameObject
  {
    public abstract void Process(ActivationContext c);

    public virtual void Initialize(Game game)
    {
      Game = game;
    }    
  }
}