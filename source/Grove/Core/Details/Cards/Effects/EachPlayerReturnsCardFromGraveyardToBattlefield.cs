namespace Grove.Core.Details.Cards.Effects
{
  using System.Linq;
  using Controllers.Human;
  using Controllers.Results;
  using Infrastructure;
  using Targeting;
  using Ui;
  using Zones;

  public class EachPlayerReturnsCardFromGraveyardToBattlefield : Effect
  {
    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Decisions.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: player,
        init: p =>
          {
            p.Param("card", Source.OwningCard);
            p.QueryAi = self =>
              {
                var chosenCards = new ChosenCards();
                
                var bestCreature = self.Controller.Graveyard.Creatures
                  .OrderByDescending(x => x.Score)
                  .FirstOrDefault();

                if (bestCreature != null)
                  chosenCards.Add(bestCreature);

                return chosenCards;
              };
            p.QueryUi = self =>
              {                                                  
                var chosenCards = new ChosenCards();

                if (self.Controller.Graveyard.Creatures.Count() == 0)
                  return chosenCards;

                var dialog = self.TargetDialog.Create(
                  new UiTargetValidator(
                    minTargetCount: 1,
                    maxTargetCount: 1,
                    text: "Select a creature in your graveyard",
                    isValid: target =>
                      target.Zone == Zone.Graveyard &&
                        target.Is().Creature &&
                          target.Controller == self.Controller),
                  canCancel: false
                  );

                self.Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);
                
                if (dialog.Selection.Count > 0)
                  chosenCards.Add(dialog.Selection[0].Card());

                return chosenCards;
              };
            p.Process = self =>
              {
                if (self.Result.None())
                  return;

                var creature = self.Result.First();
                creature.PutToBattlefield();
              };
          });
    }

    protected override void ResolveEffect()
    {
      ChooseCreatureToPutIntoPlay(Players.Active);
      ChooseCreatureToPutIntoPlay(Players.Passive);
    }
  }
}