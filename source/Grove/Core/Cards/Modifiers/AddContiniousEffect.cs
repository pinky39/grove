namespace Grove.Core.Cards.Modifiers
{
  public class AddContiniousEffect : Modifier
  {
    private ContiniousEffects _continiousEffects;
    private ContinuousEffect _continiousEffect;

    public IContinuousEffectFactory Effect { get; set; }
    
    public override void Apply(ContiniousEffects continiousEffects)
    {
      _continiousEffects = continiousEffects;
      _continiousEffect = Effect.Create(Source,Target, Game);
      _continiousEffects.Add(_continiousEffect);
      
      _continiousEffect.Activate();
    }
    
    protected override void Unapply()
    {
      _continiousEffects.Remove(_continiousEffect);
    }
  }
}