namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Targeting;

  public class Befoul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Befoul")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text("Destroy target land or nonblack creature. It can't be regenerated.")
        .FlavorText("'The land putrefied at its touch, turned into an oily bile in seconds.'{EOL}—Radiant, archangel")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<DestroyTargetPermanents>(e => e.AllowRegenerate = false);
            p.Category = EffectCategories.Destruction;
            p.EffectTargets = L(Target(
              Validators.Card(card => card.Is().Land || (card.Is().Creature && !card.HasColors(ManaColors.Black))),
              Zones.Battlefield()));
            p.TargetingAi = TargetingAi.Destroy();
          });
    }
  }
}