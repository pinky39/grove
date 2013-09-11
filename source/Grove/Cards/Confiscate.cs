namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Confiscate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Confiscate")
        .ManaCost("{4}{U}{U}")
        .Type("Enchantment Aura")
        .Text("You control enchanted permanent.")
        .FlavorText(
          "I don't understand why he works so hard on a device to duplicate a sound so easily made with hand and armpit.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new ChangeController(m => m.SourceCard.Controller))
              .Tags(EffectTag.ChangeController);

            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectGainControl());
          });
    }
  }
}