namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ArchfiendOfDepravity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Archfiend of Depravity")
        .ManaCost("{3}{B}{B}")
        .Type("Creature - Demon")
        .Text("{Flying}{EOL}At the beginning of each opponent's end step, that player chooses up to two creatures he or she controls, then sacrifices the rest.")
        .FlavorText("\"Why would I kill you all? Who then would be left to worship me?\"")
        .Power(5)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "At the beginning of each opponent's end step, that player chooses up to two creatures he or she controls, then sacrifices the rest.";
          p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true));
          p.Effect = () => new PlayerSelectPermanentsAndSacrificeRest(
            toUpCount: 2,
            player: P((e, g) => g.Players.Active),
            filter: c => c.Is().Creature,
            text: "Choose up to creatures you control, then sacrifice the rest.");
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
