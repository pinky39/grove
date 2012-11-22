namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class Douse : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Douse")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("{1}{U}: Counter target red spell.")
        .FlavorText(
          "The academy's libraries were protected by fire-prevention spells. Even after the disaster, the books were intact—though forever sealed in time.")
        .Timing(Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{1}{U}: Counter target red spell.",
            Cost<TapOwnerPayMana>(c => c.Amount = "{1}{U}".ParseManaAmount()),
            Effect<CounterTargetSpell>(),
            targetSelectorAi: TargetSelectorAi.CounterSpell(),
            effectValidator: TargetValidator(
              TargetIs.CounterableSpell(card => card.HasColors(ManaColors.Red)),
              ZoneIs.Stack()),
            category: EffectCategories.Counterspell,
            timing: Timings.CounterSpell()
            )
        );
    }
  }
}