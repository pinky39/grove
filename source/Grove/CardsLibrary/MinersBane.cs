namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class MinersBane : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Miner's Bane")
        .ManaCost("{4}{R}{R}")
        .Type("Creature — Elemental")
        .Text(
          "{2}{R}: Miner's Bane gets +1/+0 and gains trample until end of turn. {I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
        .Power(6)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{R}: Miner's Bane gets +1/+0 and gains trample until end of turn.";
            p.Cost = new PayMana("{2}{R}".Parse(), supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(1, 0) {UntilEot = true},
              () => new AddStaticAbility(Static.Trample) {UntilEot = true});

            p.TimingRule(new AfterOpponentDeclaresBlockers());          
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}