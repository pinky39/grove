namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Zones;

  public class CopperGnomes : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Copper Gnomes")
        .ManaCost("{2}")
        .Type("Artifact Creature Gnome")
        .Text("{4}, Sacrifice Copper Gnomes: You may put an artifact card from your hand onto the battlefield.")
        .FlavorText(
          "Start with eleven gnomes and a room of parts, and come morning you'll have ten and a monster the likes of which you've never seen.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "{4}, Sacrifice Copper Gnomes: You may put an artifact card from your hand onto the battlefield.",
            Cost<SacOwnerPayMana>(cost => cost.Amount = 4.AsColorlessMana()),
            Effect<PutTargetCardToBattlefield>(e =>
              {
                e.Validator = card => card.Zone == Zone.Hand && card.Is().Artifact;
                e.Text = "Select an artifact in your hand";
              }),
            timing: All(
              Any(Timings.BeforeDeath(), Timings.EndOfTurn()),
              Timings.HasCardInHand(card => card.Is().Artifact)))
        );
    }
  }
}