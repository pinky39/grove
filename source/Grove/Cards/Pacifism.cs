namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Pacifism : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Pacifism")
        .ManaCost("{1}{W}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature can't attack or block.")
        .FlavorText("'Fight? I cannot. I do not care if I live or die, so long as I can rest.'{EOL}—Urza, to Serra")
        .Timing(Timings.FirstMain())
        .Effect<EnchantCreature>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.CannotBlock),
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.CannotAttack)))
        .Targets(
          filter: TargetFilters.Pacifism(),
          effect: C.Selector(Selectors.EnchantedCreature()));
    }
  }
}