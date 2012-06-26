namespace Grove.Core.CardDsl
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public static class CardFactoryEx
  {
    public static Card.CardFactory Leveler(this Card.CardFactory card, CardCreationCtx ctx, IManaAmount cost,
                                           params LevelDefinition[] levels)
    {
      var abilities = new List<object>();      
      
      abilities.Add(
        ctx.ActivatedAbility(
          String.Format("{0}: Put a level counter on this. Level up only as sorcery.", cost),
          ctx.Cost<TapOwnerPayMana>((cst, _) => cst.Amount = cost),
          ctx.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(c.Modifier<IncreaseLevel>())),
          timing: Timings.Leveler(cost, levels), activateAsSorcery: true));


      foreach (var levelDefinition in levels)
      {
        var definition = levelDefinition;

        abilities.Add(
          ctx.StaticAbility(
            ctx.Trigger<OnLevelChanged>((c, _) => c.Level = definition.Min),
            ctx.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = definition.StaticAbility,
                minLevel: definition.Min, maxLevel: definition.Max),
              c.Modifier<SetPowerAndToughness>((m, _) =>
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