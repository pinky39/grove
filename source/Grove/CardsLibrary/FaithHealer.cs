namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class FaithHealer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Faith Healer")
        .ManaCost("{1}{W}")
        .Type("Creature Human Cleric")
        .Text("Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.")
        .FlavorText("The power of faith is quiet. It is the leaf unmoved by the hurricane.")
        .Power(1)
        .Toughness(1)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.";

            p.Cost = new Sacrifice();
            p.Effect = () => new ChangeLife(amount: P(e => e.Target.Card().ConvertedCost), whos: P(e => e.Controller));

            p.TargetSelector.AddCost(trg =>
              {
                trg
                  .Is.Card(x => x.Is().Enchantment, controlledBy: ControlledBy.SpellOwner)
                  .On.Battlefield();

                trg.Message = "Select an enchantment to sacrifice.";
              });

            p.TargetingRule(new CostSacrificeToGainLife());
          });
    }
  }
}