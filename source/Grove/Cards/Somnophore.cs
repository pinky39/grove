namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Messages;
  using Core.Targeting;

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
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.",
            Trigger<DealDamageToCreatureOrPlayer>(t => t.ToPlayer()),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<TapTargetCreature>(),
              Effect<ApplyModifiersToTargets>(e1 => e1.Modifiers(
                Modifier<AddStaticAbility>(m =>
                  {
                    m.StaticAbility = Static.DoesNotUntap;
                    m.AddLifetime(Lifetime<PermanentLeavesBattlefieldLifetime>(l => l.Permanent = m.Source));
                  })
                ))
              )),                        
            selectorAi: TargetSelectorAi.Pacifism(),
            effectValidator: Validator(
              Validators.Creature(p => p.Target.Card().Controller == p.Trigger<DamageHasBeenDealt>().Receiver), 
              text: "Select creature to tap."))
        );
    }
  }
}