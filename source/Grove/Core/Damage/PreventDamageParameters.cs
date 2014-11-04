namespace Grove
{
  public class PreventDamageParameters
  {
    public int Amount;
    public bool IsCombat;
    public bool CanBePrevented = true;
    public bool QueryOnly = true;
    public Card Source;
    public ITarget Target;
  }
}