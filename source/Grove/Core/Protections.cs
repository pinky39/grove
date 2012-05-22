namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Protections : IModifiable, IHashable
  {
    private readonly TrackableList<ManaColors> _colors;

    private Protections() {}

    public Protections(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _colors = new TrackableList<ManaColors>(changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator hashCalculator)
    {
      return hashCalculator.Calculate(_colors);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AddProtectionFromColors(ManaColors color)
    {
      _colors.Add(color);
    }

    public bool HasProtectionFrom(ManaColors colors)
    {
      var mergedProtection = _colors.Aggregate(ManaColors.None,
        (current, protectionFromColors) => current | protectionFromColors);

      return (colors & mergedProtection) != ManaColors.None;
    }

    public void RemoveProtectionFromColors(ManaColors colors)
    {
      _colors.Remove(colors);
    }
  }
}