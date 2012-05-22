namespace Grove.Core.Controllers.Results
{
  using Infrastructure;

  [Copyable]
  public class ChosenPlayable
  {
    public Playable Playable { get; set; }
    public bool WasPriorityPassed { get { return Playable.WasPriorityPassed; } }
    
    public static implicit operator ChosenPlayable(Playable playable)
    {
      return new ChosenPlayable{
        Playable = playable
      };
    }
  }
}