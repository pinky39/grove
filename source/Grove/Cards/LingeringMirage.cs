namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class LingeringMirage : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lingering Mirage")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text("{Enchant land}{EOL}Enchanted land is an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Birds frozen in flight. Sea turned to glass. Tolaria hidden in a mirror.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<ChangeBasicLand>(m => m.ChangeTo = "Island")));
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Land), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.EnchantLand(ControlledBy.Opponent);
          });
    }
  }
}