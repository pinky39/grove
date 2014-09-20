namespace Grove.UserInterface.SelectEffectChoice
{
  using Effects;

  public class EffectChoiceViewModel
  {
    public EffectChoiceViewModel(IEffectChoice choice)
    {      
      Choice = choice;
      Selected = choice.Options[0];      
    }

    public IEffectChoice Choice { get; set; }
    public object Selected { get; set; }
  }
}