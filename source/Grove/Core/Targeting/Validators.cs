namespace Grove.Core.Targeting
{
  using System;
  using Ai;

  public static class Validators
  {
    public static IsValidTarget Player()
    {
      return p => p.Target.IsPlayer();
    }

    public static IsValidTarget CreatureOrPlayer()
    {
      return p => p.Target.IsPlayer() || (p.Target.IsCard() && p.Target.Card().Is().Creature);
    }

    public static IsValidTarget CounterableSpell(Func<Card, bool> filter = null)
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

    public static IsValidTarget AttackerOrBlocker()
    {
      return p => p.Target.IsCard() && (p.OwningCard.IsAttacker || p.OwningCard.IsBlocker);
    }

    public static IsValidTarget ValidEquipmentTarget()
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

    public static IsValidTarget Card(Func<IsValidTargetParameters, bool> filter)
    {
      filter = filter ?? delegate { return true; };
      return p => p.Target.IsCard() && filter(p);
    }

    public static IsValidTarget Card(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return p => p.Target.IsCard() &&                  
          filter(p.Target.Card());
    }
    
    public static IsValidTarget Card(ControlledBy controlledBy, Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return p => p.Target.IsCard() &&        
          p.IsControlledBy(controlledBy) &&          
          filter(p.Target.Card());
    }        
  }
}