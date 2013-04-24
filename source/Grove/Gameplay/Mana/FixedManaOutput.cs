namespace Grove.Core.Mana
{
  public class FixedManaOutput : ManaOutput
  {
    private readonly IManaAmount _amount;

    private FixedManaOutput()
    {
      
    }
    
    public FixedManaOutput(IManaAmount amount)
    {
      _amount = amount;
    }

    protected override IManaAmount GetAmountInternal()
    {
      return _amount;
    }
  }
}