namespace Grove.Core.Cards
{
  public interface IActivatedAbilityFactory
  {      
    ActivatedAbility Create(Card card, Game game);
  }  
}