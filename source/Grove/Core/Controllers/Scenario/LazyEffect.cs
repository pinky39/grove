namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Effects;
  using Infrastructure;
  using Modifiers;

  public class LazyEffect : ITarget
  {
    public Func<Effect> Effect { get; set; }

    public int CalculateHash(HashCalculator calc)
    {
      return 0;
    }

    public void AddModifier(IModifier modifier) {}

    public void RemoveModifier(IModifier modifier) {}
  }
}