namespace Grove.Core.Details.Mana
{
  public interface IManaSource
  {
    int Priority { get; }

    object Resource { get; }

    void Consume(IManaAmount amount, ManaUsage usage = ManaUsage.Any);
    IManaAmount GetAvailableMana(ManaUsage usage = ManaUsage.Any);
  }
}