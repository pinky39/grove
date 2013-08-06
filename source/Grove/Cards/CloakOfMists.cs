namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
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
            p.TargetingRule(new CombatEnchantment(filter: x => !x.Has().Unblockable));
          });
    }
  }
}