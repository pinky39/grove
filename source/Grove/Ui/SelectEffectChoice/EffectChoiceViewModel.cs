namespace Grove.Ui.SelectEffectChoice
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Effects;

  public class EffectChoiceViewModel
  {
    public EffectChoiceViewModel(EffectChoice effectChoice)
    {
      Options = effectChoice.Options.ToList();
      Selected = Options[0];
    }

    public List<EffectChoiceOption> Options { get; private set; }
    public EffectChoiceOption Selected { get; set; }
  }
}