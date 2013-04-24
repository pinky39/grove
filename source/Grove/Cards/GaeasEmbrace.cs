namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

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
                    Cost = new PayMana(Mana.Green, ManaUsage.Abilities),
                    Effect = () => new Gameplay.Effects.Regenerate()
                  };

                ap.TimingRule(new Ai.TimingRules.Regenerate());

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