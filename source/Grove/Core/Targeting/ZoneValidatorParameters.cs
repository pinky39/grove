namespace Grove.Core.Targeting
{
  using Core.Zones;

  public class ZoneValidatorParameters
  {
    public Zone Zone { get; private set; }
    public Player ZoneOwner { get; private set; }
    public Card Source { get; private set; }
    public Game Game { get; private set; }    

    public ZoneValidatorParameters(Zone zone, Player controller, Card source, Game game)
    {
      Zone = zone;
      ZoneOwner = controller;
      Source = source;
      Game = game;
    }
  }
}