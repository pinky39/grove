namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class VugLizard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vug Lizard")
        .ManaCost("{1}{R}{R}")
        .Type("Creature Lizard")
        .Text(
          "{Mountainwalk}{EOL}{Echo} {1}{R}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")        
        .Power(3)
        .Toughness(4)
        .Echo("{1}{R}{R}")
        .SimpleAbilities(Static.Mountainwalk);
    }
  }
}