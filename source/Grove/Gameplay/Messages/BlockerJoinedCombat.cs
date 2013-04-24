namespace Grove.Gameplay.Messages
{
  using Combat;

  public class BlockerJoinedCombat
  {
    public Blocker Blocker { get; set; }
    public Attacker Attacker { get; set; }
  }
}