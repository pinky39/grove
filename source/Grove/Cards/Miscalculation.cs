namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;  
  
  public class Miscalculation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Miscalculation")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Counter target spell unless its controller pays {2}.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell(doNotCounterCost: 2);
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            p.TimingRule(new Artifical.TimingRules.Counterspell(2));
            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
          });
    }
  }
}