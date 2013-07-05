namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class WingSnare : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.TargetingRule(new Destroy());
            p.TimingRule(new FirstMain());
          });
    }
  }
}