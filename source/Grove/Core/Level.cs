namespace Grove.Core
{
  using Messages;
  using Modifiers;

  public class Level : Characteristic<int?>, IModifiable
  {
    private readonly Card _card;
    private readonly Game _game;

    private Level() {}

    public Level(int? value) : base(value, null, null) {}

    public Level(int? value, Game game, Card card)
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
      _game.Publish(new LevelChanged
        {
          Card = _card
        });
    }
  }
}