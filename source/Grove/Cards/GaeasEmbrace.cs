namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class GaeasEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gaea's Embrace")
        .ManaCost("{2}{G}{G}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature gets +3/+3 and has trample.{EOL}{G}: Regenerate enchanted creature.")
        .FlavorText("The forest rose to the battle, not to save the people but to save itself.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "{G}: Regenerate enchanted creature.",
                    Cost = new PayMana(Mana.Green, ManaUsage.Abilities),
                    Effect = () => new Gameplay.Effects.Regenerate()
                  };

                ap.TimingRule(new Artifical.TimingRules.Regenerate());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              },
              () => new AddPowerAndToughness(3, 3),
              () => new AddStaticAbility(Static.Trample)
              ) {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}