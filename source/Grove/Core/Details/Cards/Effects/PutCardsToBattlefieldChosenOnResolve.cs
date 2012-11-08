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
      Game.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: Controller,
        init: p =>
          {            
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
                    isValid: card => self.Controller == card.Controller && Validator(card)),
                  canCancel: false
                  );

                self.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

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