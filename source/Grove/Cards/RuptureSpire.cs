namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Zones;

  public class RuptureSpire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Rupture Spire")
        .Type("Land")
        .Text(
          "Rupture Spire enters the battlefield tapped.{EOL}When Rupture Spire enters a battlefield, sacrifice it unless you pay {1}.{EOL}{T}: Add one mana of any color to your mana pool.")
        .Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true)
        .Timing(All(Timings.Lands(), Timings.HasConvertedMana(1)))
        .Abilities(
          ManaAbility(ManaUnit.Any, "{T}: Add one mana of any color to your mana pool."),
          TriggeredAbility(
            "When Rupture Spire enters the battlefield, sacrifice it unless you pay {1}.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<PayManaOrSacrifice>(e => e.Amount = 1.AsColorlessMana()))
        );
    }
  }
}