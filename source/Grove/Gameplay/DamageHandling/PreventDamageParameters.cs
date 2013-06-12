namespace Grove.Gameplay.DamageHandling
{
  using Targeting;

  public class PreventDamageParameters
  {
    public int Amount;
    public bool IsCombat;
    public bool QueryOnly = true;
    public Card Source;
    public ITarget Target;
  }
}