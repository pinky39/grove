namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class Exhaustion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Exhaustion")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Creatures and lands target opponent controls don't untap during his or her next untap step.")
        .FlavorText(
          "The mage felt as though he'd been in the stasis suit for days. Upon his return, he found it was months.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPlayer(
              selector: e => e.Controller.Opponent,
              modifiers: () =>
                {
                  var cp = new ContinuousEffectParameters{
                    Selector = (card, ctx) =>
                      card.Controller == ctx.Opponent &&
                        (card.Is().Creature || card.Is().Land),
                    Modifier = () => new AddSimpleAbility(Static.DoesNotUntap)};                    

                  var modifier = new AddContiniousEffect(new ContinuousEffect(cp));

                  modifier.AddLifetime(new EndOfStep(
                    Step.Untap, 
                    l => l.Modifier.SourceCard.Controller.IsActive));
                  
                  return modifier;
                });

            p.TimingRule(new OnSecondMain());
          });
    }
  }
}