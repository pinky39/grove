namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class PathOfPeace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Path of Peace")
        .ManaCost("{3}{W}")
        .Type("Sorcery")
        .Text("Destroy target creature. Its owner gains 4 life.")
        .FlavorText(
          "When the sword becomes a burden, let the warrior lay it aside that another with a truer heart might take it up.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents
              {
                AfterResolve = e => e.Target.Card().Owner.Life += 4
              };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}