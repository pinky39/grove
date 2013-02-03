namespace Grove.Core.Targeting
{
  using Core.Zones;

  public class ZoneValidatorDelegateParameters : GameObject
  {
    public Zone Zone { get; private set; }
    public Player ZoneOwner { get; private set; }
    public Card Source;
    
    public ZoneValidatorDelegateParameters(Zone zone, Player controller, Game game)
    {
      Zone = zone;
      ZoneOwner = controller;      
      Game = game;
    }
  }
}