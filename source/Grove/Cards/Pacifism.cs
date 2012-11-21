namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Pacifism : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Pacifism")
        .ManaCost("{1}{W}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature can't attack or block.")
        .FlavorText("'Fight? I cannot. I do not care if I live or die, so long as I can rest.'{EOL}—Urza, to Serra")
        .Timing(Timings.FirstMain())
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.CannotBlock),
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.CannotAttack)))
        .Targets(
          selectorAi: TargetSelectorAi.Pacifism(),
          effectValidator: TargetValidator(TargetIs.EnchantedCreature()));
    }
  }
}