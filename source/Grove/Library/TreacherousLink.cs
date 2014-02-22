namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class TreacherousLink : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treacherous Link")
        .ManaCost("{1}{B}")
        .Type("Enchantment - Aura")
        .Text("All damage that would be dealt to enchanted creature is dealt to its controller instead.")
        .FlavorText("You cannot possibly know the toll your alliances will exact from you.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              modifiers: () => new AddDamageRedirection(modifier => new RedirectDamageFromTargetToTarget(
                @from: modifier.SourceCard.AttachedTo,
                to: modifier.SourceCard.AttachedTo.Controller)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectRedirectDamageToControllerEnchantment());
          });
    }
  }
}