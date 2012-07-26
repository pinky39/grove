namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using Targeting;  
  
  public class AiDamageDistributor : IDamageDistributor
  {
    public Func<IList<ITarget>, int, IList<int>> Distribution { get; set; }

    public IList<int> DistributeDamage(IList<ITarget> targets, int damage)
    {
      return Distribution(targets, damage);
    }
  }
}