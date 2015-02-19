namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.RepetitionRules;
  using Grove.AI.TimingRules;
  using Grove.Infrastructure;
  using Grove.Triggers;

  public class Pestilence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pestilence")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.{EOL}{B}: Pestilence deals 1 damage to each creature and each player.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, if no creatures are on the battlefield, sacrifice Pestilence.";
            p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) => g.Players.Permanents().None(x => x.Is().Creature)
              });
            p.Effect = () => new SacrificeOwner();

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Pestilence deals 1 damage to each creature and each player.";
            p.Cost = new PayMana(Mana.Black, supportsRepetitions: true);
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountCreature: 1,
              amountPlayer: 1);

            p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
            p.RepetitionRule(new RepeatForOptimalMassDamage());            
          });
    }
  }
}