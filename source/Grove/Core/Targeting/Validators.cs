namespace Grove.Core.Targeting
{
  using System;
  using System.Linq;
  using Ai;
  using Zones;

  public static class Validators
  {
    public static TargetValidatorDelegate Player()
    {
      return p => p.Target.IsPlayer();
    }

    public static TargetValidatorDelegate CreatureOrPlayer()
    {
      return p => p.Target.IsPlayer() || (p.Target.IsPermanent() && p.Target.Card().Is().Creature);
    }

    public static TargetValidatorDelegate Counterspell(params string[] types)
    {
      return p =>
        {
          var isValid = p.Target.IsEffect() && p.Target.Effect().CanBeCountered &&
            p.Target.Effect().Source is Card;

          if (types.Length == 0)
            return isValid;

          var owner = p.Target.Effect().Source.OwningCard;
          return isValid && types.Any(owner.Is);
        };
    }

    public static TargetValidatorDelegate Permanent(Func<Card, bool> filter = null,
      Controller controller = Controller.Any)
    {
      filter = filter ?? delegate { return true; };
      return p => p.Target.IsPermanent() && IsValidController(p.Controller, p.Target.Card().Controller, controller) && filter(p.Target.Card());
    }

    public static TargetValidatorDelegate AttackerOrBlocker()
    {
      return p => p.Target.IsPermanent() && (p.Target.Card().IsAttacker || p.Target.Card().IsBlocker);
    }

    public static TargetValidatorDelegate Equipment()
    {
      return p =>
        {
          var equipment = p.Source;

          if (!p.Target.IsPermanent())
            return false;

          if (!p.Target.Is().Creature) return false;

          if (p.Target.Card().Controller != equipment.Controller)
            return false;

          return !equipment.IsAttached || equipment.AttachedTo != p.Target;
        };
    }

    public static TargetValidatorDelegate EnchantedCreature()
    {
      return p => p.Target.IsPermanent() && p.Target.Is().Creature;
    }

    public static TargetValidatorDelegate Creature(Func<TargetValidatorParameters, bool> filter)
    {
      return p => p.Target.IsPermanent() && p.Target.Is().Creature && filter(p);
    }

    private static bool IsValidController(Player spellController, Player targetController, Controller controller)
    {
      switch (controller)
      {
        case (Controller.SpellOwner):
          return spellController == targetController;
        case (Controller.Opponent):
          return spellController != targetController;
      }
      return true;
    }

    public static TargetValidatorDelegate Creature(Func<Card, bool> filter = null,
      Controller controller = Controller.Any)
    {
      filter = filter ?? delegate { return true; };


      return p => p.Target.IsPermanent() && p.Target.Is().Creature &&
        IsValidController(p.Controller, p.Target.Card().Controller, controller) && filter(p.Target.Card());
    }

    public static TargetValidatorDelegate EffectOrPermanent(Func<ITarget, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };
      return p => (p.Target.IsPermanent() || p.Target.IsEffect()) && filter(p.Target);
    }

    public static TargetValidatorDelegate CardInHand(Func<TargetValidatorParameters, bool> filter = null, bool yourHandOnly = true)
    {
      filter = filter ?? delegate { return true; };

      return p => p.Target.IsCard() && 
        (p.Target.Card().Zone == Zone.Hand) && 
        (!yourHandOnly || p.Target.Card().Controller == p.Controller) &&
          filter(p);
    }

    public static TargetValidatorDelegate CardInHand(Func<Card, bool> filter = null, bool yourHandOnly = true)
    {
      filter = filter ?? delegate { return true; };

      return p => p.Target.IsCard() && 
        (p.Target.Card().Zone == Zone.Hand) && 
        (!yourHandOnly || p.Target.Card().Controller == p.Controller) &&
          filter(p.Target.Card());
    }

    public static TargetValidatorDelegate CardInGraveyard(Func<Card, bool> filter = null, 
      bool yourGraveyardOnly = true)
    {
      filter = filter ?? delegate { return true; };

      return
        p => p.Target.IsCard() &&
          p.Target.Card().Zone == Zone.Graveyard &&
           (!yourGraveyardOnly || p.Target.Card().Controller == p.Controller) &&
              filter(p.Target.Card());
    }
  }
}