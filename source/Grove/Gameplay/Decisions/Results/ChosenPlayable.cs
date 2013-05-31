namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using Infrastructure;

  [Copyable, Serializable]
  public class ChosenPlayable
  {
    public IPlayable Playable { get; set; }
    public bool WasPriorityPassed { get { return Playable.WasPriorityPassed; } }    
  }
}