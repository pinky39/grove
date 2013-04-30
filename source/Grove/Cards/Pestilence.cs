﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.States;
  using Infrastructure;

  public class Pestilence : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Pestilence")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.{EOL}{B}: Pestilence deals 1 damage to each creature and each player.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.";
            p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) => g.Players.Permanents().None(x => x.Is().Creature)
              });
            p.Effect = () => new SacrificeOwner();

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Pestilence deals 1 damage to each creature and each player.";
            p.Cost = new PayMana(Mana.Black, ManaUsage.Abilities);
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountCreature: 1,
              amountPlayer: 1);

            p.TimingRule(new Any(new MassRemoval(), new EndOfTurn()));
          });
    }
  }
}