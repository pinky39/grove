namespace Grove.Core.Ai
{
  public abstract class MachinePlayRule : GameObject
  {
    public abstract void Process(ActivationContext context);

    public virtual void Initialize(Game game)
    {
      Game = game;
    }
  }
}