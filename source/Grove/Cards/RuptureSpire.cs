namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class RuptureSpire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
     .Named("Rupture Spire")
     .Type("Land")
     .Text("Rupture Spire enters the battlefield tapped.{EOL}When Rupture Spire enters a battlefield, sacrifice it unless you pay {1}.{EOL}{T}: Add one mana of any color to your mana pool.")
     .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = (owner) => true)
     .Timing(All(Timings.Lands, Timings.ControllerHasMana(1.AsColorlessMana())))
     .Abilities(
       C.ManaAbility(Mana.Any, "{T}: Add one mana of any color to your mana pool."),
       C.TriggeredAbility(
         "When Rupture Spire enters the battlefield, sacrifice it unless you pay {1}.",
         C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
         C.Effect<PayManaOrSacrifice>((e, _) => e.Amount = 1.AsColorlessMana()))
     );
    }
  }    
}