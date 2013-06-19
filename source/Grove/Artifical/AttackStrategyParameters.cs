namespace Grove.Artifical
{
  using System.Collections.Generic;
  using Gameplay;

  public class AttackStrategyParameters
  {    
    public int DefendingPlayersLife;
    public List<Card> AttackerCandidates;
    public List<Card> BlockerCandidates;
  }
}