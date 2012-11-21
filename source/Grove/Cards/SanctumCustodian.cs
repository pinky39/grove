namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Dsl;
  using Core.Targeting;

  public class SanctumCustodian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sanctum Custodian")
        .ManaCost("{2}{W}")
        .Type("Creature Human Cleric")
        .Text("{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.")
        .FlavorText("Serra told them to guard Urza as he healed. Five years they stood.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.",
            Cost<TapOwnerPayMana>(cost => cost.TapOwner = true),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddDamagePrevention>(m =>
                {
                  m.Prevention = Prevention<PreventDamageToTarget>(p =>
                    {
                      p.AmountIsDepletable = true;
                      p.Amount = 2;
                    });
                }, untilEndOfTurn: true))),
            effectValidator: TargetValidator(TargetIs.CreatureOrPlayer()),
            targetSelectorAi: TargetSelectorAi.PreventNextDamageToCreatureOrPlayer(2)
            )
        );
    }
  }
}