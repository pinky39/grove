namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Mana;
  using Core.Dsl;

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
        .Timing(Timings.Creatures())
        .Abilities(
          ManaAbility(
            manaAmount: "{B}{B}".ParseManaAmount(),
            text: "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.",
            cost: Cost<SacrificeOwner>(),
            priority: ManaSourcePriorities.OnlyIfNecessary)
        );
    }
  }
}