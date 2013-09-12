namespace Grove.Gameplay.Costs
{
  using System;

  public class CanPayResult
  {
    private Lazy<int> _maxRepetitions = new Lazy<int>(() => 1);
    private Lazy<int?> _maxX = new Lazy<int?>(() => null);
    private Lazy<bool> _canPay;

    public void CanPay(Func<bool> getValue)
    {
      _canPay = new Lazy<bool>(getValue);
    }

    public void CanPay(bool value)
    {
      _canPay = new Lazy<bool>(() => value);
    }

    public void MaxX(Func<int?> getValue)
    {
      _maxX = new Lazy<int?>(getValue);
    }

    public void MaxRepetitions(Func<int> getValue)
    {
      _maxRepetitions = new Lazy<int>(getValue);
    }

    public Lazy<int> MaxRepetitions()
    {
      return _maxRepetitions;
    }
    
    public Lazy<int?> MaxX()
    {
      return _maxX;
    }

    public Lazy<bool> CanPay()
    {
      return _canPay;
    }
  }
}