namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class CleverImpersonator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Clever Impersonator")
        .ManaCost("{2}{U}{U}")
        .Type("Creature - Shapeshifter")
        .Text(
          "You may have Clever Impersonator enter the battlefield as a copy of any nonland permanent on the battlefield.")
        .FlavorText("\"Our own selves are the greatest obstacles to enlightenment.\"{EOL}—Narset, khan of the Jeskai")
        .Power(0)
        .Toughness(0)
        .Cast(p => p.TimingRule(new WhenPermanentCountIs(1, c => !c.Is().Land)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "You may have Clever Impersonator enter the battlefield as a copy of any nonland permanent on the battlefield.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.UsesStack = false;

            p.Effect = () => new BecomeCopyOfTargetCard();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => !c.Is().Land, canTargetSelf: false).On.Battlefield(),
              trg => trg.MustBeTargetable = false);

            p.TargetingRule(new EffectOrCostRankBy(x => -x.Score));
          });
    }
  }
}