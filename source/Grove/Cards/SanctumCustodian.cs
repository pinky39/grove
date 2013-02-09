namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Preventions;
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
        .Abilities(
          ActivatedAbility(
            "{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.",
            Cost<Tap>(),
            Effect<Core.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddDamagePrevention>(m =>
                {
                  m.Prevention = Prevention<PreventDamage>(p =>
                    {                      
                      p.Amount = 2;
                    });
                }, untilEndOfTurn: true))), 
            Target(Validators.CreatureOrPlayer(), Zones.Battlefield()),
            targetingAi: TargetingAi.PreventNextDamageToCreatureOrPlayer(2))
        );
    }
  }
}