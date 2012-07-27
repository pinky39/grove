namespace Grove.Core.Targeting
{
  public interface ITargetValidator
  {
    int? MaxCount { get; }
    int MinCount { get; }    
    bool IsValid(ITarget target);

    string GetMessage(int targetNumber);
  }
}