namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Targeting;

  public class WizardMentor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Wizard Mentor")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Return Wizard Mentor and target creature you control to their owner's hand.")
        .FlavorText(
          "Although some of the students quickly grasped the concept, the others could summon only blackboards.")
        .Power(2)
        .Toughness(2)
        .Abilities(
          ActivatedAbility(
            "{T}: Return Wizard Mentor and target creature you control to their owner's hand.",
            Cost<Tap>(),
            Effect<Core.Effects.ReturnToHand>(e => { e.ReturnOwner = true; }),
            Target(Validators.Card(ControlledBy.SpellOwner, card => card.Is().Creature), Zones.Battlefield()),
            targetingAi: TargetingAi.BounceSelfAndTargetCreatureYouControl(),
            timing: Timings.NoRestrictions()
            )
        );
    }
  }
}