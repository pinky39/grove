namespace Grove.UserInterface.Messages
{
  using Grove.Gameplay;

  public class BlockerSelected
  {
    public Card Blocker { get; set; }
    public Card Attacker { get; set; }
  }
}