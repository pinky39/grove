namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class CruelEdict : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Cruel Edict")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Target opponent sacrifices a creature.")
        .FlavorText("'Choose your next words carefully. They will be your last.'{EOL}—Phage the Untouchable")
        .Category(EffectCategories.Destruction)
        .Timing(Timings.RemovalPlayerChoosesCreatures(1))
        .Effect<OpponentSacrificesCreatures>(e => e.Count = 1);
    }
  }
}