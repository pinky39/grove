namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Targeting;

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
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.";

            p.Cost = new Sacrifice();
            p.Effect = () => new ControllerGainsLife(P(e => e.Target.Card().ConvertedCost));

            p.TargetSelector.AddCost(trg =>
              {
                trg
                  .Is.Card(x => x.Is().Enchantment, controlledBy: ControlledBy.SpellOwner)
                  .On.Battlefield();

                trg.Message = "Select an enchantment to sacrifice.";
              });

            p.TargetingRule(new SacrificeToGainLife());
          });
    }
  }
}