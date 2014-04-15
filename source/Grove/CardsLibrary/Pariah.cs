namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Pariah : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pariah")
        .ManaCost("{2}{W}")
        .Type("Enchantment - Aura")
        .Text("All damage that would be dealt to you is dealt to enchanted creature instead.")
        .FlavorText(
          "'It is not sad', Radiant chided the lesser angel. 'It is right. Every society must have its outcasts.'")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              modifiers: () => new AddDamageRedirection(modifier => new RedirectDamageFromTargetToTarget(
                @from: modifier.SourceCard.Controller,
                to: modifier.SourceCard.AttachedTo)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectRedirectDamageToCreatureEnchantment());
          });
    }
  }
}