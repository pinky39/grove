namespace Grove.Modifiers
{
  public class AddNamedGameModifier : Modifier, IGameModifier
  {
    private readonly Static _namedModifier;
    private NamedGameModifiers _namedModifiers;
    private AddToList<Static> _modifier;

    private AddNamedGameModifier() {}

    public AddNamedGameModifier(Static namedModifier)
    {
      _namedModifier = namedModifier;
    }

    public override void Apply(NamedGameModifiers namedGameModifiers)
    {
      _namedModifiers = namedGameModifiers;

      _modifier = new AddToList<Static>(_namedModifier);
      _modifier.Initialize(ChangeTracker);

      _namedModifiers.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _namedModifiers.RemoveModifier(_modifier);
    }
  }
}