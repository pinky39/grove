namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Preventions;
  using Core.Dsl;
  using Core.Targeting;

  public class SanctumCustodian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sanctum Custodian")
        .ManaCost("{2}{W}")
        .Type("Creature Human Cleric")
        .Text("{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.")
        .FlavorText("Serra told them to guard Urza as he healed. Five years they stood.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.TapOwner = true),
            C.Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
              c.Modifier<AddDamagePrevention>((m, c0) =>
                {
                  m.Prevention = c0.Prevention<PreventDamageToTarget>((p, _) =>
                    {
                      p.AmountIsDepletable = true;
                      p.Amount = 2;
                    });
                }, untilEndOfTurn: true))),
            effectSelector: C.Selector(Selectors.CreatureOrPlayer()),
            targetFilter: TargetFilters.PreventNextDamageToCreatureOrPlayer(2)
            )
        );
    }
  }
}