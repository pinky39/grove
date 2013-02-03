namespace Grove.Core
{
  using System.Collections.Generic;
  using Triggers;

  public class TriggeredAbilityParameters : AbilityParameters
  {
    private List<Trigger> _triggers = new List<Trigger>();

    public bool TriggerOnlyIfOwningCardIsInPlay;
    public IEnumerable<Trigger> Triggers { get { return _triggers; }}
    
    public void Trigger(Trigger trigger)
    {
      _triggers.Add(trigger);
    }
  }
}