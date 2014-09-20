namespace Grove.AI.CombatRules
{
  public abstract class CombatRule : GameObject
  {
    protected Card OwningCard { get; private set; }
    public abstract void Apply(CombatAbilities combatAbilities);

    public void Initialize(Card owningCard, Game game)
    {
      Game = game;
      OwningCard = owningCard;
    }
  }
}