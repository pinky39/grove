namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Victimize : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Creature));
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          });
    }
  }
}