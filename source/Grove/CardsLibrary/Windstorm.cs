namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.CostRules;
  using AI.TimingRules;
  using Effects;

  public class Windstorm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Windstorm")
        .ManaCost("{G}").HasXInCost()
        .Type("Instant")
        .Text("Windstorm deals X damage to each creature with flying.")
        .FlavorText("\"When the last dragon fell, its spirit escaped as a roar into the wind.\"{EOL}—Temur tale")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToCreaturesAndPlayers(
            amountCreature: (e, creature) => e.X.GetValueOrDefault(),
            filterCreature: (effect, card) => card.Has().Flying);

          p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
          p.CostRule(new XIsAvailableMana());
        });
    }
  }
}
