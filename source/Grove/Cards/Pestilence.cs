namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Infrastructure;

  public class Pestilence : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Pestilence")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.{EOL}{B}: Pestilence deals 1 damage to each creature and each player.")
        .Cast(p => p.Timing = All(Timings.FirstMain(), Timings.OnlyOneOfKind()))                  
        .Abilities(
          TriggeredAbility(
            "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.",
            Trigger<OnStepStart>(t =>
              {
                t.ActiveTurn = true;
                t.PassiveTurn = true;
                t.Step = Step.EndOfTurn;
                t.Condition = self => self.Game.Players.Permanents().None(x => x.Is().Creature);
              }),
            Effect<SacrificeSource>(), 
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{B}: Pestilence deals 1 damage to each creature and each player.",
            Cost<PayMana>(cost => cost.Amount = ManaAmount.Black),
            Effect<DealDamageToEach>(e =>
              {
                e.AmountCreature = 1;
                e.AmountPlayer = 1;
              }),
            timing: Any(Timings.MassRemovalInstantSpeed(), Timings.EndOfTurn()))
        );
    }
  }
}