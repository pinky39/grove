namespace Grove.Ui.CombatDamage
{
  using System;
  using Core;
  using Core.Controllers.Results;
  using Infrastructure;

  public class ViewModel
  {
    private readonly BlockerDamageAssignments _assignments;
    private readonly Attacker _attacker;
    private readonly Game _game;
    private readonly DamageDistribution _damageDistribution;    
    private int _damageToAssign;

    public ViewModel(Game game, Attacker attacker, DamageDistribution damageDistribution)
    {
      _game = game;
      _damageDistribution = damageDistribution;
      _assignments = new BlockerDamageAssignments(attacker);
      _attacker = attacker;
      _damageToAssign = attacker.DamageThisWillDealInOneDamageStep;
    }

    public Card Attacker { get { return _attacker.Card; } }
    public BlockerDamageAssignments Blockers { get { return _assignments; } }
    public bool CanAccept { get { return HasAllDamageBeenAssigned; } }
    private bool HasAllDamageBeenAssigned { get { return _damageToAssign == 0; } }

    public string Title { get { return String.Format("Distribute combat damage: {0} left.", _damageToAssign); } }

    public void Accept()
    {
      foreach (var assignment in _assignments)
      {
        _damageDistribution.Assign(assignment.Blocker, assignment.Damage);
      }

      Close();
    }

    [Updates("CanAccept", "Title")]
    public virtual void AssignOneDamage(BlockerDamageAssignment blocker)
    {
      if (!HasAllDamageBeenAssigned && _assignments.CanAssignCombatDamageTo(blocker))
      {
        _damageToAssign -= 1;
        blocker.Damage++;
      }
    }

    public void ChangePlayersInterest(Card card)
    {
      _game.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    [Updates("CanAccept", "Title")]
    public virtual void Clear()
    {
      _assignments.Clear();
      _damageToAssign = _attacker.DamageThisWillDealInOneDamageStep;
    }

    public virtual void Close() {}


    public interface IFactory
    {
      ViewModel Create(Attacker attacker, DamageDistribution damageDistribution);
    }
  }
}