namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class SkirgeFamiliar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Skirge Familiar")
        .ManaCost("{4}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}Discard a card: Add {B} to your mana pool.")
        .FlavorText("The first Yawgmoth priest to harness their power was rewarded with several unique mutilations.")
        .Power(3)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "Discard a card: Add {B} to your mana pool.";
            p.Cost = new DiscardTarget();
            p.Effect = () => new AddManaToPool(Mana.Black);
            p.TargetSelector.AddCost(trg => trg.Is.Card().In.OwnersHand());

            p.TimingRule(new WhenYouNeedAdditionalMana(1));
            p.TargetingRule(new CostDiscardCard());
            p.UsesStack = false;
          });
    }
  }
}