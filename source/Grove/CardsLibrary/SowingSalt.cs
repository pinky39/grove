namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class SowingSalt : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sowing Salt")
        .ManaCost("{2}{R}{R}")
        .Type("Sorcery")
        .Text(
          "Exile target nonbasic land. Search its controller's graveyard, hand, and library for all cards with the same name as that land and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ExileTargets(),
              new ExileCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());
         
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectExileBattlefield());         
          });
    }
  }
}