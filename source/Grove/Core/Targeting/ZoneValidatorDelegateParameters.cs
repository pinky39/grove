namespace Grove.Core.Targeting
{
  using Core.Zones;

  public class ZoneValidatorDelegateParameters : GameObject
  {
    public Zone Zone { get; private set; }
    public Player ZoneOwner { get; private set; }
    public Card Source { get; private set; }
    
    public ZoneValidatorDelegateParameters(Zone zone, Card source, Player controller, Game game)
    {
      Zone = zone;
      Source = source;
      ZoneOwner = controller;      
      Game = game;
    }
  }
}