namespace Grove.Core
{
  using Messages;
  using Modifiers;

  public class CardTypeCharacteristic : Characteristic<CardType>, IModifiable
  {
    private readonly Card _card;
    private readonly Game _game;

    private CardTypeCharacteristic() {}

    public CardTypeCharacteristic(CardType value) : base(value, null, null) {}

    public CardTypeCharacteristic(CardType value, Game game, Card card)
      : base(value, game.ChangeTracker, card)
    {
      _game = game;
      _card = card;
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    protected override void OnCharacteristicChanged()
    {
      _game.Publish(new TypeChanged {Card = _card});
    }
  }
}