namespace Grove
{
  using Events;
  using Infrastructure;
  using Modifiers;

  public class TypeOfCard : Characteristic<CardType>, IAcceptsCardModifier
  {
    private readonly CardBase _cardBase;
    private Card _card;
    private TypeOfCard() {}

    public TypeOfCard(CardBase cardBase) : base(cardBase.Value.Type)
    {
      _cardBase = cardBase;      
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    protected override void AfterMemberCopy()
    {
      _cardBase.Changed += OnCardBaseChanged;
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _card = (Card) hashDependancy;
      _cardBase.Changed += OnCardBaseChanged;
    }

    private void OnCardBaseChanged()
    {
      ChangeBaseValue(_cardBase.Value.Type);
    }

    protected override void OnCharacteristicChanged(CardType oldValue, CardType newValue)
    {
      Publish(new TypeChangedEvent(_card, oldValue, newValue));
    }
  }
}