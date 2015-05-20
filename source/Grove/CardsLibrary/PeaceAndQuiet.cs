namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class PeaceAndQuiet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Peace and Quiet")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy two target enchantments.")
        .FlavorText("In time our realm will shine again. But it will gleam only when we scour away the taint of doubt.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Enchantment().On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.Destroy));
          });
    }
  }
}