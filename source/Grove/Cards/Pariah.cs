namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Redirections;
  using Core.Dsl;
  using Core.Targeting;

  public class Pariah : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Pariah")
        .ManaCost("{2}{W}")
        .Type("Enchantment - Aura")
        .Timing(Timings.FirstMain())
        .Text("All damage that would be dealt to you is dealt to enchanted creature instead.")
        .FlavorText(
          "'It is not sad', Radiant chided the lesser angel. 'It is right. Every society must have its outcasts.'")
        .Effect<Attach>(e =>
          {
            e.ModifiesAttachmentController = true;
            e.Modifiers(
              Modifier<AddDamageRedirection>(m =>
                m.Redirection = Redirection<RedirectDamageToTarget>(r => { r.Target = m.Source.AttachedTo; }))
              );
          })
        .Targets(
          selectorAi: TargetSelectorAi.DamageRedirection(),
          effectValidator: Validator(Validators.EnchantedCreature()));
    }
  }
}