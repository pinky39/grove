namespace Grove
{
  using System;

  public class IsValidTargetBuilder
  {
    private IsValidZoneBuilder _zoneBuilder;
    public Func<IsValidTargetParameters, bool> IsValidTarget { get; private set; }
    public Func<IsValidZoneParameters, bool> IsValidZone { get { return _zoneBuilder == null ? null : _zoneBuilder.IsValidZone; } }
    public bool MustBeTargetable { get { return _zoneBuilder != null && _zoneBuilder.MustBeTargetable; } }

    public IsValidTargetBuilder Is
    {
      get { return this; }
    }

    public IsValidTargetBuilder Player()
    {
      IsValidTarget = p => p.Target.IsPlayer();
      return this;
    }

    public IsValidZoneBuilder CreatureOrPlayer()
    {
      IsValidTarget = p => p.Target.IsPlayer() || (p.Target.IsCard() && p.Target.Card().Is().Creature);
      return CreateZoneBuilder();
    }


    public IsValidZoneBuilder CounterableSpell(Func<Effect, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      IsValidTarget = p => p.Target.IsEffect() &&
        p.Target.Effect().CanBeCountered &&
        p.Target.Effect().Source is CastRule &&
        filter(p.Target.Effect());

      return CreateZoneBuilder();
    }

    public IsValidZoneBuilder AttackerOrBlocker()
    {
      IsValidTarget =
        p => p.Target.IsCard() && (p.Target.Card().IsAttacker || p.Target.Card().IsBlocker);
      return CreateZoneBuilder();
    }

    public IsValidZoneBuilder ValidEquipmentTarget()
    {
      IsValidTarget = p =>
        {
          var equipment = p.OwningCard;

          if (!p.Target.Is().Creature) return false;

          if (p.Target.Card().Controller != equipment.Controller)
            return false;

          return !equipment.IsAttached || equipment.AttachedTo != p.Target;
        };

      return CreateZoneBuilder();
    }

    public IsValidZoneBuilder Card(Func<IsValidTargetParameters, bool> filter)
    {
      filter = filter ?? delegate { return true; };
      IsValidTarget = p => p.Target.IsCard() && filter(p);
      return CreateZoneBuilder();
    }

    public IsValidZoneBuilder Card(Func<Card, bool> filter = null, ControlledBy? controlledBy = null,
      bool canTargetSelf = true)
    {
      filter = filter ?? delegate { return true; };

      IsValidTarget = p =>
        {
          if (p.Target == p.OwningCard && !canTargetSelf)
          {
            return false;
          }

          var hasValidController = controlledBy == null || HasValidController(
            p.Target.Controller(),
            p.Controller,
            controlledBy.Value);

          if (p.Target.IsCard())
          {
            return hasValidController &&
              filter(p.Target.Card());
          }

          if (p.Target.IsEffect())
          {
            return hasValidController &&
              filter(p.Target.Effect().Source.OwningCard);
          }

          return false;
        };

      return CreateZoneBuilder();
    }

    private static bool HasValidController(Player targetController, Player sourceController, ControlledBy controlledBy)
    {
      switch (controlledBy)
      {
        case (ControlledBy.Opponent):
          return targetController != sourceController;

        case (ControlledBy.SpellOwner):
          return targetController == sourceController;
      }

      return true;
    }

    public IsValidZoneBuilder Creature(ControlledBy? controlledBy = null, bool canTargetSelf = true)
    {
      return Card(x => x.Is().Creature, controlledBy, canTargetSelf);
    }

    public IsValidZoneBuilder Enchantment()
    {
      return Card(x => x.Is().Enchantment);
    }


    private IsValidZoneBuilder CreateZoneBuilder()
    {
      _zoneBuilder = new IsValidZoneBuilder(this);
      return _zoneBuilder;
    }
  }
}