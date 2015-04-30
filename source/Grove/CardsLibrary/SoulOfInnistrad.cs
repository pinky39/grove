namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;

  public class SoulOfInnistrad : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Innistrad")
        .ManaCost("{4}{B}{B}")
        .Type("Creature — Avatar")
        .Text("{Deathtouch}{EOL}{3}{B}{B}: Return up to three target creature cards from your graveyard to your hand.{EOL}{3}{B}{B}, Exile Soul of Innistrad from your graveyard: Return up to three target creature cards from your graveyard to your hand.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Deathtouch)
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{B}{B}: Return up to three target creature cards from your graveyard to your hand.";
          p.Cost = new PayMana("{3}{B}{B}".Parse());

          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 0;
            trg.MaxCount = 3;
            trg.Is.Creature().On.YourGraveyard();
          });

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          
          p.TimingRule(new OnEndOfOpponentsTurn());
          p.TimingRule(new WhenYourGraveyardCountIs(c => c.Is().Creature));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{B}{B}, Exile Soul of Innistrad from your graveyard: Return up to three target creature cards from your graveyard to your hand.";
          p.Cost = new AggregateCost(
            new PayMana("{3}{B}{B}".Parse()),
            new Exile(fromGraveyard: true));

          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 0;
            trg.MaxCount = 3;
            trg.Is.Creature(canTargetSelf: false).In.YourGraveyard();            
          });

          p.ActivationZone = Zone.Graveyard;

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));

          p.TimingRule(new OnEndOfOpponentsTurn());
          p.TimingRule(new WhenYourGraveyardCountIs(c => c.Is().Creature, 3));
        });
    }
  }
}
