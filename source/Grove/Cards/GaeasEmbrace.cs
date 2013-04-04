namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class GaeasEmbrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "{G}: Regenerate enchanted creature.",
                    Cost = new PayMana(ManaAmount.Green, ManaUsage.Abilities),
                    Effect = () => new Core.Effects.Regenerate()
                  };

                ap.TimingRule(new Core.Ai.TimingRules.Regenerate());

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