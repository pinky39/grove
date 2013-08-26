namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Whirlwind : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Whirlwind")
        .ManaCost("{2}{G}{G}")
        .Type("Sorcery")
        .Text("Destroy all creatures with flying.")
        .FlavorText("Urza tried to rule the air, but Gaea taught him that she controlled all the elements.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.Effect = () => new DestroyAllPermanents((e, c) => c.Is().Creature && c.Has().Flying);
          });
    }
  }
}