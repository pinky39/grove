namespace Grove.Artifical.Decisions
{
  using Gameplay.Decisions.Results;
  using Gameplay.Messages;

  public class ChooseEffectOptions : Gameplay.Decisions.ChooseEffectOptions
  {
    public ChooseEffectOptions()
    {
      Result = new ChosenOptions();
    }
    
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Choices);

      Publish(new EffectOptionsWereChosen {Text = 
        string.Format("{0} chose {1}.", Controller, string.Join(", ", Result.Options))});
    }
  }
}