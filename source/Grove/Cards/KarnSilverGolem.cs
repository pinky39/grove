namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class KarnSilverGolem : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Karn, Silver Golem")
        .ManaCost("{5}")
        .Type("Legendary Artifact Creature Golem")
        .Text(
          "Whenever Karn, Silver Golem blocks or becomes blocked, it gets -4/+4 until end of turn.{EOL}{1}: Target noncreature artifact becomes an artifact creature with power and toughness each equal to its converted mana cost until end of turn.")
        .Power(4)
        .Toughness(4)
        .Abilities(
          TriggeredAbility(
            "Whenever Karn, Silver Golem blocks or becomes blocked, it gets -4/+4 until end of turn.",
            Trigger<OnBlock>(t =>
              {
                t.GetsBlocked = true;
                t.Blocks = true;
              }),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = -4;
                  m.Toughness = 4;
                }, untilEndOfTurn: true))),
            triggerOnlyIfOwningCardIsInPlay: true),
          ActivatedAbility(
            "{1}: Target noncreature artifact becomes an artifact creature with power and toughness each equal to its converted mana cost until end of turn.",
            Cost<PayMana>(c => c.Amount = 1.Colorless()),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  var target = m.Target.Card();

                  m.Power = target.ConvertedCost;
                  m.Toughness = target.ConvertedCost;
                  m.Type = target.Type + " Creature";
                }, untilEndOfTurn: true)
              )),
            Target(Validators.Card(c => c.Is().Artifact && !c.Is().Creature), Zones.Battlefield()),
            targetingAi: TargetingAi.GreatestConvertedManaCost(ControlledBy.SpellOwner),
            timing: Timings.ChangeToCreature()));
    }
  }
}