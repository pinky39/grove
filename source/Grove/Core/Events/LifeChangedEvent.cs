namespace Grove.Events
{
  public class LifeChangedEvent
  {
    public readonly int NewValue;
    public readonly int OldValue;
    
    public readonly Player Player;

    public bool IsLifeGain { get { return NewValue > OldValue; } }
    public bool IsLifeLoss { get { return NewValue < OldValue; } }

    public LifeChangedEvent(Player player, int newValue, int oldValue)
    {
      Player = player;

      NewValue = newValue;
      OldValue = oldValue;
    }

    public override string ToString()
    {
      var name = Player.Name == "You" ? "Your" : Player.Name;
      return string.Format("{0} life total is {1}.", name, Player.Life);
    }
  }
}