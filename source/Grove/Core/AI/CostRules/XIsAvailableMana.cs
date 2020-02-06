namespace Grove.AI.CostRules
{
  public class XIsAvailableMana : CostRule
  {
    private readonly int _modifier;

    public XIsAvailableMana()
    {
    }

    public XIsAvailableMana(int modifier)
    {
      _modifier = modifier;
    }
    
    public override int CalculateX(CostRuleParameters p)
    {                  
      var x = p.MaxX + _modifier;
      return x > 0 ? x : 0;
    }
  }
}