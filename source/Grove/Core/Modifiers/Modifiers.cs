namespace Grove.Core.Modifiers
{
  using System.Collections.Generic;
  using System.Linq;

  public static class Modifiers
  {
    public static List<Modifier> CreateModifiers(this IEnumerable<IModifierFactory> modifierFactories, Card source, Card target, int? x)
    {
      return modifierFactories.Select(factory => factory.CreateModifier(source, target, x)).ToList();
    }
  }
}