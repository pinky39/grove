namespace Grove.Core.Details.Cards.Effects
{
  using System.Linq;
  using System.Windows;
  using Controllers;
  using Controllers.Results;
  using Ui;

  public class PutOnTopOfLibraryUnlessOpponentSacLand : Effect
  {
    protected override void ResolveEffect()
    {
      Decisions.Enqueue<AdHocDecision<BooleanResult>>(
        controller: Players.GetOpponent(Controller),
        init: p =>
          {
            p.Param("card", Source.OwningCard);
            p.QueryAi = self => { return self.Controller.Battlefield.Lands.Count() >= 4; };
            p.QueryUi = shell =>
              {
                var result = shell.ShowMessageBox(
                  message: "Sacrifice a land?",
                  buttons: MessageBoxButton.YesNo,
                  type: DialogType.Small);

                return result == MessageBoxResult.Yes;
              };

            p.Process = self =>
              {
                if (self.Result.IsTrue)
                {
                  self.Controller.MoveCardFromBattlefieldOnTopOfLibrary(
                    self.Param<Card>("card"));
                }
              };
          });
    }
  }
}