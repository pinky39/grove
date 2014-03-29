namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
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
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Junk Diver dies, return another target artifact card from your graveyard to your hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new PutTargetsToBattlefield();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).In.YourGraveyard());
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          });
    }
  }
}