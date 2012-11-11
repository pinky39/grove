namespace Grove.Core.Controllers.Machine
{
  using System;

  public class ChooseEffectOptions : Controllers.ChooseEffectOptions
  {
    protected override void ExecuteQuery()
    {
      Result = Ai(this);
    }
  }
}