namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

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
              .SetTags(EffectTag.ChangeController);

            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectGainControl());
          });
    }
  }
}