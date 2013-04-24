namespace Grove.Gameplay.Decisions
{
  using System;
  using Card;

  public interface ICardValidator
  {
    bool IsValidCard(Card card);
  }

  public class DelegateCardValidator : ICardValidator
  {
    private readonly Func<Card, bool> _validator;

    public DelegateCardValidator(Func<Card, bool> validator)
    {
      _validator = validator;
    }

    public bool IsValidCard(Card card)
    {
      return _validator(card);
    }
  }
}