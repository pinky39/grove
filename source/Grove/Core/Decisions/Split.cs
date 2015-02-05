namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;

  [Serializable]
  public class Split : ISerializable
  {
    private readonly List<List<Card>> _groups;

    public Split()
    {
      _groups = new List<List<Card>>();
    }

    public Split(IEnumerable<IEnumerable<Card>> groups)
    {
      _groups = groups.Select(x => x.ToList()).ToList();
    }

    public Split(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;
      var groupsWithCardIds = (List<List<int>>) info.GetValue("groups", typeof (List<List<int>>));

      _groups = groupsWithCardIds
        .Select(x => x
          .Select(y => (Card) ctx.Recorder.GetObject(y))
          .ToList())
        .ToList();
    }

    public List<List<Card>> Groups { get { return _groups; } }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      var groupsWithCardIds = _groups.Select(x => x.Select(y => y.Id).ToList()).ToList();
      info.AddValue("groups", groupsWithCardIds);
    }
  }
}