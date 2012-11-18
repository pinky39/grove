namespace Grove.Core.Cards
{
  using Infrastructure;
  using Mana;

  public class ManaAmountCharacteristic : Characteristic<IManaAmount>
  {
    private ManaAmountCharacteristic() {}

    public ManaAmountCharacteristic(IManaAmount value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}
  }
}