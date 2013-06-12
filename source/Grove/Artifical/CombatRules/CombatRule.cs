namespace Grove.Artifical.CombatRules
{
  using Gameplay;
  using Gameplay.Misc;

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