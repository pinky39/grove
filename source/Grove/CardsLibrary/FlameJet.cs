namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;


  public class FlameJet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Flame Jet")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Flame Jet deals 3 damage to target player.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Did the land make the Keldons? Did the Keldons make the land? Or were they simply meant for each other?")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectDealDamage(3));
            p.TimingRule(new OnSecondMain());
          });
    }
  }
}