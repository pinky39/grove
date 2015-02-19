namespace Grove
{
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Level : Characteristic<int?>, IAcceptsCardModifier
  {
    private readonly CardBase _cardBase;
    private Card _card;

    private Level() {}


    public Level(CardBase cardBase) : base(cardBase.Value.Level)
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
      ChangeBaseValue(_cardBase.Value.Level);
    }

    protected override void OnCharacteristicChanged(int? oldValue, int? newValue)
    {
      Publish(new LevelChangedEvent(_card));
    }
  }
}