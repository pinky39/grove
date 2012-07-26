namespace Grove.Core.Targeting
{
  public class DistributeDamage : UiTargetPostprocessor
  {
    private readonly int _amount;

    public DistributeDamage(int amount)
    {
      _amount = amount;
    }

    public override void Postprocess(Targets targets)
    {
      // 
    }
  
    public interface IFactory
    {
      DistributeDamage Create(int amount);
    }
  }
}