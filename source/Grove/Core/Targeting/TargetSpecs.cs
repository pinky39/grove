namespace Grove.Core.Targeting
{
  using System;

  public class TargetSpecs
  {
    private readonly TargetValidatorParameters _p;

    public TargetSpecs(TargetValidatorParameters parameters)
    {
      _p = parameters;
    }

    public TargetValidatorParameters Player()
    {
      _p.TargetSpec = p => p.Target.IsPlayer();
      return _p;
    }

    public TargetValidatorParameters CreatureOrPlayer()
    {
      _p.TargetSpec = p => p.Target.IsPlayer() || (p.Target.IsCard() && p.Target.Card().Is().Creature);
      return _p;
    }

    public TargetValidatorParameters Counterable(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      _p.TargetSpec = p =>
        {
          return p.Target.IsEffect() &&
            p.Target.Effect().CanBeCountered &&
              p.Target.Effect().Source is CastInstruction &&
                filter(p.Target.Effect().Source.OwningCard);
        };

      return _p;
    }

    public TargetValidatorParameters AttackerOrBlocker()
    {
      _p.TargetSpec =
        p => p.Target.IsCard() && (p.Effect.Source.OwningCard.IsAttacker || p.Effect.Source.OwningCard.IsBlocker);
      return _p;
    }

    public TargetValidatorParameters ValidEquipmentTarget()
    {
      _p.TargetSpec = p =>
        {
          var equipment = p.Effect.Source.OwningCard;

          if (!p.Target.Is().Creature) return false;

          if (p.Target.Card().Controller != equipment.Controller)
            return false;

          return !equipment.IsAttached || equipment.AttachedTo != p.Target;
        };

      return _p;
    }

    public TargetValidatorParameters Card(Func<TargetValidatorDelegateParameters, bool> filter)
    {
      filter = filter ?? delegate { return true; };
      _p.TargetSpec = p => p.Target.IsCard() && filter(p);
      return _p;
    }

    public TargetValidatorParameters Card(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      _p.TargetSpec = p => p.Target.IsCard() &&
        filter(p.Target.Card());

      return _p;
    }

    public TargetValidatorParameters Creature()
    {
      return Card(x => x.Is().Creature);
    }

    public TargetValidatorParameters Enchantment()
    {
      return Card(x => x.Is().Enchantment);
    }
  }
}