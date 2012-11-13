namespace Grove.Core.Targeting
{
  using Cards;

  public interface ITriggeredAbilityFactory
  {
    TriggeredAbility Create(Card owningCard, Card sourceCard, Game game);
  }
}