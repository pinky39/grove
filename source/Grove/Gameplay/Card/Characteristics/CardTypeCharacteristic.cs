namespace Grove.Gameplay.Card.Characteristics
{
  using Infrastructure;
  using Messages;
  using Modifiers;

  public class CardTypeCharacteristic : Characteristic<CardType>, IModifiable
  {
    private Card _card;
    private CardTypeCharacteristic() {}
    public CardTypeCharacteristic(CardType value) : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _card = (Card) hashDependancy;
    }

    protected override void OnCharacteristicChanged()
    {
      Publish(new TypeChanged {Card = _card});
    }
  }
}