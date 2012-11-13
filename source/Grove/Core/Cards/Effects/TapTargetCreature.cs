namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

  public class TapTargetCreature : Effect
  {    
    protected override void ResolveEffect()
    {
      Target().Card().Tap();
    }
  }
}