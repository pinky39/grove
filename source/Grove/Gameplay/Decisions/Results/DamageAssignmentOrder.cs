namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;
  using Persistance;

  [Copyable, Serializable]
  public class DamageAssignmentOrder : ISerializable
  {
    private readonly Dictionary<Blocker, int> _assignmentOrder = new Dictionary<Blocker, int>();

    public DamageAssignmentOrder() {}

    protected DamageAssignmentOrder(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;

      var assignmentOrder = (List<BlockerOrder>) info.GetValue("assignmentOrder", typeof (List<BlockerOrder>));

      foreach (var blockerOrder in assignmentOrder)
      {
        var blocker = ctx.Game.Combat.FindBlocker((Card) ctx.Recorder.GetObject(blockerOrder.BlockerId));
        _assignmentOrder.Add(blocker, blockerOrder.Order);
      }
    }

    public int this[Blocker blocker] { get { return _assignmentOrder[blocker]; } }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      var assignmentOrder = _assignmentOrder.Select(x => new BlockerOrder
        {
          BlockerId = x.Key.Card.Id,
          Order = x.Value
        })
        .ToList();

      info.AddValue("assignmentOrder", assignmentOrder);
    }

    public void Assign(Blocker blocker, int order)
    {
      _assignmentOrder[blocker] = order;
    }

    [Serializable]
    public class BlockerOrder
    {
      public int BlockerId;
      public int Order;
    }
  }
}