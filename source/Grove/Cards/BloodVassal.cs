namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Dsl;
  using Core.Mana;

  public class BloodVassal : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Blood Vassal")
        .ManaCost("{2}{B}")
        .Type("Creature - Thrull")
        .Text("Sacrifice Blood Vassal: Add {B}{B} to your mana pool.")
        .FlavorText("'They are bred to suffer and born to die. Much like humans.'{EOL}—Gix, Yawgmoth praetor")
        .Power(2)
        .Toughness(2)        
        .Abilities(
          ManaAbility(
            manaAmount: "{B}{B}".ParseMana(),
            text: "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.",
            cost: Cost<Sacrifice>(),
            priority: ManaSourcePriorities.OnlyIfNecessary)
        );
    }
  }
}