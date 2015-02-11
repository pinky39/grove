namespace Grove
{
  using Infrastructure;
  using Modifiers;

  public class CombatCost : Characteristic<int?>, IAcceptsCardModifier
  {
    private readonly CardBase _cardBase;    

    private CombatCost() {}

    public CombatCost(CardBase cardBase)
      : base(cardBase.Value.CombatCost)
    {
      _cardBase = cardBase;      
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    protected override void AfterMemberCopy()
    {
      _cardBase.Changed += OnCardBaseChanged;      
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);      
      _cardBase.Changed += OnCardBaseChanged;
    }

    private void OnCardBaseChanged()
    {
      ChangeBaseValue(_cardBase.Value.CombatCost);
    }   
  }
}