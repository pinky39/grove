namespace Grove.UserInterface.Messages
{
  using Grove.Gameplay.Targeting;

  public class SelectionChanged
  {
    public ITarget Selection { get; set; }
  }
}