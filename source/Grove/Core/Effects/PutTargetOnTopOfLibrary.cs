namespace Grove.Effects
{
  public class PutTargetOnTopOfLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Target.Card().PutOnTopOfLibrary();
    }
  }
}