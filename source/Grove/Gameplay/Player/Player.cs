namespace Grove.Gameplay.Player
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Card.Abilities;
  using Common;
  using Damage;
  using Grove.Infrastructure;
  using Mana;
  using Messages;
  using Modifiers;
  using Targeting;
  using Zones;

  public class Player : GameObject, ITarget, IDamageable, IAcceptsModifiers, IHasLife
  {
    private readonly AssignedDamage _assignedDamage;
    private readonly Battlefield _battlefield;
    private readonly ContiniousEffects _continuousEffects = new ContiniousEffects();
    private readonly DamagePreventions _damagePreventions = new DamagePreventions();
    private readonly DamageRedirections _damageRedirections = new DamageRedirections();
    private readonly List<string> _deck;
    private readonly Exile _exile;
    private readonly Graveyard _graveyard;
    private readonly Hand _hand;
    private readonly Trackable<bool> _hasLost = new Trackable<bool>();
    private readonly Trackable<bool> _hasMulligan = new Trackable<bool>(true);
    private readonly Trackable<bool> _hasPriority = new Trackable<bool>();
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly LandLimit _landLimit = new LandLimit(1);
    private readonly Trackable<int> _landsPlayedCount = new Trackable<int>(0);
    private readonly Library _library;
    private readonly Life _life = new Life(20);
    private readonly ManaVault _manaVault = new ManaVault();
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();

    public Player(string name, string avatar, ControllerType controller, List<string> deck)
    {
      Name = name;
      Avatar = avatar;
      Controller = controller;

      _assignedDamage = new AssignedDamage(this);
      _battlefield = new Battlefield(this);
      _hand = new Hand(this);
      _graveyard = new Graveyard(this);
      _library = new Library(this);
      _exile = new Exile(this);
      _deck = deck;
    }

    private Player() {}
    public ControllerType Controller { get; private set; }
    public string Name { get; private set; }
    public Player Opponent { get { return Players.GetOpponent(this); } }
    public int LandsPlayedCount { get { return _landsPlayedCount.Value; } set { _landsPlayedCount.Value = value; } }
    public ManaCounts ManaPool { get { return _manaVault.ManaPool; } }

    private IEnumerable<IModifiable> ModifiableProperties
    {
      get
      {
        yield return _damagePreventions;
        yield return _damageRedirections;
        yield return _continuousEffects;
        yield return _landLimit;
      }
    }

    public string Avatar { get; private set; }
    public IBattlefieldQuery Battlefield { get { return _battlefield; } }
    public IEnumerable<Card> Exile { get { return _exile; } }
    public bool CanMulligan { get { return _hand.CanMulligan && HasMulligan; } }
    public bool CanPlayLands { get { return LandsPlayedCount < _landLimit.Value; } }
    public IGraveyardQuery Graveyard { get { return _graveyard; } }
    public IHandQuery Hand { get { return _hand; } }
    public bool HasLost { get { return _hasLost.Value; } set { _hasLost.Value = value; } }
    public bool HasMulligan { get { return _hasMulligan.Value; } set { _hasMulligan.Value = value; } }
    public bool HasPriority { get { return _hasPriority.Value; } set { _hasPriority.Value = value; } }
    public virtual bool IsActive { get { return _isActive.Value; } set { _isActive.Value = value; } }
    public bool IsHuman { get { return Controller == ControllerType.Human; } }
    public bool IsMachine { get { return Controller == ControllerType.Machine; } }
    public bool IsScenario { get { return Controller == ControllerType.Scenario; } }
    public bool IsMax { get; set; }
    public ILibraryQuery Library { get { return _library; } }

    public int NumberOfCardsAboveMaximumHandSize { get { return Math.Max(0, _hand.Count - 7); } }

    public int Score
    {
      get
      {
        var score = _life.Score +
          _battlefield.Score +
            _hand.Score +
              _graveyard.Score;


        if (HasLost)
        {
          score -= (1000 - Turn.TurnCount)*1000;
        }

        return IsMax ? score : -score;
      }
    }

    public void AddModifier(IModifier modifier)
    {
      foreach (var modifiableProperty in ModifiableProperties)
      {
        modifiableProperty.Accept(modifier);
      }

      _modifiers.Add(modifier);
      modifier.Activate();
    }

    public void RemoveModifier(IModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();
    }

    public void DealDamage(Damage damage)
    {
      _damagePreventions.PreventReceivedDamage(damage);

      if (damage.Amount == 0)
        return;

      var wasRedirected = _damageRedirections.RedirectDamage(damage);

      if (wasRedirected)
        return;

      Life -= _damagePreventions.PreventLifeloss(damage.Amount);

      if (damage.Source.Has().Lifelink)
      {
        var controller = damage.Source.Controller;
        controller.Life += damage.Amount;
      }

      Publish(new DamageHasBeenDealt(this, damage));
    }

    public int Life
    {
      get { return _life.Value; }
      set
      {
        _life.Value = value;

        if (Life <= 0)
          HasLost = true;
      }
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        Life,
        HasPriority.GetHashCode(),
        IsActive.GetHashCode(),
        calc.Calculate(_assignedDamage),
        calc.Calculate(_battlefield),
        calc.Calculate(_graveyard),
        calc.Calculate(_library),
        calc.Calculate(_hand),
        _landLimit.Value.GetValueOrDefault(),
        _landsPlayedCount.Value
        );
    }

    public void AddManaSource(ManaUnit unit)
    {
      _manaVault.Add(unit);
    }

    public void RemoveManaSource(ManaUnit unit)
    {
      _manaVault.Remove(unit);
    }

    public void Initialize(Game game)
    {
      Game = game;

      _life.Initialize(ChangeTracker);
      _landsPlayedCount.Initialize(ChangeTracker);
      _hasMulligan.Initialize(ChangeTracker);
      _hasLost.Initialize(ChangeTracker);
      _isActive.Initialize(ChangeTracker);
      _hasPriority.Initialize(ChangeTracker);
      _manaVault.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);
      _damagePreventions.Initialize(this, Game);
      _damageRedirections.Initialize(ChangeTracker);
      _assignedDamage.Initialize(ChangeTracker);
      _continuousEffects.Initialize(null, Game, null);
      _landLimit.Initialize(Game, null);
      _battlefield.Initialize(Game);
      _hand.Initialize(Game);
      _graveyard.Initialize(Game);
      _library.Initialize(Game);
      _exile.Initialize(Game);

      LoadLibrary();
    }

    public void PutCardToBattlefield(Card card)
    {
      _battlefield.Add(card);
    }

    public int GetConvertedMana(ManaUsage usage = ManaUsage.Any)
    {
      return _manaVault.GetAvailableMana(usage).Converted;
    }

    public IManaAmount GetAvailableMana(ManaUsage usage = ManaUsage.Any)
    {
      return _manaVault.GetAvailableMana(usage);
    }

    public void AddManaToManaPool(IManaAmount manaAmount, ManaUsage usageRestriction = ManaUsage.Any)
    {
      _manaVault.AddManaToPool(manaAmount, usageRestriction);
    }

    public void AssignDamage(Damage damage)
    {
      _assignedDamage.Assign(damage);
    }

    public void Consume(IManaAmount amount, ManaUsage usage)
    {
      _manaVault.Consume(amount, usage);
    }

    public void DealAssignedDamage()
    {
      _assignedDamage.Deal();
    }

    public void DiscardCard(Card card)
    {
      _graveyard.Add(card);
    }

    public void DiscardHand()
    {
      foreach (var card in _hand.ToList())
      {
        DiscardCard(card);
      }
    }

    public void DiscardRandomCard()
    {
      if (_hand.IsEmpty)
        return;

      var card = _hand.RandomCard;
      DiscardCard(card);
    }

    public void DrawCard()
    {
      var card = _library.Top;

      if (card == null)
      {
        HasLost = true;
        return;
      }

      _hand.Add(card);

      if (Ai.IsSearchInProgress)
      {
        card.Hide();
      }
    }

    public void DrawCards(int cardCount)
    {
      for (var i = 0; i < cardCount; i++)
      {
        DrawCard();
      }
    }

    public void DrawStartingHand()
    {
      DrawCards(7);
      HasMulligan = true;
    }

    public void EmptyManaPool()
    {
      _manaVault.EmptyManaPool();
    }

    public IEnumerable<ITarget> GetTargets(Func<Zone, Player, bool> zoneFilter)
    {
      yield return this;

      foreach (var card in _battlefield.GenerateZoneTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (var card in _hand.GenerateZoneTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (var card in _graveyard.GenerateZoneTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (var card in _library.GenerateZoneTargets(zoneFilter))
      {
        yield return card;
      }
    }

    public bool HasMana(int amount, ManaUsage usage = ManaUsage.Any)
    {
      return _manaVault.Has(amount.Colorless(), usage);
    }

    public bool HasMana(IManaAmount amount, ManaUsage usage = ManaUsage.Any)
    {
      return _manaVault.Has(amount, usage);
    }

    public void MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard()
    {
      var creatures = _battlefield.Creatures.ToList();

      foreach (var creature in creatures)
      {
        if (creature.Toughness <= 0)
        {
          creature.Sacrifice();
          continue;
        }

        if (creature.HasLeathalDamage || creature.Life <= 0)
        {
          creature.Destroy(allowToRegenerate: true);
        }
      }
    }

    public void PutCardToGraveyard(Card card)
    {
      if (card.Is().Token)
      {
        _exile.Add(card);
        return;
      }

      _graveyard.Add(card);
    }

    public void RemoveDamageFromPermanents()
    {
      foreach (var card in _battlefield)
      {
        card.ClearDamage();
      }
    }

    public void RemoveRegenerationFromPermanents()
    {
      foreach (var permanent in _battlefield)
      {
        permanent.CanRegenerate = false;
      }
    }

    public void ShuffleIntoLibrary(Card card)
    {
      _library.Add(card);
      _library.Shuffle();
    }

    public void ShuffleLibrary()
    {
      _library.Shuffle();
    }

    public void TakeMulligan()
    {
      if (!CanMulligan)
        return;

      var mulliganSize = _hand.MulliganSize;

      foreach (var card in Hand.ToList())
      {
        _library.Add(card);
      }

      _library.Shuffle();

      for (var i = 0; i < mulliganSize; i++)
      {
        DrawCard();
      }

      HasMulligan = true;
    }

    public void PutCardToExile(Card card)
    {
      _exile.Add(card);
    }

    public void Mill(int count)
    {
      for (var i = 0; i < count; i++)
      {
        var card = _library.Top;

        if (card == null)
          return;


        if (Ai.IsSearchInProgress)
        {
          card.Hide();
        }

        _graveyard.Add(card);
      }
    }

    public void PutCardToHand(Card card)
    {
      if (card.Is().Token)
      {
        _exile.Add(card);
        return;
      }

      _hand.Add(card);
    }

    public void PutCardOnTopOfLibrary(Card card)
    {
      _library.PutOnTop(card);
    }

    private void LoadLibrary()
    {
      var cards = _deck.Select(name => CardDatabase.CreateCard(name).Initialize(this, Game));

      foreach (var card in cards)
      {
        _library.Add(card);
      }
    }

    public override string ToString()
    {
      return Name;
    }

    public void RevealHand()
    {
      foreach (var card in _hand)
      {
        card.Reveal();
      }
    }

    public void RevealLibrary()
    {
      foreach (var card in _library)
      {
        card.Reveal();
      }
    }
  }
}