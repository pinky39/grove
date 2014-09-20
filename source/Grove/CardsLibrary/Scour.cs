namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Scour : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scour")
        .ManaCost("{2}{W}{W}")
        .Type("Instant")
        .Text("Exile target enchantment. Search its controller's graveyard, hand, and library for all cards with the same name as that enchantment and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ExileCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Enchantment).On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Exile));
          });
    }
  }
}