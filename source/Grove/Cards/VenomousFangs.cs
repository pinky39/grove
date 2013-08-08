namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class VenomousFangs : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      // original ability is slightly different, but AI understands 
      // deathtouch and the additional code is currently just not worth the effort.
      
      yield return Card
        .Named("Venomous Fangs")
        .ManaCost("{2}{G}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature has Deathtouch.")
        .FlavorText("All the pain of the shattered forest contained in a single drop.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.Deathtouch));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}