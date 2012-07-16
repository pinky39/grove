﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Sicken : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sicken")
        .ManaCost("{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature gets -1/-1.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Urza dared to attack Phyrexia. Slowly, it retaliated.")
        .Category(EffectCategories.ToughnessReduction)
        .Cycling("{2}")
        .Effect<EnchantCreature>((e, c) => e.Modifiers(c.Modifier<AddPowerAndToughness>((m, _) =>
          {
            m.Power = -1;
            m.Toughness = -1;
          })))
        .Timing(Timings.MainPhases())
        .Targets(
          filter: TargetFilters.ReduceToughness(1),
          effect: C.Selector(Selectors.EnchantedCreature())
        );
    }
  }
}