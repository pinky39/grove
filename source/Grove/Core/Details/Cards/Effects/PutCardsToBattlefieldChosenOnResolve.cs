namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Linq;
  using Controllers.Human;
  using Controllers.Results;
  using Infrastructure;
  using Targeting;
  using Ui;

  public class PutCardToBattlefieldChosenOnResolve : Effect
  {
    public string Text;
    public Func<Card, bool> Validator;

    protected override void ResolveEffect()
    {
      Decisions.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: Controller,
        init: p =>
          {
            p.Param("self", Validator);
            p.QueryAi = self =>
              {
                var chosenCards = new ChosenCards();

                var target = self.Game.GenerateTargets()
                  .Where(x => x.IsCard())
                  .Select(x => x.Card())
                  .Where(x => x.Controller == self.Controller)
                  .Where(Validator)
                  .OrderByDescending(x => x.Score)
                  .FirstOrDefault();

                if (target != null)
                  chosenCards.Add(target);

                return chosenCards;
              };
            p.QueryUi = self =>
              {
                var chosenCards = new ChosenCards();

                var dialog = self.TargetDialog.Create(
                  new UiTargetValidator(
                    minTargetCount: 0,
                    maxTargetCount: 1,
                    text: FormatDialogMessage(Text),
                    isValid: Validator),
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

                var chosen = self.Result.First();

                chosen.PutToBattlefield();
              };
          });
    }
  }
}