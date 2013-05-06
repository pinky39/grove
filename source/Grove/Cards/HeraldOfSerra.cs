namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class HeraldOfSerra : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Herald of Serra")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Angel")
        .Text(
          "{Flying}; {echo} (During your next upkeep after this permanent comes under your control, pay its casting cost or sacrifice it.){EOL}Attacking does not cause Herald of Serra to tap.")
        .Power(3)
        .Toughness(4)
        .Echo("{2}{W}{W}")
        .StaticAbilities(
          Static.Vigilance,
          Static.Flying
        );
    }
  }
}