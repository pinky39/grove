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

    public bool IsPlayRestrictedFor(Player player)
    {
      if (IsTopSpellControlledBy(player))
        return true;

      if (IsResponseToOpponentSpell(player))
        return false;

      return !IsThisAppropriateTimeToPlaySpells(player);
    }

    private bool IsResponseToOpponentSpell(Player player)
    {
      return
        !_stack.IsEmpty && _stack.TopSpellOwner != player;
    }

    private bool IsThisAppropriateTimeToPlaySpells(Player player)
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
      }

      return false;
    }

    private bool IsTopSpellControlledBy(Player player)
    {
      return _stack.TopSpellOwner == player;
    }
  }
}