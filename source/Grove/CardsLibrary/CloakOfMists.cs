namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class CloakOfMists : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Unblockable));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment(filter: x => !x.Has().Unblockable));
          });
    }
  }
}