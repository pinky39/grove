namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Encrust : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Encrust")
        .ManaCost("{1}{U}{U}")
        .Type("Enchantment — Aura")
        .Text(
          "Enchant artifact or creature{EOL}Enchanted permanent doesn't untap during its controller's untap step and its activated abilities can't be activated.")
        .FlavorText("\"The sea blesses the tiny with the power to fell the mighty.\"{EOL}—Talrand, sky summoner")
        .Cast(p =>
          {
            p.Effect = () => new Attach(new ModifierFactory[]
              {
                () => new AddStaticAbility(Static.DoesNotUntap),
                () => new DisableAbilities(activated: true),
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Creature || c.Is().Artifact).On.Battlefield());
            
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack());            
          });
    }
  }
}