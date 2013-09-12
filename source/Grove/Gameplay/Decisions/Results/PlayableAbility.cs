namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Runtime.Serialization;

  [Serializable]
  public class PlayableAbility : Playable
  {
    public PlayableAbility()
    {      
    }
    
    protected PlayableAbility(SerializationInfo info, StreamingContext context) : base(info, context) {}
    
    public override void Play()
    {
      Card.ActivateAbility(Index, ActivationParameters);
    }

    public override string ToString()
    {
      return string.Format("ability {0} of {1}", Index, Card);
    }
  }
}