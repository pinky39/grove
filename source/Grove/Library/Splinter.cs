namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Effects;

  public class Splinter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Splinter")
        .ManaCost("{2}{G}{G}")
        .Type("Sorcery")
        .Text(
          "Exile target artifact. Search its controller's graveyard, hand, and library for all cards with the same name as that artifact and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ExileCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());
          });
    }
  }
}