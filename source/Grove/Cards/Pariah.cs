namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Damage;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class Pariah : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              modifiesAttachmentController: true,
              modifiers: () => new AddDamageRedirection(
                new RedirectDamageToTarget(r => r.Modifier.Source.AttachedTo)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new DamageRedirectionEnchantment());
          });
    }
  }
}