namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using Controllers;
  using Controllers.Results;
  using Ui;

  public class CustomizableEffect : Effect
  {
    public Func<CustomizableEffect, Game, ChosenOptions> ChooseAi;
    public Action<CustomizableEffect, ChosenOptions> Logic;
    public string Text;
    public EffectChoice[] Choices { get; set; }

    public override bool NeedsTargets { get { return true; } }    

    protected override void ResolveEffect()
    {
      Decisions.Enqueue<AdhocDecision<ChosenOptions>>(
        controller: Controller,
        init: p =>
          {
            p.Param("this", this);
            p.QueryAi = self => { return ChooseAi(self.Param<CustomizableEffect>("this"), self.Game); };
            p.QueryUi = self =>
              {
                var dialog = self.EffectChoiceDialog.Create(Choices, Text);
                self.Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

                return dialog.ChosenOptions;
              };

            p.Process = self => { Logic(self.Param<CustomizableEffect>("this"), self.Result); };
          }
        );
    }
  }
}