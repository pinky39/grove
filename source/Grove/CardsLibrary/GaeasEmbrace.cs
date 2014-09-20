namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

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
              () =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{G}: Regenerate enchanted creature.",
                      Cost = new PayMana(Mana.Green, ManaUsage.Abilities),
                      Effect = () => new RegenerateOwner()
                    };

                  ap.TimingRule(new RegenerateSelfTimingRule());

                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(3, 3),
              () => new AddStaticAbility(Static.Trample)
              ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}