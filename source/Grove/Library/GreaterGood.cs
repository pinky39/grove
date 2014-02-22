namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class GreaterGood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Greater Good")
        .ManaCost("{2}{G}{G}")
        .Type("Enchantment")
        .Text("Sacrifice a creature: Draw cards equal to the sacrificed creature's power, then discard three cards.")
        .FlavorText("We have more sprouts than they have hands.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice a creature: Draw cards equal to the sacrificed creature's power, then discard three cards.";

            p.Cost = new Sacrifice();
            p.Effect = () => new DrawCards(
              count: P(e => e.Target.Card().Power.GetValueOrDefault()),
              discardCount: 3);
            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select a creature to sacrifice.";
              });

            p.TargetingRule(new CostSacrificeToDrawCards(c => c.Power > 3));
          }
        );
    }
  }
}