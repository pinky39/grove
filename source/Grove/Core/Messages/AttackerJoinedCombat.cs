namespace Grove.Core.Messages
{
  using Details.Combat;

  public class AttackerJoinedCombat
  {
    public Attacker Attacker { get; set; }
    public bool WasDeclared { get; set; }
  }
}