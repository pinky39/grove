namespace Grove.Core.Targeting
{
  public interface ITargetSelector
  {
    int? MaxCount { get; }
    int MinCount { get; }
    string Text { get; }
    bool IsValid(Target target);
  }
}