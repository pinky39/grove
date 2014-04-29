namespace Grove
{
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Level : Characteristic<int?>, IAcceptsCardModifier
  {
    private Card _card;

    private Level() {}
    public Level(int? value) : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);

      _card = (Card) hashDependancy;
    }

    protected override void OnCharacteristicChanged(int? oldValue, int? newValue)
    {
      Publish(new LevelChangedEvent(_card));
    }
  }
}