namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;

  public class Crosswinds : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Crosswinds")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text("Creatures with flying get -2/-0.")
        .FlavorText("Harbin's ornithopter had been trapped for two days within the currents of the storm. When the skies cleared, all he could see was a horizon of trees.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddPowerAndToughness>(
                (m, _) =>
                  {
                    m.Power = -2;                    
                  });
              e.CardFilter = (card, source) => card.Is().Creature && card.Has().Flying;
            }));
        
      
    }
  }
}