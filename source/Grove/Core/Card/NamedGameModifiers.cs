namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public interface INamedGameModifiers
  {
    bool CreaturesEnteringBattlefieldDontTriggerAbilities { get; }
  }

  public class NamedGameModifiers : GameObject, IAcceptsGameModifier, INamedGameModifiers
  {
    private readonly Characteristic<List<Static>> _abilities;

    public NamedGameModifiers()
    {
      _abilities = new Characteristic<List<Static>>(new List<Static>());
    }

    public void Initialize(Game game)
    {
      Game = game;
      _abilities.Initialize(game, null);
    }


    private bool Has(Static ability)
    {
      return _abilities.Value.Any(x => x == ability);
    }

    public void Accept(IGameModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AddModifier(PropertyModifier<List<Static>> modifier)
    {
      _abilities.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<List<Static>> modifier)
    {
      _abilities.RemoveModifier(modifier);
    }

    public bool CreaturesEnteringBattlefieldDontTriggerAbilities { get { return Has(Static.CreaturesEnteringBattlefieldDontTriggerAbilities); } }
  }
}