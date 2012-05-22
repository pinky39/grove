namespace Grove.Core.Messages
{
  public class PlayerHasCastASpell
  {
    public bool HasTarget
    {
      get { return Target != null; }
    }

    public Card Spell { get; set; }
    public object Target { get; set; }
  }
}