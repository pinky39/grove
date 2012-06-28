namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;

  public delegate List<Targets> TargetsFilterDelegate(TargetFilterParameters parameters);

  public class TargetFilterParameters
  {
    public TargetFilterParameters(TargetCandidates candidates, Card source, int? maxX, bool forceOne, Game game)
    {
      Candidates = candidates;
      Source = source;
      MaxX = maxX;
      ForceOne = forceOne;
      Game = game;
    }

    public Game Game { get; private set; }
    public TargetCandidates Candidates { get; private set; }
    public Card Source { get; private set; }
    public int? MaxX { get; private set; }
    public bool ForceOne { get; private set; }
    public Player Opponent { get { return Game.Players.GetOpponent(Source.Controller); } }
    public Player Controller { get { return Source.Controller; } }
    public Combat Combat { get { return Game.Combat; } }
  }
}