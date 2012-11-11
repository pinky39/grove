namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Details.Cards.Effects;
  using Details.Cards.Modifiers;
  using Details.Mana;
  using Infrastructure;
  using Targeting;

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