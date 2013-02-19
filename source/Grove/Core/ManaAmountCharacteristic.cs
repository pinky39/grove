namespace Grove.Core
{
  using Mana;

  public class ManaAmountCharacteristic : Characteristic<IManaAmount>
  {
    private ManaAmountCharacteristic() {}

    public ManaAmountCharacteristic(IManaAmount value)
      : base(value) {}
  }
}