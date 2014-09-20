namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class AngelsTrumpet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Angel's Trumpet")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text(
          "All creatures have vigilance.{EOL}At the beginning of each player's end step, tap all untapped creatures that player controls that didn't attack this turn. Angel's Trumpet deals damage to the player equal to the number of creatures tapped this way.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .ContinuousEffect(p =>
          {
            p.CardFilter = (card, source) => card.Is().Creature;
            p.Modifier = () => new AddStaticAbility(Static.Vigilance);
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's end step, tap all untapped creatures that player controls that didn't attack this turn. Angel's Trumpet deals damage to the player equal to the number of creatures tapped this way.";
            p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true));
            p.Effect = () => new TapCreaturesThatDidntAttackDamagePlayer();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}