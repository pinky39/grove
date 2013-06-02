namespace Grove.Gameplay.Decisions.Results
{
  public interface IPlayable
  {
    bool WasPriorityPassed { get; }
    bool CanPlay();
    void Play();
  }
}