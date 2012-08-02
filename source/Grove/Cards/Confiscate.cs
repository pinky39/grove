namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Confiscate : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Confiscate")
        .ManaCost("{4}{U}{U}")
        .Type("Enchantment Aura")
        .Text("You control enchanted permanent.")
        .FlavorText(
          "'I don't understand why he works so hard on a device to duplicate a sound so easily made with hand and armpit.'{EOL}—Barrin, progress report")
        .Timing(Timings.FirstMain())
        .Effect<EnchantCreature>(p => p.Effect.Modifiers(
          p.Builder.Modifier<ChangeController>((m, c) => m.NewController = m.Source.Controller)))
        .Targets(
          selectorAi: TargetSelectorAi.GainControl(),
          effectValidator: C.Validator(Validators.Permanent())
        );
    }
  }
}