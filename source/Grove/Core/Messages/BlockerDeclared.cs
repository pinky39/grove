namespace Grove.Core.Messages
{
  using Details.Combat;

  public class BlockerDeclared
  {
    public Blocker Blocker { get; set; }
    public Attacker Attacker { get; set; }
  }
}