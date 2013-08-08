namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class RumblingSlum : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rumbling Slum")
        .ManaCost("{1}{R}{G}{G}")
        .Type("Creature - Elemental")
        .Text("At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.")
        .FlavorText(
          "The Orzhov contract the Izzet to animate slum districts and banish them to the wastes. The Gruul adopt them and send them back to the city for vengeance.")
        .Power(5)
        .Toughness(5)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.";
            p.Trigger(new OnStepStart(step: Step.Untap));
            p.Effect = () => new DealDamageToCreaturesAndPlayers(amountPlayer: 1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}