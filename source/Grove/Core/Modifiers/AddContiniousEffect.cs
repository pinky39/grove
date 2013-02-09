namespace Grove.Core.Modifiers
{
  public class AddContiniousEffect : Modifier
  {
    private readonly ContinuousEffect _continiousEffect;
    private ContiniousEffects _continiousEffects;

    private AddContiniousEffect() {}

    public AddContiniousEffect(ContinuousEffect continiousEffect)
    {
      _continiousEffect = continiousEffect;
    }

    public override Modifier Initialize(ModifierParameters p, Game game)
    {
      base.Initialize(p, game);
      _continiousEffect.Initialize(Source, Game, Target);
      return this;
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