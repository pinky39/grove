namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Messages;
  using Modifiers;

  public class Level : Characteristic<int?>, IModifiable
  {
    private Card _card;

    private Level() {}
    public Level(int? value) : base(value) {}

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
      Publish(new LevelChanged
        {
          Card = _card
        });
    }
  }
}