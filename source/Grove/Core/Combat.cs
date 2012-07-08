namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Combat : IHashable
  {
    private readonly IAttackerFactory _attackerFactory;
    private readonly TrackableList<Attacker> _attackers;
    private readonly IBlockerFactory _blockerFactory;
    private readonly TrackableList<Blocker> _blockers;
    private readonly Players _players;
    private readonly Publisher _publisher;

    private Combat() {}

    public Combat(ChangeTracker changeTracker, IAttackerFactory attackerFactory, IBlockerFactory blockerFactory,
                  Publisher publisher, Players players)
    {
      _attackerFactory = attackerFactory;
      _blockerFactory = blockerFactory;
      _publisher = publisher;
      _players = players;
      _attackers = new TrackableList<Attacker>(changeTracker);
      _blockers = new TrackableList<Blocker>(changeTracker);
    }

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void AssignCombatDamage(Decisions decisions, bool firstStrike = false)
    {
      IEnumerable<Blocker> blockers = firstStrike
        ? _blockers.Where(x => x.Card.HasFirstStrike)
        : _blockers.Where(x => x.Card.HasNormalStrike);

      IEnumerable<Attacker> attackers = firstStrike
        ? _attackers.Where(x => x.Card.HasFirstStrike)
        : _attackers.Where(x => x.Card.HasNormalStrike);

      foreach (Blocker blocker in blockers)
      {
        blocker.DistributeDamageToAttacker();
      }

      foreach (Attacker attacker in attackers)
      {
        attacker.DistributeDamageToBlockers(decisions);
      }
    }

    public bool CanAnyAttackerBeBlockedByAny(IEnumerable<Card> creatures)
    {
      foreach (Attacker attacker in _attackers)
      {
        foreach (Card creature in creatures)
        {
          if (attacker.CanBeBlockedBy(creature))
            return true;
        }
      }

      return false;
    }

    public void DealAssignedDamage()
    {
      foreach (Attacker attacker in _attackers)
      {
        attacker.DealAssignedDamage();
      }

      foreach (Blocker blocker in _blockers)
      {
        blocker.DealAssignedDamage();
      }

      _players.Defending.DealAssignedDamage();
    }

    public void JoinAttack(Card card, bool wasDeclared = false)
    {
      Attacker attacker = _attackerFactory.Create(card);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      PublishMessage(new AttackerJoinedCombat
        {
          Attacker = attacker,
          WasDeclared = wasDeclared
        });
    }

    public void DeclareAttacker(Card card)
    {
      if (!card.CanAttack)
        return;

      JoinAttack(card, wasDeclared: true);
    }

    public void DeclareBlocker(Card cardBlocker, Card cardAttacker)
    {
      Attacker attacker = FindAttacker(cardAttacker);
      Blocker blocker = _blockerFactory.Create(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      _publisher.Publish(new BlockerDeclared
        {
          Blocker = blocker,
          Attacker = attacker
        });
    }

    public bool IsAttacker(Card card)
    {
      return FindAttacker(card) != null;
    }

    public bool IsBlocker(Card card)
    {
      return FindBlocker(card) != null;
    }

    public void Remove(Card card)
    {
      Attacker attacker = FindAttacker(card);

      if (attacker != null)
      {
        _attackers.Remove(attacker);
        attacker.RemoveFromCombat();
        return;
      }

      Blocker blocker = FindBlocker(card);

      if (blocker != null)
      {
        _blockers.Remove(blocker);
        blocker.RemoveFromCombat();
      }
    }

    public void RemoveAll()
    {
      foreach (Blocker blocker in _blockers)
      {
        blocker.RemoveFromCombat();
      }

      foreach (Attacker attacker in _attackers)
      {
        attacker.RemoveFromCombat();
      }

      _blockers.Clear();
      _attackers.Clear();
    }

    public void SetDamageAssignmentOrder(Decisions decisions)
    {
      foreach (Attacker attacker in _attackers)
      {
        decisions.EnqueueSetDamageAssignmentOrder(
          attacker.Controller, attacker);
      }
    }

    private Attacker FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    private Blocker FindBlocker(Card cardBlocker)
    {
      return _blockers.FirstOrDefault(b => b.Card == cardBlocker);
    }

    private void PublishMessage<TMessage>(TMessage message)
    {
      _publisher.Publish(message);
    }

    public bool AnyCreaturesWithFirstStrike()
    {
      return _attackers.Any(x => x.Card.HasFirstStrike) ||
        _blockers.Any(x => x.Card.HasFirstStrike);
    }

    public bool AnyCreaturesWithNormalStrike()
    {
      return _attackers.Any(x => x.Card.HasNormalStrike) ||
        _blockers.Any(x => x.Card.HasNormalStrike);
    }

    public static bool CanAttackerBeDealtLeathalCombatDamage(Card attacker, IEnumerable<Card> blockers,
                                                             int additionalPower, int additionalToughness,
                                                             out Card singleBlockerThatDealsKillingBlow)
    {
      var blockersThatDealLeathalDamage = new List<Card>();
      int leathalDamage = 0;
      int greatestDamage = 0;
      Card blockerThatDealsGreatestDamage = null;

      singleBlockerThatDealsKillingBlow = null;

      int total = 0;

      if (!attacker.CanBeDestroyed)
        return false;

      if (attacker.HasFirstStrike)
      {
        // eliminate blockers that will be killed
        // before they can deal damage
        blockers = blockers.Where(
          b => (b.HasFirstStrike || b.LifepointsLeft > (attacker.Power + additionalPower)));
      }

      foreach (Card blocker in blockers)
      {
        var damage = new Damage(blocker, blocker.Power.Value);
        int dealtAmount = attacker.CalculateDealtDamageAmount(damage);

        if (dealtAmount > 0 && damage.IsLeathal)
        {
          blockersThatDealLeathalDamage.Add(blocker.Card());
          leathalDamage += dealtAmount;
        }
        if (dealtAmount > greatestDamage)
        {
          greatestDamage = dealtAmount;
          blockerThatDealsGreatestDamage = blocker;
        }
        total += dealtAmount;
      }

      if (blockersThatDealLeathalDamage.Count > 1)
        return true;

      int lifepoints = attacker.LifepointsLeft + additionalToughness;

      if (blockersThatDealLeathalDamage.Count == 1)
      {
        if (lifepoints > total - leathalDamage)
        {
          singleBlockerThatDealsKillingBlow = blockersThatDealLeathalDamage[0];
        }
        return true;
      }

      if (lifepoints <= total && lifepoints > total - greatestDamage)
      {
        singleBlockerThatDealsKillingBlow = blockerThatDealsGreatestDamage;
        return true;
      }

      return false;
    }

    public static bool CanAttackerBeDealtLeathalCombatDamage(Card attacker, IEnumerable<Card> blockers,
                                                             int additionalPower = 0, int additionalThougness = 0)
    {
      Card killerBlocker;

      return CanAttackerBeDealtLeathalCombatDamage(attacker, blockers, additionalPower, additionalThougness,
        out killerBlocker);
    }

    public static int CalculateTrampleDamage(Card attacker, Card blocker)
    {
      return CalculateTrampleDamage(attacker, blocker.ToEnumerable());
    }

    public static int CalculateTrampleDamage(Card attacker, IEnumerable<Card> blockers)
    {
      if (attacker.Has().Trample == false)
        return 0;

      return attacker.TotalDamageThisCanDealInAllDamageSteps - blockers.Sum(x => x.LifepointsLeft);
    }

    public static int CalculateDefendingPlayerLifeloss(Card attacker, IEnumerable<Card> blockers)
    {
      if (blockers.None())
        return attacker.Power.Value;

      if (attacker.Has().Trample)
      {
        int? totalToughness = blockers.Sum(x => x.Toughness);
        int? diff = attacker.Power - totalToughness;

        return (diff > 0 ? diff : 0).Value;
      }

      return 0;
    }

    public static bool CanBlockerBeDealtLeathalCombatDamage(Card blocker, Card attacker,
      int additionalPower = 0, int additionalThoughness = 0)
    {
      if (!blocker.CanBeDestroyed)
        return false;

      if (blocker.HasFirstStrike && !attacker.HasFirstStrike && !attacker.Has().Indestructible)
      {
        // can blocker kill attacker before it can even deal damage        
        var blockerDamage = new Damage(blocker, blocker.Power.Value + additionalPower);
        int blockerAmount = attacker.CalculateDealtDamageAmount(blockerDamage);

        if (blockerAmount > 0 && blockerDamage.IsLeathal)
        {
          return false;
        }

        if (blockerAmount >= attacker.LifepointsLeft)
          return false;
      }

      var attackerDamage = new Damage(attacker, attacker.Power.Value);
      int attackerAmount = blocker.CalculateDealtDamageAmount(attackerDamage);

      if (attackerAmount == 0)
        return false;

      if (attackerDamage.IsLeathal)
        return true;

      return attackerAmount >= blocker.LifepointsLeft + additionalThoughness;
    }

    public bool CanBeDealtLeathalCombatDamage(Card card)
    {
      Attacker attacker = FindAttacker(card);

      if (attacker != null)
      {
        return attacker.WillBeDealtLeathalCombatDamage();
      }

      Blocker blocker = FindBlocker(card);

      if (blocker != null)
      {
        return blocker.WillBeDealtLeathalCombatDamage();
      }

      return false;
    }

    public bool HasBlockers(Card card)
    {
      Attacker attacker = FindAttacker(card);
      return attacker.BlockersCount > 0;
    }

    public bool CanBlockAnyAttacker(Card card)
    {
      return Attackers.Any(attacker => attacker.CanBeBlockedBy(card));
    }

    public int CalculateGainIfGivenABoost(Card attackerOrBlocker, int power, int thougness)
    {
      Attacker attacker = FindAttacker(attackerOrBlocker);

      if (attacker != null)
      {
        return attacker.CalculateGainIfGivenABoost(power, thougness);
      }

      Blocker blocker = FindBlocker(attackerOrBlocker);

      if (blocker != null)
      {
        return blocker.CalculateGainIfGivenABoost(power, thougness);
      }

      return 0;
    }

    public int CountHowManyThisCouldBlock(Card card)
    {
      Player opponent = _players.GetOpponent(card.Controller);
      return opponent.Battlefield.CreaturesThatCanAttack.Count(x => x.CanBeBlockedBy(card));
    }

    public bool CouldBeBlockedByAny(Card card)
    {
      Player opponent = _players.GetOpponent(card.Controller);
      return opponent.Battlefield.CreaturesThatCanBlock.Any(card.CanBeBlockedBy);
    }

    public Card GetBestDamagePreventionCandidateForAttackerOrBlocker(Card attackerOrBlocker)
    {
      Attacker attacker = FindAttacker(attackerOrBlocker);

      if (attacker != null)
      {
        return attacker.GetBestDamagePreventionCandidate();
      }

      Blocker blocker = FindBlocker(attackerOrBlocker);

      if (blocker != null)
      {
        return blocker.GetBestDamagePreventionCandidate();
      }

      return null;
    }

    public Card GetAttackerWhichWillDealGreatestDamageToDefender()
    {
      return Attackers.OrderByDescending(x => x.GetDamageThisWillDealToPlayer())
        .FirstOrDefault();
    }
  }
}