namespace Grove.Core.Decisions.Machine
{
  public class ChooseEffectOptions : Decisions.ChooseEffectOptions
  {
    protected override void ExecuteQuery()
    {
      Result = Ai(this);
    }
  }
}