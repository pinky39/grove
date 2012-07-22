namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Infrastructure;

  public class Pestilence : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Pestilence")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.{EOL}{B}: Pestilence deals 1 damage to each creature and each player.")
        .Timing(All(Timings.FirstMain(), Timings.OnlyOneOfKind()))
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.",
            C.Trigger<AtBegginingOfStep>((t, _) =>
              {
                t.ActiveTurn = true;
                t.PassiveTurn = true;
                t.Step = Step.EndOfTurn;
                t.Condition = self => self.Game.Players.Permanents().None(x => x.Is().Creature);
              }),
            C.Effect<SacrificeSource>(), 
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          C.ActivatedAbility(
            "{B}: Pestilence deals 1 damage to each creature and each player.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = ManaAmount.Black),
            C.Effect<DealDamageToEach>(e =>
              {
                e.AmountCreature = 1;
                e.AmountPlayer = 1;
              }),
            timing: Any(Timings.MassRemovalInstantSpeed(), Timings.EndOfTurn()))
        );
    }
  }
}