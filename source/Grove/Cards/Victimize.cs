namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Victimize : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Victimize")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text(
          "Choose two target creature cards in your graveyard. Sacrifice a creature. If you do, return the chosen cards to the battlefield tapped.")
        .FlavorText("The priest cast Xantcha to the ground. 'It is defective. We must scrap it.'")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsToBattlefield(mustSacCreatureOnResolve: true, tapped: true);
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().In.YourGraveyard();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new SecondMain());
            p.TimingRule(new ControllerHasPermanents(c => c.Is().Creature));
            p.TargetingRule(new OrderByRank(c => -c.Score));
          });
    }
  }
}