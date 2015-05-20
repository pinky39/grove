namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class GoblinWelder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Welder")
        .ManaCost("{R}")
        .Type("Creature Goblin Artificer")
        .Text(
          "{T}: Choose target artifact a player controls and target artifact card in that player's graveyard. If both targets are still legal as this ability resolves, that player simultaneously sacrifices the artifact and returns the artifact card to the battlefield.")
        .FlavorText("I wrecked your metal guy, boss. But look I made you an ashtray.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Choose target artifact a player controls and target artifact card in that player's graveyard. If both targets are still legal as this ability resolves, that player simultaneously sacrifices the artifact and returns the artifact card to the battlefield.";

            p.Cost = new Tap();
            p.Effect = () => new ExchangeCardsInBattlefieldAndGraveyard();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield(),
              trg => trg.Message = "Choose an artifact in play.");

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Artifact).In.Graveyard(),
              trg => trg.Message = "Choose an artifact in graveyard.");

            p.TargetSelector.ValidateTargetDependencies =
              vp => vp.Effect[0].Card().Controller == vp.Effect[1].Card().Controller;

            p.TargetingRule(new EffectExchangeBattlefieldGraveyard());
            p.TimingRule(new OnMainStepsOfYourTurn());
          });
    }
  }
}