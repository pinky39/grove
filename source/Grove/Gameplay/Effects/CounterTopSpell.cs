namespace Grove.Core.Effects
{
  public class CounterTopSpell : Effect
  {    
    protected override void ResolveEffect()
    {
      if (Stack.TopSpell != null)      
        Stack.Counter(Stack.TopSpell);
    }
  }
}