namespace Grove.Core.Ai
{
  using Infrastructure;
  using Zones;

  [Copyable]
  public class CastRestrictions
  {
    private readonly Stack _stack;
    private readonly TurnInfo _turn;

    private CastRestrictions() {}

    public CastRestrictions(Stack stack, TurnInfo turn)
    {
      _stack = stack;
      _turn = turn;
    }

    public bool IsPlayRestrictedFor(IPlayer player)
    {
      if (IsTopSpellControlledBy(player))
        return true;

      if (IsResponseToOpponentSpell(player))
        return false;

      return !IsThisAppropriateTimeToPlaySpells(player);
    }

    private bool IsResponseToOpponentSpell(IPlayer player)
    {
      return
        !_stack.IsEmpty && _stack.TopSpellOwner != player;
    }

    private bool IsThisAppropriateTimeToPlaySpells(IPlayer player)
    {
      switch (_turn.Step)
      {
        case (Step.FirstMain):
        case (Step.SecondMain):
          return player.IsActive;

        case (Step.BeginningOfCombat):
          return true;

        case (Step.DeclareAttackers):
          return true;

        case (Step.DeclareBlockers):
          return true;

        case (Step.CombatDamage):
          return player.IsActive;

        case (Step.EndOfTurn):
          return !player.IsActive;
      }

      return false;
    }

    private bool IsTopSpellControlledBy(IPlayer player)
    {
      return _stack.TopSpellOwner == player;
    }
  }
}