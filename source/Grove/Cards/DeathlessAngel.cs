namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class DeathlessAngel : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Deathless Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}{W}{W}: Target creature is indestructible this turn.")
        .FlavorText(
          "I should have died that day, but I suffered not a scratch. I awoke in a lake of blood, none of it apparently my own.")
        .Power(5)
        .Toughness(7)
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{W}{W}: Target creature is indestructible this turn.";
            p.Cost = new PayMana("{W}{W}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Indestructible)) {Category = EffectCategories.Protector};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new GainIndestructible());
          });
    }
  }
}