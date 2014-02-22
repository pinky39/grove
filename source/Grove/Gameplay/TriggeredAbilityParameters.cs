namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Grove.Gameplay.Triggers;

  public class TriggeredAbilityParameters : AbilityParameters
  {
    private readonly List<Trigger> _triggers = new List<Trigger>();

    public bool TriggerOnlyIfOwningCardIsInPlay;
    public List<Trigger> Triggers { get { return _triggers; } }

    public void Trigger(Trigger trigger)
    {
      _triggers.Add(trigger);
    }
  }
}