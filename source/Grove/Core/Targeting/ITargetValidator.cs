namespace Grove.Core.Targeting
{
  public interface ITargetValidator
  {
    int? MaxCount { get; }
    int MinCount { get; }
    string Text { get; }
    bool IsValid(ITarget target);
  }
}