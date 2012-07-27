namespace Grove.Core.Targeting
{
  using System.Collections.Generic;

  public class NoDamageDistributor : IDamageDistributor
  {
    public IList<int> DistributeDamage(IList<ITarget> targets, int damage)
    {
      return new [] {damage};
    }
  }
}