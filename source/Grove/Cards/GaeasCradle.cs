namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;

  public class GaeasCradle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Gaea's Cradle")
        .Type("Legendary Land")
        .Text("{T}: Add {G} to your mana pool for each creature you control.")
        .FlavorText(
          "'Here sprouted the first seedling of Argoth. Here the last tree will fall.'{EOL}—Gamelen, Citanul elder")        
        .Abilities(
          ManaAbility(
            (ability, game) =>
              ManaAmount.OfSingleColor(ManaColors.Green, ability.OwningCard.Controller.Battlefield.Creatures.Count()),
            "{T}: Add {G} to your mana pool for each creature you control.")
        );
    }
  }
}