namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class StaticAbilityParameters
  {
    public bool EnabledInAllZones;
    public List<ModifierFactory> Modifiers = new List<ModifierFactory>();
    public Func<ConditionParameters, bool> Condition = null;

    public void Modifier(ModifierFactory modifier)
    {
      Modifiers.Add(modifier);
    }

    public class ConditionParameters
    {
      private readonly Card _owningCard;
      private readonly Game _game;

      public ConditionParameters(Card owningCard, Game game)
      {
        _owningCard = owningCard;
        _game = game;
      }

      public bool OwnerControlsPermanent(Func<Card, bool> selector)
      {
        var permanenents = _game.Players.Permanents()
          .Where(c => c.Controller == _owningCard.Controller);
        return permanenents.Any(selector);
      }

      public bool PermanentExists(Func<Card, bool> selector)
      {
        var permanenents = _game.Players.Permanents();
        return permanenents.Any(selector);
      }

      public bool OwningCardHas(Static ability)
      {
        return _owningCard.Has().Has(ability);
      }
    }
  }
}