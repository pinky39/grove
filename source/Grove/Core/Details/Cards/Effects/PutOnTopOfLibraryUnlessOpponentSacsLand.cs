namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Linq;
  using System.Windows;
  using Controllers.Human;
  using Controllers.Results;
  using Infrastructure;
  using Targeting;
  using Ui;

  public class PutOnTopOfLibraryUnlessOpponentSacsLand : Effect
  {
    protected override void ResolveEffect()
    {
      Decisions.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: Players.GetOpponent(Controller),
        init: p =>
          {
            p.Param("card", Source.OwningCard);
            p.QueryAi = self =>
              {
                var chosenCards = new ChosenCards();

                if (self.Controller.Battlefield.Lands.Count() < 4)
                {
                  return chosenCards;
                }

                var pickedLand = self.Controller.Battlefield.Lands
                  .OrderBy(x => x.Score).First();

                chosenCards.Add(pickedLand);
                return chosenCards;
              };
            p.QueryUi = self =>
              {
                var chosenCards = new ChosenCards();

                var result = self.Shell.ShowMessageBox(
                  message: FormatDialogMessage("Sacrifice a land?"),
                  buttons: MessageBoxButton.YesNo,
                  type: DialogType.Small);

                if (result == MessageBoxResult.No)
                  return chosenCards;

                var dialog = self.TargetDialog.Create(
                  new UiTargetValidator(
                    minTargetCount: 1,
                    maxTargetCount: 1,
                    text: FormatDialogMessage("Select a land to sacrifice"),
                    isValid: target => target.IsPermanent && target.Is().Land && target.Controller == self.Controller),
                  canCancel: false
                  );

                self.Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);
                chosenCards.Add(dialog.Selection[0].Card());

                return chosenCards;
              };
            p.Process = self =>
              {
                if (self.Result.None())
                  return;
                
                var wurm = self.Param<Card>("card");
                var land = self.Result.First();

                wurm.PutOnTopOfLibrary();
                land.Sacrifice();                                
              };
          })
        ;
    }
  }
}