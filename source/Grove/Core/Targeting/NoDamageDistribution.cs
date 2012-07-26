namespace Grove.Core.Targeting
{
  using System.Collections.Generic;

  public class NoDamageDistribution : IDamageDistributor
  {
    public IList<int> DistributeDamage(IList<ITarget> targets, int damage)
    {
      return new [] {damage};
    }
  }
}