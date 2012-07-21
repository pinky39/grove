namespace Grove.Core.Details.Cards.Effects
{
  using System.Linq;
  using System.Windows;
  using Controllers;
  using Controllers.Results;
  using Ui;

  public class PayLifeOrTap : Effect
  {
    public int Life { get; set; }

    protected override void ResolveEffect()
    {
      Decisions.Enqueue<AdHocDecision<BooleanResult>>(
        controller: Controller,
        init: p =>
          {
            p.Param("card", Source.OwningCard);
            p.QueryAi = self =>
              {
                var controller = self.Controller;

                var spellsWithCost = controller.Hand
                  .Where(x => x.ManaCost != null)
                  .ToList();

                if (spellsWithCost.Count == 0)
                {
                  return false;
                }

                // one less is available because the land
                // is already counted
                var available = controller.ConvertedMana - 1;
                
                return spellsWithCost.Any(x =>
                  x.ManaCost.Converted == available + 1);
              };
            p.QueryUi = self =>
              {
                var result = self.Shell.ShowMessageBox(
                  message: string.Format("Pay {0} life?", Life),
                  buttons: MessageBoxButton.YesNo,
                  type: DialogType.Small);

                return result == MessageBoxResult.Yes;
              };

            p.Process = self =>
              {
                if (self.Result.IsTrue)
                {
                  self.Controller.Life -= Life;
                  return;
                }

                self.Param<Card>("card").Tap();
              };
          });
    }
  }
}