namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class WingSnare : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wing Snare")
        .ManaCost("{2}{G}")
        .Type("Sorcery")
        .Text("Destroy target creature with flying.")
        .FlavorText("Argoth's doom rained from a clear sky. Yavimaya will not share that fate.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.Has().Flying)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}