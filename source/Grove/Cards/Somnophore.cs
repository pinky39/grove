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
      yield return C.Card
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
          C.TriggeredAbility(
            "Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, _) => t.ToPlayer()),
            C.Effect<CompoundEffect>(p => p.Effect.ChildEffects(
              p.Builder.Effect<TapTargetCreature>(),
              p.Builder.Effect<ApplyModifiersToTargets>(p1 => p1.Effect.Modifiers(
                p1.Builder.Modifier<AddStaticAbility>((m, c) =>
                  {
                    m.StaticAbility = Static.DoesNotUntap;
                    m.AddLifetime(new PermanentLeavesBattlefieldLifetime(m.Source, c.ChangeTracker));
                  })
                ))
              )),                        
            aiSelector: AiTargetSelectors.Pacifism(),
            effectValidator: C.Validator(Validators.Creature(
              p => p.Target.Card().Controller == p.Trigger<DamageHasBeenDealt>().Receiver)))
        );
    }
  }
}