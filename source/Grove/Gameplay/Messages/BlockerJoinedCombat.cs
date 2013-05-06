namespace Grove.Gameplay.Messages
{
  public class BlockerJoinedCombat
  {
    public Blocker Blocker { get; set; }
    public Attacker Attacker { get; set; }
  }
}