namespace Grove.Modifiers
{
  using Events;

  public class ChangeCardTemplate : Modifier, ICardModifier
  {
    private readonly CardTemplate _template;
    private CardBase _cardBase;
    private CardParametersSetter _modifier;

    private ChangeCardTemplate() {}

    public ChangeCardTemplate(CardTemplate template)
    {
      _template = template;
    }

    public override void Apply(CardBase cardBase)
    {
      _cardBase = cardBase;
      var cardParameters = _template.CreateCardParameters();
      cardParameters.Initialize(OwningCard, Game);

      _modifier = new CardParametersSetter(cardParameters);
      _modifier.Initialize(ChangeTracker);
      cardBase.AddModifier(_modifier);      
    }

    protected override void Unapply()
    {
      _cardBase.RemoveModifier(_modifier);
    }
  }
}