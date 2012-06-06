namespace Grove.Core
{
  public static class ManaSourcePriorities
  {
    public static readonly int Land = 1000;
    public static readonly int Creature = 2000;
    public static readonly int Restricted = 3000;
  }
  
  public interface IManaSource
  {
    int Priority { get; }

    object Resource { get; }

    void Consume(IManaAmount amount);        
    IManaAmount GetAvailableMana();
  }
}