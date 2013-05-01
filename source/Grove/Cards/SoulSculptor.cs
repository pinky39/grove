namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class SoulSculptor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Soul Sculptor")
        .ManaCost("{2}{W}")
        .Type("Creature Human")
        .Text(
          "{1}{W},{T}: Target creature becomes an enchantment and loses all abilities until a player casts a creature spell.")
        .FlavorText("Does the stone mimic life, or did it once live?")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{W},{T}: Target creature becomes an enchantment and loses all abilities until a player casts a creature spell.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{W}".Parse(), ManaUsage.Abilities),
              new Tap());
            
            p.Effect = () => new ApplyModifiersToTargets(
              () =>
                {
                  var modifier = new ChangeToEnchantment();
                  modifier.AddLifetime(new PlayerCastsCreatureLifetime());
                  return modifier;
                },
              () =>
                {
                  var modifier = new DisableAbilities();
                  modifier.AddLifetime(new PlayerCastsCreatureLifetime());
                  return modifier;
                });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval(combatOnly: true));
          });
    }
  }
}