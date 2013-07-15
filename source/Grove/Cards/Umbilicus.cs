namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class Umbilicus : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Umbilicus")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("At the beginning of each player's upkeep, that player returns a permanent he or she controls to its owner's hand unless he or she pays 2 life.")
        .FlavorText("It was the explorers' only tether to reality.")
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of each player's upkeep, that player returns a permanent he or she controls to its owner's hand unless he or she pays 2 life.";

            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true));
            
            p.Effect = () => new ActivePlayerPaysLifeOrReturnSelectedPermanentToHand(
              life: 2);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}