namespace Grove
{
  using System.Collections.Generic;

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