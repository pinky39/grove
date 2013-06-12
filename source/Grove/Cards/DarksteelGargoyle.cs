namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class DarksteelGargoyle : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Darksteel Gargoyle")
        .ManaCost("{7}")
        .Type("Artifact Creature - Gargoyle")
        .Text(
          "{Flying}{EOL}Darksteel Gargoyle is indestructible.{EOL}('Destroy' effects and lethal damage don't destroy it.)")
        .FlavorText("The ultimate treasure is one that guards itself.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(
          Static.Flying,
          Static.Indestructible
        );
    }
  }
}