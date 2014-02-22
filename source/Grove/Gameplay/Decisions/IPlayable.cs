namespace Grove.Gameplay.Decisions
{
  public interface IPlayable
  {
    bool WasPriorityPassed { get; }    
    void Play();
  }
}