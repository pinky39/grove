namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SiegeWurm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Siege Wurm")
        .ManaCost("{5}{G}{G}")
        .Type("Creature — Wurm")
        .Text(
          "{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}{Trample} (If this creature would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.)")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.Convoke, Static.Trample);
    }
  }
}