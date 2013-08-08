namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class Antagonism : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Antagonism")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.";
            p.Trigger(new OnTurnOpponentWasNotDealtDamage());

            p.Effect = () => new DealDamageToPlayer(
              amount: 2,
              player: P((e, g) => g.Players.Active));
          });
    }
  }
}