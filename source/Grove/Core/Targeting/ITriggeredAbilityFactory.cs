namespace Grove.Core.Targeting
{
  using Details.Cards;

  public interface ITriggeredAbilityFactory
  {
    TriggeredAbility Create(Card owningCard, Card sourceCard, Game game);
  }
}