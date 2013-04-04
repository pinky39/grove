namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Targeting;

  public class GreaterGood : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Greater Good")
        .ManaCost("{2}{G}{G}")
        .Type("Enchantment")
        .Text("Sacrifice a creature: Draw cards equal to the sacrificed creature's power, then discard three cards.")
        .FlavorText("We have more sprouts than they have hands.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
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

            p.TargetingRule(new SacrificeToDrawCards(c => c.Power > 3));
          }
        );
    }
  }
}