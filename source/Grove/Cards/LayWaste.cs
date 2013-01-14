namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Targeting;

  public class LayWaste : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lay Waste")
        .ManaCost("{3}{R}")
        .Type("Sorcery")
        .Text("Destroy target land.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyTargetPermanents>();
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Land),
              Zones.Battlefield()));
            p.TargetingAi = TargetingAi.Destroy();
          });
    }
  }
}