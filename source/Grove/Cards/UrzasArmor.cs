namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Dsl;

  public class UrzasArmor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Urza's Armor")
        .ManaCost("{6}")
        .Type("Artifact")
        .Text("If a source would deal damage to you, prevent 1 of that damage.")
        .FlavorText(
          "'Tawnos's blueprints were critical to the creation of my armor. As he once sealed himself in steel, I sealed myself in a walking crypt.'{EOL}—Urza")
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddDamagePrevention>(
                m => m.Prevention = Prevention<PreventDamageToTarget>(p => p.Amount = 1));
              e.CardFilter = delegate { return false; };
              e.PlayerFilter = (player, armor) => player == armor.Controller;
            })
        );
    }
  }
}