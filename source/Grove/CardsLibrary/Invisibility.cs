namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Invisibility : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Invisibility")
        .ManaCost("{U}{U}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature can't be blocked except by Walls. ")
        .FlavorText(
          "Verick held his breath. Breathing wouldn't reveal his position, but it would force him to smell the goblins.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddSimpleAbility(Static.CanOnlyBeBlockedByWalls));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}