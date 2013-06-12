namespace Grove.Gameplay.Abilities
{
  using Infrastructure;
  using Misc;
  using Modifiers;
  
  public class TriggeredAbilities : GameObject, IAcceptsCardModifier, IHashable
  {
    private readonly TrackableList<TriggeredAbility> _abilities = new TrackableList<TriggeredAbility>();

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(TriggeredAbility ability)
    {
      _abilities.Add(ability);
    }

    public void Remove(TriggeredAbility ability)
    {
      _abilities.Remove(ability);
      ability.Dispose();
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;

      _abilities.Initialize(game.ChangeTracker, card);

      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Initialize(card, game);
      }
    }

    public void DisableAll()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Disable();
      }
    }

    public void EnableAll()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Enable();
      }
    }
  }
}