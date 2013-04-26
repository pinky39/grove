namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class SkirgeFamiliar : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Skirge Familiar")
        .ManaCost("{4}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}Discard a card: Add {B} to your mana pool.")
        .FlavorText("The first Yawgmoth priest to harness their power was rewarded with several unique mutilations.")
        .Power(3)
        .Toughness(2)
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "Discard a card: Add {B} to your mana pool.";
            p.Cost = new DiscardTarget();
            p.Effect = () => new AddManaToPool(Mana.Black);
            p.TargetSelector.AddCost(trg => trg.Is.Card().In.OwnersHand());

            p.TimingRule(new ControllerNeedsAdditionalMana(1));
            p.TargetingRule(new OrderByRank(c => c.Score));
            p.UsesStack = false;
          });
    }
  }
}