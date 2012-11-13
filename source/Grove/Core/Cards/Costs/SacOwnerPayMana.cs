namespace Grove.Core.Cards.Costs
{
  public class SacOwnerPayMana : TapAndSacOwnerPayMana
  {
    public SacOwnerPayMana()
    {
      TapOwner = false;
    }
  }
}