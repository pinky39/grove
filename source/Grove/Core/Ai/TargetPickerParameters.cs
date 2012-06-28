namespace Grove.Core.Ai
{
  using System.Collections.Generic;

  public delegate List<Targets> TargetsFilterDelegate(TargetPickerParameters parameters);
  
  public class TargetPickerParameters
  {    
    public TargetPickerParameters(TargetCandidates candidates, int? maxX, bool forceOne, Game game)
    {      
      Candidates = candidates;
      MaxX = maxX;
      ForceOne = forceOne;
      Game = game;
    }

    public Game Game { get; private set; }
    public TargetCandidates Candidates { get; private set; }
    public int? MaxX { get; private set; }
    public bool ForceOne { get; private set; }
  }
}