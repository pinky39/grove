namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Impatience : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Impatience")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of each player's end step, if that player didn't cast a spell this turn, Impatience deals 2 damage to him or her.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's end step, if that player didn't cast a spell this turn, Impatience deals 2 damage to him or her.";

            p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) => g.Turn.Events.HasActivePlayerPlayedAnySpell == false
              });

            p.Effect = () => new DealDamageToPlayer(
              amount: 2,
              player: P((e, g) => g.Players.Active));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}