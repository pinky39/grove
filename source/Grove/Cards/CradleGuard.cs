namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class CradleGuard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cradle Guard")
        .ManaCost("{1}{G}{G}")
        .Type("Creature Treefolk")
        .Text(
          "{Trample}, {Echo} {1}{G}{G} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText("Mother, sleep / Dream our lives{EOL}Our roots your soul / Our leaves your bed.")
        .Power(4)
        .Toughness(4)
        .Echo("{1}{G}{G}")
        .SimpleAbilities(Static.Trample);
    }
  }
}