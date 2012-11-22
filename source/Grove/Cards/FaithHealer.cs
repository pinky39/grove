namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class FaithHealer : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Faith Healer")
        .ManaCost("{1}{W}")
        .Type("Creature Human Cleric")
        .Text("Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.")
        .FlavorText("The power of faith is quiet. It is the leaf unmoved by the hurricane.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "Sacrifice an enchantment: You gain life equal to the sacrificed enchantment's converted mana cost.",
            Cost<SacPermanent>(),
            Effect<GainLife>(e => e.Amount = e.Target().Card().ConvertedCost),
            costValidator:
              TargetValidator(TargetIs.Card(
                controller: Controller.SpellOwner,
                filter: card => card.Is().Enchantment),
                ZoneIs.Battlefield(),
                text: "Select an enchantment to sacrifice.", mustBeTargetable: false),
            targetSelectorAi: TargetSelectorAi.CostSacrificeGainLife()
            )
        );
    }
  }
}