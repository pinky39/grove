namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using Controllers;
  using Controllers.Results;
  using Ui;

  public class CustomizableEffect : Effect
  {
    private readonly List<EffectChoice> _choices = new List<EffectChoice>();
    public Func<CustomizableEffect, Game, ChosenOptions> ChooseAi;
    public Action<CustomizableEffect, ChosenOptions> Logic;
    public string Text;        

    public void Choices(params EffectChoice[] choices)
    {
      _choices.AddRange(choices);
    }

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
                var dialog = self.EffectChoiceDialog.Create(_choices, Text);
                self.Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

                return dialog.ChosenOptions;
              };

            p.Process = self => { Logic(self.Param<CustomizableEffect>("this"), self.Result); };
          }
        );
    }
  }
}