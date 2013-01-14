namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Redirections;
  using Core.Targeting;

  public class Pariah : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
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
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e =>
              {
                e.ModifiesAttachmentController = true;
                e.Modifiers(
                  Modifier<AddDamageRedirection>(m =>
                    m.Redirection = Redirection<RedirectDamageToTarget>(r => { r.Target = m.Source.AttachedTo; }))
                  );
              });
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DamageRedirection();
          });
    }
  }
}