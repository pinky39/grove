namespace Grove.Core.Targeting
{
  public interface ITriggeredAbilityFactory
  {
    TriggeredAbility Create(Card owningCard, Card sourceCard, Game game);
  }
}