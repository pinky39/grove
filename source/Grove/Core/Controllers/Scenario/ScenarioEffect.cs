namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Details.Cards.Effects;

  public class ScenarioEffect
  {
    public Func<Effect> Effect { get; set; }
  }
}