namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

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
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}