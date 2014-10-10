namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class UnmakeTheGraves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unmake the Graves")
        .ManaCost("{4}{B}")
        .Type("Instant")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Return up to two target creature cards from your graveyard to your hand.")
        .FlavorText("\"I'm raising an army. Any volunteers?\"")
        .Convoke()
        .Cast(p =>
        {
          p.Text = "Return up to two target creature cards from your graveyard to your hand.";
          p.Effect = () => new ReturnToHand();
          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 0;
            trg.MaxCount = 2;
            trg.Is.Creature().On.YourGraveyard();
          });

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          p.TimingRule(new WhenYourGraveyardCountIs(c => c.Is().Creature));
        });
    }
  }
}
