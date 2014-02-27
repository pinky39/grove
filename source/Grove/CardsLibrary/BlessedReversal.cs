namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class BlessedReversal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blessed Reversal")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("You gain 3 life for each creature attacking you.")
        .FlavorText("Even an enemy is a valuable resource.")
        .Cast(p =>
          {
            p.Effect = () => new ControllerGainsLife(
              amount: P((e, g) => e.Controller.IsActive ? 0
                : g.Combat.AttackerCount*3));

            p.TimingRule(new AfterOpponentDeclaresAttackers());
          });
    }
  }
}