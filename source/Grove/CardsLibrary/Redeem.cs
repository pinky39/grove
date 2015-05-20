namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;

  public class Redeem : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Redeem")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Prevent all damage that would be dealt this turn to up to two target creatures.")
        .FlavorText(
          "That they are saved from death is immaterial. What is important is that they know the source of their benefaction.")
        .Cast(p =>
          {
            p.Text = "Prevent all damage that would be dealt this turn to up to two target creatures.";            
            p.Effect = () => new PreventAllDamageToTargets();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Creature().On.Battlefield(),
              trg => {                
                trg.MinCount = 1;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectPreventNextDamageToTargets());
          });
    }
  }
}