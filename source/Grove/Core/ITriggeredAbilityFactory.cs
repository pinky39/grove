namespace Grove.Core
{
  public interface ITriggeredAbilityFactory
  {
    TriggeredAbility Create(Card owningCard, Card sourceCard);
  }
}