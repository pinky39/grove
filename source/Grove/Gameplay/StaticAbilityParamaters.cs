namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Grove.Gameplay.Modifiers;

  public class StaticAbilityParamaters
  {
    public bool EnabledInAllZones;
    public List<ModifierFactory> Modifiers = new List<ModifierFactory>();

    public void Modifier(ModifierFactory modifier)
    {
      Modifiers.Add(modifier);
    }
  }
}