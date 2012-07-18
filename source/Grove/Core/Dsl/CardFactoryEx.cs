namespace Grove.Core.Dsl
{
  using System;
  using System.Collections.Generic;
  using Ai;
  using Details.Cards.Costs;
  using Details.Cards.Effects;
  using Details.Cards.Modifiers;
  using Details.Cards.Triggers;
  using Details.Mana;

  public static class CardFactoryEx
  {
    public static Card.CardFactory Leveler(this Card.CardFactory card, CardBuilder ctx, IManaAmount cost,
                                           EffectCategories category = EffectCategories.Generic,
                                           params LevelDefinition[] levels)
    {
      var abilities = new List<object>();

      abilities.Add(
        ctx.ActivatedAbility(
          String.Format("{0}: Put a level counter on this. Level up only as sorcery.", cost),
          ctx.Cost<TapOwnerPayMana>((cst, _) => cst.Amount = cost),
          ctx.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(p.Builder.Modifier<IncreaseLevel>())),
          timing: Timings.Leveler(cost, levels), activateAsSorcery: true, category: category));


      foreach (var levelDefinition in levels)
      {
        var definition = levelDefinition;

        abilities.Add(
          ctx.StaticAbility(
            ctx.Trigger<OnLevelChanged>((c, _) => c.Level = definition.Min),
            ctx.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = definition.StaticAbility,
                minLevel: definition.Min, maxLevel: definition.Max),
              p.Builder.Modifier<SetPowerAndToughness>((m, _) =>
                {
                  m.Power = definition.Power;
                  m.Tougness = definition.Thoughness;
                }, minLevel: definition.Min, maxLevel: definition.Max))))
          );
      }

      card.IsLeveler().Abilities(abilities.ToArray());

      return card;
    }
  }
}