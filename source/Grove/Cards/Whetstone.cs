namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;

  public class Whetstone : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Whetstone")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3}: Each player puts the top two cards of his or her library into his or her graveyard.")
        .FlavorText("To hone swords and dull minds.")
        .ActivatedAbility(p =>
          {
            p.Text = "{3}: Each player puts the top two cards of his or her library into his or her graveyard.";
            p.Cost = new PayMana(3.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new EachPlayerPutTopCardsFromLibraryToGraveyard(2);
            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
          });
    }
  }
}