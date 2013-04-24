namespace Grove.Core.Decisions.Scenario
{
  using System;
  using Gameplay.Card.Characteristics;
  using Gameplay.Common;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Targeting;
  using Grove.Infrastructure;

  public class ScenarioEffect : ITarget, IHasColors
  {
    public Func<Effect> Effect { get; set; }

    public int CalculateHash(HashCalculator calc)
    {
      return Effect().CalculateHash(calc);
    }

    public void AddModifier(IModifier modifier) {}

    public void RemoveModifier(IModifier modifier) {}
    
    public bool HasColor(CardColor color)
    {
      return Effect().HasColor(color);
    }
  }
}