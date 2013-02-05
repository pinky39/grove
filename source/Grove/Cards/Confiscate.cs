namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Confiscate : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new Attach(() => new ChangeController(m => m.Source.Controller))
              {Category = EffectCategories.Destruction};

            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new GainControl());
          });
    }
  }
}