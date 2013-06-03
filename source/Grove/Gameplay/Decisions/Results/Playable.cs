namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Runtime.Serialization;
  using Abilities;
  using Infrastructure;
  using Persistance;

  [Copyable, Serializable]
  public abstract class Playable : ISerializable, IPlayable
  {
    public ActivationParameters ActivationParameters = new ActivationParameters();
    public Card Card;
    public int Index;
    protected Playable() {}

    protected Playable(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;

      ActivationParameters = (ActivationParameters) info.GetValue("parameters", typeof (ActivationParameters));
      Card = (Card)ctx.Recorder.GetObject(info.GetInt32("card"));
      Index = info.GetInt32("index");
    }
    
    public virtual bool WasPriorityPassed { get { return false; } }
    public Player Controller { get { return Card.Controller; } }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("parameters", ActivationParameters);
      info.AddValue("card", Card.Id);
      info.AddValue("index", Index);
    }

    public virtual bool CanPlay()
    {
      return true;
    }

    public virtual void Play() {}
  }
}