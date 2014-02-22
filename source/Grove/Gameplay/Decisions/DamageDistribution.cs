namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Grove.Infrastructure;

  [Copyable, Serializable]
  public class DamageDistribution : ISerializable
  {
    private readonly Dictionary<Blocker, int> _distribution = new Dictionary<Blocker, int>();

    public DamageDistribution() {}

    protected DamageDistribution(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;

      var distribution = (List<BlockerDamage>) info.GetValue("distribution", typeof (List<BlockerDamage>));

      foreach (var blockerDamage in distribution)
      {
        var blocker = ctx.Game.Combat.FindBlocker((Card) ctx.Recorder.GetObject(blockerDamage.BlockerId));
        _distribution.Add(blocker, blockerDamage.Damage);
      }      
    }

    public int this[Blocker blocker]
    {
      get
      {
        var assigned = 0;
        _distribution.TryGetValue(blocker, out assigned);
        return assigned;
      }
    }

    public int Total { get { return _distribution.Sum(x => x.Value); } }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      var distribution = _distribution.Select(x => new BlockerDamage
        {
          BlockerId = x.Key.Card.Id,
          Damage = x.Value
        })
        .ToList();

      info.AddValue("distribution", distribution);      
    }

    public void Assign(Blocker blocker, int amount)
    {
      var assigned = 0;
      _distribution.TryGetValue(blocker, out assigned);
      _distribution[blocker] = assigned + amount;
    }    

    [Serializable]
    public class BlockerDamage
    {
      public int BlockerId;
      public int Damage;
    }
  }
}