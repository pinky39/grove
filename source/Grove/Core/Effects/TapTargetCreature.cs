namespace Grove.Core.Effects
{
  public class TapTargetCreature : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().Tap();
    }
  }
}