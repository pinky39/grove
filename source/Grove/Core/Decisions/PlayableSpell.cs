namespace Grove.Decisions
{
  using System;
  using System.Runtime.Serialization;

  [Serializable]
  public class PlayableSpell : Playable
  {
    public PlayableSpell() {}

    protected PlayableSpell(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public override void Play()
    {
      Card.Cast(Index, ActivationParameters);
    }

    public override string ToString()
    {
      return string.Format("spell {0} of {1}", Index, Card);
    }
  }
}