namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class CloakOfMists : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Cloak of Mists")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature is unblockable.")
        .FlavorText(
          "All we could lose, we did. All we could keep, we do. And both are shrouded by mists.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddStaticAbility(Static.Unblockable));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}