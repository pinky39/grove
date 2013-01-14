namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class CruelEdict : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Cruel Edict")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Target opponent sacrifices a creature.")
        .FlavorText("'Choose your next words carefully. They will be your last.'{EOL}—Phage the Untouchable")
        .Cast(p =>
          {
            p.Timing = Timings.NonInstantRemovalPlayerChooses(1);
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<OpponentSacrificesCreatures>(e => e.Count = 1);
          });
    }
  }
}