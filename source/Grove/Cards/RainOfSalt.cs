namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class RainOfSalt : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rain of Salt")
        .ManaCost("{4}{R}{R}")
        .Type("Sorcery")
        .Text("Destroy two target lands.")
        .FlavorText("Here, rain does not wash the land; it desiccates it.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Land).On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new FirstMain());
            p.TargetingRule(new Destroy());
          });
    }
  }
}