namespace Grove.Decisions
{
  public interface IPlayable
  {
    bool WasPriorityPassed { get; }    
    void Play();
  }
}