namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class CounterTopSpell : Effect
  {
    protected override void ResolveEffect()
    {
      if (Stack.TopSpell != null)
        Stack.Counter(Stack.TopSpell);
    }
  }
}