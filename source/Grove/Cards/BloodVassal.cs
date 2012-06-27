namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Costs;

  public class BloodVassal : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Blood Vassal")
        .ManaCost("{2}{B}")
        .Type("Creature - Thrull")
        .Text("Sacrifice Blood Vassal: Add {B}{B} to your mana pool.")
        .FlavorText("'They are bred to suffer and born to die. Much like humans.'{EOL}—Gix, Yawgmoth praetor")
        .Power(2)
        .Toughness(2)
        .Abilities(
          C.ManaAbility(
            manaAmount: "{B}{B}".ParseManaAmount(), 
            text: "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.",
            cost: C.Cost<SacrificeOwner>(),
            priority: ManaSourcePriorities.OnlyIfNecessary)
        );
    }
  }
}