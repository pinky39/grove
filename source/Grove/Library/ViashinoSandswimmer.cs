namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class ViashinoSandswimmer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Sandswimmer")
        .ManaCost("{2}{R}{R}")
        .Type("Creature Viashino")
        .Text(
          "{R}: Flip a coin. If you win the flip, return Viashino Sandswimmer to its owner's hand. If you lose the flip, sacrifice Viashino Sandswimmer.")
        .FlavorText("Few swim in a place of such thirst.")
        .Power(3)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}: Flip a coin. If you win the flip, return Viashino Sandswimmer to its owner's hand. If you lose the flip, sacrifice Viashino Sandswimmer.";
            p.Cost = new PayMana(Mana.Red, ManaUsage.Abilities);
            p.Effect = () => new FlipACoinReturnToHandOrSacrifice();
            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}