namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class Confiscate : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Confiscate")
        .ManaCost("{4}{U}{U}")
        .Type("Enchantment Aura")
        .Text("You control enchanted permanent.")
        .FlavorText(
          "'I don't understand why he works so hard on a device to duplicate a sound so easily made with hand and armpit.'{EOL}—Barrin, progress report")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<ChangeController>(m => m.NewController = m.Source.Controller)));
            p.EffectTargets = L(Target(Validators.Card(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.GainControl();
          });
    }
  }
}