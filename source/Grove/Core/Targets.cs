namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;

  public class Targets : TargetBag<ITarget>, IHashable
  {
    public int CalculateHash(HashCalculator calc)
    {
      var hashes = Values.Select(calc.Calculate);
      return HashCalculator.CombineCommutative(hashes);
    }

    public bool Any()
    {
      return Count > 0;
    }

    public bool None()
    {
      return Count == 0;
    }    

    public bool SharesAnyTarget(Targets targets)
    {
      return targets.Values.Any(Contains);
    }    
  }
}