namespace Grove.Core.Details.Cards.Modifiers
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public static class Modifiers
  {
    public static List<Modifier> CreateModifiers(this IEnumerable<IModifierFactory> modifierFactories, Card source,
                                                 ITarget target, object origin, int? x)
    {
      return modifierFactories.Select(factory => factory.CreateModifier(source, target, origin, x)).ToList();
    }
  }
}