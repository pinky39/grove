namespace Grove.Gameplay
{
  using System;
  using System.Linq;
  using Grove.Infrastructure;
  using Grove.Gameplay.Messages;

  public class PermanentCountManaOutput : ManaOutput, IReceive<ZoneChanged>
  {
    private readonly ManaColor _color;
    private readonly ControlledBy _controlledBy;
    private readonly Func<Card, bool> _filter;

    private PermanentCountManaOutput() {}

    public PermanentCountManaOutput(ManaColor color, Func<Card, bool> filter,
      ControlledBy controlledBy = ControlledBy.SpellOwner)
    {
      _color = color;
      _filter = filter;
      _controlledBy = controlledBy;
    }

    public void Receive(ZoneChanged message)
    {
      if (_controlledBy == ControlledBy.SpellOwner && message.Controller != ManaAbility.SourceCard.Controller)
      {
        return;
      }

      if (_controlledBy == ControlledBy.Opponent && message.Controller == ManaAbility.SourceCard.Controller)
      {
        return;
      }

      if (!_filter(message.Card))
        return;

      if (message.ToBattlefield)
      {
        Increased(Mana.Colored(_color, 1));
        return;
      }

      if (message.FromBattlefield)
      {
        Decreased(Mana.Colored(_color, 1));
        return;
      }
    }

    protected override IManaAmount GetAmountInternal()
    {
      int count;

      switch (_controlledBy)
      {
        case ControlledBy.SpellOwner:
          count = ManaAbility.SourceCard.Controller.Battlefield.Count(_filter);
          break;
        case ControlledBy.Opponent:
          count = ManaAbility.SourceCard.Controller.Opponent.Battlefield.Count(_filter);
          break;
        default:
          count = Players.Permanents().Count(_filter);
          break;
      }

      if (count == 0)
        return Mana.Zero;

      return new SingleColorManaAmount(_color, count);
    }
  }
}