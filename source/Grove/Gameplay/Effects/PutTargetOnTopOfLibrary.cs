namespace Grove.Gameplay.Effects
{
  using Targeting;

  public class PutTargetOnTopOfLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Target.Card().PutOnTopOfLibrary();
    }
  }
}