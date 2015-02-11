namespace Grove.Costs
{
  public class CanPayResult
  {
    public readonly bool CanPay;
    public readonly int? MaxX;
    public readonly int MaxRepetitions;

    public CanPayResult(bool canPay, int? maxX = null, int maxRepetitions = 1)
    {
      CanPay = canPay;
      MaxX = maxX;
      MaxRepetitions = maxRepetitions;
    }

    public static implicit operator CanPayResult(bool canPay)
    {
      return new CanPayResult(canPay);
    }
  }
}