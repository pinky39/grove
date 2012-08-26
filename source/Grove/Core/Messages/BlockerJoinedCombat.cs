namespace Grove.Core.Messages
{
  using Details.Combat;

  public class BlockerJoinedCombat
  {
    public Blocker Blocker { get; set; }
    public Attacker Attacker { get; set; }
  }
}