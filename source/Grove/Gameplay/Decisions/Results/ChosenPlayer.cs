namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using Infrastructure;

  [Copyable, Serializable]
  public class ChosenPlayer
  {
    private ChosenPlayer() {}

    public ChosenPlayer(Player player)
    {
      Player = player;
    }

    public Player Player { get; private set; }
  }
}