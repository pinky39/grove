namespace Grove
{
  using Infrastructure;
  using Modifiers;

  public class CardBase : GameObject, IAcceptsCardModifier, ICopyContributor
  {
    private readonly Characteristic<CardParameters> _parameters;
    public TrackableEvent Changed = new TrackableEvent();

    private CardBase() {}

    public CardBase(CardParameters cardParameters)
    {
      _parameters = new Characteristic<CardParameters>(cardParameters);
    }

    public CardParameters Value { get { return _parameters.Value; } }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AddModifier(PropertyModifier<CardParameters> modifier)
    {
      _parameters.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<CardParameters> modifier)
    {
      _parameters.RemoveModifier(modifier);
    }

    public void Initialize(Game game, IHashDependancy hashDependancy)
    {
      Game = game;

      Changed.Initialize(ChangeTracker);      
      _parameters.Initialize(game, hashDependancy);
      _parameters.Changed += OnParametersChanged;
    }

    private void OnParametersChanged(CharacteristicChangedParams<CardParameters> p)
    {
      Changed.Raise();
    }

    public void AfterMemberCopy(object original)
    {
      _parameters.Changed += OnParametersChanged;
    }
  }
}