namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class BaneslayerAngel : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Baneslayer Angel")
        .ManaCost("{3}{W}{W}")
        .Type("Creature - Angel")
        .Text("{Flying}, {First strike}, {Lifelink}{EOL}Baneslayer Angel has protection from Demons and from Dragons.")
        .FlavorText("Some angels protect the meek and innocent. Others seek out and smite evil wherever it lurks.")
        .Power(5)
        .Toughness(5)
        .Protections("demon", "dragon")
        .StaticAbilities(
          Static.Flying,
          Static.FirstStrike,
          Static.Lifelink
        );
    }
  }
}