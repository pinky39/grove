namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class CitanulCentaurs : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Citanul Centaurs")
        .ManaCost("{3}{G}")
        .Type("Creature Centaur")
        .Text(
          "{Shroud} (This permanent can't be the target of spells or abilities.){EOL}{Echo} {3}{G}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .Power(6)
        .Toughness(3)
        .Echo("{3}{G}")
        .Abilities(
          Static.Shroud
        );
    }
  }
}