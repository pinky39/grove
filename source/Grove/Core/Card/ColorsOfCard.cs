namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  public class ColorsOfCard : Characteristic<List<CardColor>>, IEnumerable<CardColor>, IAcceptsCardModifier, IHashable
  {
    private readonly CardBase _cardBase;
    private ColorsOfCard() {}

    public ColorsOfCard(CardBase cardBase) : base(cardBase.Value.Colors)
    {
      _cardBase = cardBase;
    }

    public int Count { get { return Value.Count; } }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public IEnumerator<CardColor> GetEnumerator()
    {
      return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _cardBase.Changed += OnCardBaseChanged;
    }

    public int CalculateHash(HashCalculator calc)
    {
      var hashcodes = Value.Select(item => item.GetHashCode())
        .ToList();

      return HashCalculator.CombineCommutative(hashcodes);
    }

    protected override void AfterMemberCopy()
    {
      _cardBase.Changed += OnCardBaseChanged;
    }

    private void OnCardBaseChanged()
    {
      ChangeBaseValue(_cardBase.Value.Colors);
    }
  }
}