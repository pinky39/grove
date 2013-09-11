namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class SickAndTired : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sick and Tired")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text("Two target creatures each get -1/-1 until end of turn.")
        .FlavorText("The Phyrexians' only interest in organic life is discerning its weakness.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-1, -1) {UntilEot = true}) {ToughnessReduction = 1};

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectReduceToughness(1));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.ReduceToughness));
          });
    }
  }
}