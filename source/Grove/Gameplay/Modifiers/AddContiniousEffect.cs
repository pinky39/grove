namespace Grove.Gameplay.Modifiers
{
  using Card.Abilities;

  public class AddContiniousEffect : Modifier
  {
    private readonly ContinuousEffect _continiousEffect;
    private ContiniousEffects _continiousEffects;

    private AddContiniousEffect() {}

    public AddContiniousEffect(ContinuousEffect continiousEffect)
    {
      _continiousEffect = continiousEffect;
    }

    protected override void Initialize()
    {
      _continiousEffect.Initialize(Source, Game, Target);
    }       

    public override void Apply(ContiniousEffects continiousEffects)
    {
      _continiousEffects = continiousEffects;      
      _continiousEffects.Add(_continiousEffect);

      _continiousEffect.Activate();
    }

    protected override void Unapply()
    {
      _continiousEffects.Remove(_continiousEffect);
    }
  }
}