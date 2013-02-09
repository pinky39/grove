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

  public class FaithHealer : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Faith Healer")
        .ManaCost("{1}{W}")
        .Type("Creature Human Cleric")
        .Text("Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.")
        .FlavorText("The power of faith is quiet. It is the leaf unmoved by the hurricane.")
        .Power(1)
        .Toughness(1)
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.";

            p.Cost = new Sacrifice();
            p.Effect = () => new ControllerGainsLife(e => e.Target.Card().ConvertedCost);

            p.TargetSelector.AddCost(trg =>
              {
                trg
                  .Is.Card(pr => pr.Effect.Controller == pr.Target.Card().Controller)
                  .On.Battlefield();

                trg.MustBeTargetable = false;
                trg.Text = "Select an enchantment to sacrifice.";
              });

            p.TargetingRule(new SacrificeToGainLife());
          });
    }
  }
}