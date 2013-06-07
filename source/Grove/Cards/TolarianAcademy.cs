namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class TolarianAcademy : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Tolarian Academy")
        .Type("Legendary Land")
        .Text("{T}: Add {U} to your mana pool for each artifact you control.")
        .FlavorText("The academy worked with time—until time ran out.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} to your mana pool for each artifact you control.";
            p.ManaAmount(ManaColor.Blue, c => c.Is().Artifact);
          }
        );
    }
  }
}