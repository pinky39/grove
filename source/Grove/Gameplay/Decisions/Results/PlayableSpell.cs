namespace Grove.Gameplay.Decisions.Results
{
  public class PlayableSpell : Playable
  {
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