namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Effects;

  public class LazyEffect : ITarget
  {
    public Func<Effect> Effect { get; set; }  
  }
}