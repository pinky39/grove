namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
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
            selectorAi: TargetSelectorAi.CounterSpell(),
            effectValidator: Validator(Validators.Counterspell(card => card.HasColors(ManaColors.Red))),
            category: EffectCategories.Counterspell,
            timing: Timings.CounterSpell()
            )
        );
    }
  }
}