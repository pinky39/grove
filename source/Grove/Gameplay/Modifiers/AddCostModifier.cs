namespace Grove.Gameplay.Modifiers
{
  using Costs;

  public class AddCostModifier : Modifier, IGameModifier
  {
    private readonly CostModifier _costModifier;
    private CostModifiers _costModifiers;

    private AddCostModifier() {}

    public AddCostModifier(CostModifier costModifier)
    {
      _costModifier = costModifier;
    }

    public override void Apply(CostModifiers costModifiers)
    {
      _costModifiers = costModifiers;
      _costModifier.Initialize(SourceCard, Game);
      _costModifiers.Add(_costModifier);
    }

    protected override void Unapply()
    {
      _costModifiers.Remove(_costModifier);
    }
  }
}