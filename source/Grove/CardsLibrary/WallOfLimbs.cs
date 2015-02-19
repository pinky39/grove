namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class WallOfLimbs : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Limbs")
        .ManaCost("{2}{B}")
        .Type("Creature - Zombie Wall")
        .Text(
          "{Defender}{I}(This creature can't attack.){/I}{EOL}Whenever you gain life, put a +1/+1 counter on Wall of Limbs.{EOL}{5}{B}{B},Sacrifice Wall of Limbs: Target player loses X life, where X is Wall of Limbs's power.")
        .FlavorText(
          "\"If you cannot turn your enemy's strength to weakness, then make that strength your own.\"{EOL}—Gresha, warrior sage")
        .Power(0)
        .Toughness(3)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever you gain life, put a +1/+1 counter on Wall of Limbs.";

            p.Trigger(new OnLifeChanged(life => life.IsGain && life.IsYours));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{5}{B}{B},Sacrifice Wall of Limbs: Target player loses X life, where X is Wall of Limbs's power.";

            p.Cost = new AggregateCost(
              new PayMana("{5}{B}{B}".Parse()),
              new Sacrifice());

            p.Effect = () => new ChangeLife(
              P(e => -e.Source.OwningCard.Power.GetValueOrDefault()), 
              targetPlayers: true);

            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new Any(
              new OnEndOfOpponentsTurn(),
              new WhenOwningCardWillBeDestroyed()));

            p.TargetingRule(new EffectOpponent());
          });
    }
  }
}