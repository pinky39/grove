namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class DisownedAncestor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disowned Ancestor")
        .ManaCost("{B}")
        .Type("Creature - Spirit Warrior")
        .Text("Outlast {1}{B}{I}({1}{B}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}")
        .FlavorText("Long after death, the spirits of the Disowned continue to seek redemption among their Abzan kin.")
        .Power(0)
        .Toughness(4)
        .Outlast("{1}{B}");
    }
  }
}
