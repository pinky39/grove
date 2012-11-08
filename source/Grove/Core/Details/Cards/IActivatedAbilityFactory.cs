namespace Grove.Core.Details.Cards
{
  public interface IActivatedAbilityFactory
  {      
    ActivatedAbility Create(Card card, Game game);
  }  
}