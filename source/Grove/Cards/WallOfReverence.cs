namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;

  public class WallOfReverence : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Wall of Reverence")
        .ManaCost("{3}{W}")
        .Type("Creature Spirit Wall")
        .Text(
          "{Defender}, {Flying}{EOL}At the beginning of your end step, you may gain life equal to the power of target creature you control.")
        .FlavorText(
          "The lives of elves are long, but their memories are longer. Even after death, they do not desert their homes.")
        .Power(1)
        .Toughness(6)        
        .Abilities(
          Static.Defender,
          Static.Flying,
          TriggeredAbility(
            "At the beginning of your end step, you may gain life equal to the power of target creature you control.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.EndOfTurn; }),
            Effect<ControllerGainsLife>(e => e.Amount = e.Target().Card().Power.GetValueOrDefault()),
            Target(Validators.Card(card => card.Is().Creature, controller: Controller.SpellOwner),
              Zones.Battlefield()), 
            TargetSelectorAi.CreatureWithGreatestPower(),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}