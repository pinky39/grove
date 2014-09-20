namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Modifiers;

  public class MultaniMaroSorcerer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Multani, Maro-Sorcerer")
        .ManaCost("{4}{G}{G}")
        .Type("Legendary Creature Elemental")
        .Text(
          "{Shroud}{EOL}Multani, Maro-Sorcerer's power and toughness are each equal to the total number of cards in all players' hands.")
        .FlavorText("To make peace with the forest, make peace with me.")
        .Power(0)
        .Toughness(0)
        .SimpleAbilities(Static.Shroud)
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessEqualToTotalHandsCount());
            p.EnabledInAllZones = true;
          });
    }
  }
}