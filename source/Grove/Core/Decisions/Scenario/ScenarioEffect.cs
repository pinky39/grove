namespace Grove.Core.Decisions.Scenario
{
  using System;
  using Cards.Effects;
  using Cards.Modifiers;
  using Grove.Infrastructure;
  using Grove.Core.Targeting;
  using Mana;

  public class ScenarioEffect : ITarget, IHasColors
  {
    public Func<Effect> Effect { get; set; }

    public int CalculateHash(HashCalculator calc)
    {
      return Effect().CalculateHash(calc);
    }

    public void AddModifier(IModifier modifier) {}

    public void RemoveModifier(IModifier modifier) {}
    
    public bool HasColors(ManaColors colors)
    {
      return Effect().HasColors(colors);
    }
  }
}