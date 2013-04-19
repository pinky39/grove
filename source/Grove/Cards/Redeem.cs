namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Preventions;

  public class Redeem : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddDamagePrevention(new PreventDamage()) {UntilEot = true});

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().On.Battlefield();
                trg.MinCount = 1;
                trg.MaxCount = 2;
              });
            
            p.TargetingRule(new PreventDamageToTargets());
          });
    }
  }
}