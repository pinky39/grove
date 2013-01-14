namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class ManaLeech : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Mana Leech")
        .ManaCost("{2}{B}")
        .Type("Creature Leech")
        .Text(
          "You may choose not to untap Mana Leech during your untap step.{EOL}{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.")
        .Power(1)
        .Toughness(1)
        .MayChooseNotToUntapDuringUntap()
        .Abilities(
          ActivatedAbility(
            "{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.",
            Cost<Tap>(),
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
            Target(Validators.Card(x => x.Is().Land), Zones.Battlefield()),
            targetingAi: TargetingAi.TapLand(),
            timing: All(Timings.Turn(passive: true), Timings.Steps(Step.Upkeep)))
        );
    }
  }
}