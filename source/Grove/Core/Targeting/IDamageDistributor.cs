namespace Grove.Core.Targeting
{
  using System.Collections.Generic;

  public interface IDamageDistributor
  {
    IList<int> DistributeDamage(IList<ITarget> targets, int damage);
  }
}