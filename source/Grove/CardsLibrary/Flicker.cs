namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class Flicker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Flicker")
        .ManaCost("{1}{W}")
        .Type("Sorcery")
        .Text("Exile target nontoken permanent, then return it to the battlefield under its owner's control.")
        .FlavorText("Who is truer: you who are, or you who are to be?")
        .Cast(p =>
          {
            p.Effect = () => new ExileTargetThenPutIntoPlayUnderOwnersControl();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => !c.Is().Token).On.Battlefield());
            p.TargetingRule(new EffectFlicker());
          });
    }
  }
}