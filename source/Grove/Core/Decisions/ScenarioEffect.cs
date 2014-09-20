namespace Grove.Decisions
{
  using System;
  using Grove.Infrastructure;
  using Modifiers;

  public class ScenarioEffect : ITarget, IHasColors
  {
    public Func<Effect> Effect { get; set; }

    public bool HasColor(CardColor color)
    {
      return Effect().HasColor(color);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return Effect().CalculateHash(calc);
    }

    public void AddModifier(IModifier modifier) {}

    public void RemoveModifier(IModifier modifier) {}
    
    public int Id { get { return 0; } }
  }
}