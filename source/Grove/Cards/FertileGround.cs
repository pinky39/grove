namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class FertileGround : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fertile Ground")
        .ManaCost("{1}{G}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant land}{EOL}Whenever enchanted land is tapped for mana, its controller adds one mana of any color to his or her mana pool.")
        .FlavorText("The forest was too lush for the brothers to despoil—almost.")
        .Effect<Attach>(e => e.Modifiers(Modifier<IncreaseManaOutput>(m => m.Amount = ManaAmount.Any)))
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.EnchantYourLand(),
          effectValidator: Validator(Validators.EnchantedPermanent(card => card.Is().Land)));
    }
  }
}