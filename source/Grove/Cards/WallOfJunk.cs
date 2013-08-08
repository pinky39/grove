namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class WallOfJunk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Junk")
        .Type("Artifact Creature Wall")
        .ManaCost("{2}")
        .Text(
          "{Defender}{EOL}Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)")
        .FlavorText(
          "Urza saw the wall and realized that even if he tore every Phyrexian to pieces, they would still resist him.")
        .Power(0)
        .Toughness(7)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)";
            p.Trigger(new OnStepStart(
              step: Step.EndOfCombat,
              passiveTurn: true,
              activeTurn: false) {Condition = (t, g) => t.OwningCard.IsBlocker});

            p.Effect = () => new ReturnToHand(returnOwningCard: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}