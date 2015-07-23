namespace Grove.UserInterface
{
  using System.Linq;  

  public static class UiHelpers
  {
    public static bool SelectX(ActivationPrerequisites prerequisites, ActivationParameters activation, bool canCancel)
    {
      if (prerequisites.HasXInCost)
      {
        var dialog = Ui.Dialogs.SelectXCost.Create(prerequisites.MaxX.Value, canCancel);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        activation.X = dialog.ChosenX;
      }

      return true;
    }

    public static bool SelectTargets(ActivationPrerequisites prerequisites, ActivationParameters parameters,
      bool canCancel)
    {
      if (parameters.PayManaCost && prerequisites.Selector.RequiresCostTargets)
      {
        var dialog = ShowSelectTargetDialog(
          prerequisites.Selector.Cost.FirstOrDefault(),
          parameters.X, canCancel);

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          parameters.Targets.AddCost(target);
        }
      }

      if (prerequisites.Selector.RequiresEffectTargets)
      {
        foreach (var selector in prerequisites.Selector.Effect)
        {
          var dialog = ShowSelectTargetDialog(selector, parameters.X, canCancel);

          if (dialog.WasCanceled)
            return false;

          foreach (var target in dialog.Selection)
          {
            parameters.Targets.AddEffect(target);
          }
        }
      }

      return prerequisites.Selector.ValidateTargetDependencies(
        new ValidateTargetDependenciesParam
        {
          Cost = parameters.Targets.Cost,
          Effect = parameters.Targets.Effect
        }
        );
    }

    private static SelectTarget.ViewModel ShowSelectTargetDialog(TargetValidator validator, int? x, bool canCancel)
    {
      var selectTargetParameters = new SelectTarget.SelectTargetParameters
      {
        Validator = validator,
        CanCancel = canCancel,
        X = x
      };

      var dialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
      Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }
  }
}