namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class IvorytuskFortress : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ivorytusk Fortress")
        .ManaCost("{2}{W}{B}{G}")
        .Type("Creature - Elephant")
        .Text("Untap each creature you control with a +1/+1 counter on it during each other player's untap step.")
        .FlavorText("Abzan soldiers march to war confident that their Houses march with them.")
        .Power(5)
        .Toughness(7)
        .TriggeredAbility(p =>
        {
          p.Text = "Untap each creature you control with a +1/+1 counter on it during each other player's untap step.";
          p.Trigger(new OnStepStart(Step.Untap, activeTurn: false, passiveTurn: true));
          p.Effect = () => new UntapEachPermanent(
            filter: c => c.Is().Creature && c.CountersCount(CounterType.PowerToughness) > 0,
            controlledBy: ControlledBy.SpellOwner);
        });
    }
  }
}
