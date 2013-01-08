namespace Grove.Core.Targeting
{
  using System;
  using Ai;

  public static class Validators
  {
    public static TargetValidatorDelegate Player()
    {
      return p => p.Target.IsPlayer();
    }

    public static TargetValidatorDelegate CreatureOrPlayer()
    {
      return p => p.Target.IsPlayer() || (p.Target.IsCard() && p.Target.Card().Is().Creature);
    }

    public static TargetValidatorDelegate CounterableSpell(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return p =>
        {
          return p.Target.IsEffect() &&
            p.Target.Effect().CanBeCountered &&
              p.Target.Effect().Source is CastInstruction &&
                filter(p.Target.Effect().Source.OwningCard);
        };
    }    

    public static TargetValidatorDelegate AttackerOrBlocker()
    {
      return p => p.Target.IsCard() && (p.Card.IsAttacker || p.Card.IsBlocker);
    }

    public static TargetValidatorDelegate ValidEquipmentTarget()
    {
      return p =>
        {
          var equipment = p.Source;          

          if (!p.Target.Is().Creature) return false;

          if (p.Target.Card().Controller != equipment.Controller)
            return false;

          return !equipment.IsAttached || equipment.AttachedTo != p.Target;
        };
    }    

    private static bool ValidateController(Player spellController, Player targetController, Controller controller)
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

    public static TargetValidatorDelegate Card(Func<TargetValidatorParameters, bool> filter)
    {
      filter = filter ?? delegate { return true; };
      return p => p.Target.IsCard() && filter(p);
    }

    public static TargetValidatorDelegate Card(Func<Card, bool> filter = null, Controller controller = Controller.Any)
    {
      filter = filter ?? delegate { return true; };

      return p => 
          p.Target.IsCard() &&        
          ValidateController(p.Controller, p.Card.Controller, controller) &&
          filter(p.Target.Card());
    }        
  }
}