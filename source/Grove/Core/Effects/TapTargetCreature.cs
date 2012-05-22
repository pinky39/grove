namespace Grove.Core.Effects
{
  public class TapTargetCreature : Effect
  {
    public override void Resolve()
    {
      Target.Card().Tap();
    }
  }
}