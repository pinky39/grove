namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Modifiers;
  using Core.Targeting;
  using Core.Triggers;

  public class Somnophore : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Somnophore")
        .ManaCost("{2}{U}{U}")
        .Type("Creature - Illusion")
        .Text(
          "{Flying}{EOL}Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.")
        .Power(2)
        .Toughness(2)        
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.",
            Trigger<OnDamageDealt>(t => t.ToPlayer()),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<TapTarget>(),
              Effect<ApplyModifiersToTargets>(e1 => e1.Modifiers(
                Modifier<AddStaticAbility>(m =>
                  {
                    m.StaticAbility = Static.DoesNotUntap;
                    m.AddLifetime(Lifetime<PermanentLeavesBattlefieldLifetime>(l => l.Permanent = m.Source));
                  })
                ))
              )),
            selectorAi: TargetingAi.Pacifism(),
            effectValidator: Target(
              Validators.Card(
                p => p.Card.Is().Creature && 
                p.Target.Card().Controller == p.Trigger<DamageHasBeenDealt>().Receiver),
              Zones.Battlefield(),
              text: "Select creature to tap."))
        );
    }
  }
}