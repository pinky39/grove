namespace Grove.Gameplay.Decisions.Results
{
  using System;

  [Serializable]
  public class PlayableAbility : Playable
  {
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