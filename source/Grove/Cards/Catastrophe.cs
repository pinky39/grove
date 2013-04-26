namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Catastrophe : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Catastrophe")
        .ManaCost("{4}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all lands or all creatures. Creatures destroyed this way can't be regenerated.")
        .FlavorText(
          "Radiant's eyes flashed. 'Go, then,' the angel spat at Serra, 'and leave this world to those who truly care.'")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllLandsOrCreatures();
            p.TimingRule(new SecondMain());
          });
    }
  }
}