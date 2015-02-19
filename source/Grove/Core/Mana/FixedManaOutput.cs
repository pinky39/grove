namespace Grove
{
  public class FixedManaOutput : ManaOutput
  {
    private readonly ManaAmount _amount;

    private FixedManaOutput() {}

    public FixedManaOutput(ManaAmount amount)
    {
      _amount = amount;
    }

    protected override ManaAmount GetAmountInternal()
    {
      return _amount;
    }
  }
}