namespace Grove.Gameplay.Messages
{
  using Combat;

  public class AttackerJoinedCombat
  {
    public Attacker Attacker { get; set; }
    public bool WasDeclared { get; set; }
  }
}