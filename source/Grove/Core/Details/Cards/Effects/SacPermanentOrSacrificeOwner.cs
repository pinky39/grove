namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Linq;
  using System.Windows;
  using Controllers.Human;
  using Controllers.Results;
  using Targeting;
  using Ui;

  public class SacPermanentOrSacrificeOwner : Effect
  {
    public string Message = "Select permanent to sacrifice";
    public Func<Card, bool> PayUpkeepAi = delegate { return true; };
    public Func<Card, bool> Selector = delegate { return true; };

    protected override void ResolveEffect()
    {
      if (!Controller.Battlefield.Any(Selector))
      {
        Source.OwningCard.Sacrifice();
        return;
      }

      Decisions.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: Controller,
        init: p =>
          {
            p.Param("card", Source.OwningCard);

            p.QueryAi = self =>
              {
                var owningCard = self.Param<Card>("card");

                if (!PayUpkeepAi(owningCard))
                {
                  return new ChosenCards();
                }

                return new ChosenCards(
                  self.Controller.Battlefield
                    .Where(Selector)
                    .OrderBy(x => x.Score)
                    .Take(1));
              };

            p.QueryUi = self =>
              {
                var result = self.Shell.ShowMessageBox(
                  message: "Pay upkeep?",
                  buttons: MessageBoxButton.YesNo,
                  type: DialogType.Small);

                if (result != MessageBoxResult.Yes)
                  return new ChosenCards();


                var dialog = self.TargetDialog.Create(
                  new UiTargetValidator(
                    minTargetCount: 1,
                    maxTargetCount: 1,
                    text: Message,
                    isValid: target => target.IsPermanent && Selector(target) && target.Controller == Controller),
                  canCancel: false
                  );

                self.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

                return new ChosenCards(dialog.Selection.Select(x => x.Card()));
              };

            p.Process = self =>
              {
                if (self.Result.Any())
                {
                  foreach (var card in self.Result)
                  {
                    card.Sacrifice();
                  }
                  return;
                }

                self.Param<Card>("card").Sacrifice();
              };
          });
    }
  }
}