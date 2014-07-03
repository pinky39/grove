namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class JunkDiver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Junk Diver")
        .ManaCost("{3}")
        .Type("Artifact Creature Bird")
        .Text("{Flying}{EOL}When Junk Diver dies, return another target artifact card from your graveyard to your hand.")
        .FlavorText("Garbage in, treasure out.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Junk Diver dies, return another target artifact card from your graveyard to your hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand();
            
            p.TargetSelector.AddEffect(trg => trg.Is
              .Card(p1 => p1.Target.Is().Artifact && p1.OwningCard != p1.Target)
              .In.YourGraveyard());
            
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          });
    }
  }
}