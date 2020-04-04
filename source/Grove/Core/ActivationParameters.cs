namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;

  [Copyable, Serializable]
  public class ActivationParameters : ISerializable
  {
    public bool PayManaCost = true;
    public int Repeat = 1;
    public bool SkipStack;
    public Targets Targets = new Targets();
    public int? X;

    public List<Card> ConvokeTargets = new List<Card>();
    public List<Card> DelveTargets = new List<Card>();

    public ActivationParameters()
    {

    }

    protected ActivationParameters(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext)context.Context;

      PayManaCost = (bool) info.GetValue("PayManaCost", typeof(bool));
      Repeat = (int)info.GetValue("Repeat", typeof(int));
      SkipStack = (bool)info.GetValue("SkipStack", typeof(bool));
      Targets = (Targets)info.GetValue("Targets", typeof(Targets));
      X = (int?)info.GetValue("X", typeof(int?));

      // backward compatibility, convoke, delve
      try
      {                
        var convokeTargetsIds = (List<int>)info.GetValue("convokeTargets", typeof(List<int>));
        var delveTargetsIds = (List<int>)info.GetValue("delveTargets", typeof(List<int>));

        ConvokeTargets.AddRange(convokeTargetsIds.Select(id => (Card)ctx.Recorder.GetObject(id)));
        DelveTargets.AddRange(delveTargetsIds.Select(id => (Card)ctx.Recorder.GetObject(id)));
      }
      catch (SerializationException)
      {
        // ignore
      }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("PayManaCost", PayManaCost);
      info.AddValue("Repeat", Repeat);
      info.AddValue("SkipStack", SkipStack);
      info.AddValue("Targets", Targets);
      info.AddValue("X", X);

      var convokeTargetsIds = ConvokeTargets.Select(x => x.Id).ToList();
      var delveTargetsIds = DelveTargets.Select(x => x.Id).ToList();

      info.AddValue("convokeTargets", convokeTargetsIds);
      info.AddValue("delveTargets", delveTargetsIds);
    }
  }
}