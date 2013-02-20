namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Messages;
  using Modifiers;
  using Preventions;
  using Redirections;
  using Targeting;
  using Zones;
  
  public class Player : GameObject, ITarget, IDamageable, IAcceptsModifiers, IHasLife
  {
    private readonly AssignedDamage _assignedDamage;
    private readonly Battlefield _battlefield;
    private readonly ContiniousEffects _continuousEffects = new ContiniousEffects();
    private readonly DamagePreventions _damagePreventions = new DamagePreventions();
    private readonly DamageRedirections _damageRedirections = new DamageRedirections();
    private readonly Exile _exile;    
    private readonly Graveyard _graveyard;
    private readonly Hand _hand;
    private readonly Trackable<bool> _hasLost = new Trackable<bool>();
    private readonly Trackable<bool> _hasMulligan = new Trackable<bool>(true);
    private readonly Trackable<bool> _hasPriority = new Trackable<bool>();
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly LandLimit _landLimit;
    private readonly Trackable<int> _landsPlayedCount = new Trackable<int>(0);
    private readonly Library _library;
    private readonly Life _life = new Life(20);
    private readonly ManaPool _manaPool = new ManaPool();
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();
    private ManaSources _manaSources;

    public void Initialize(Game game)
    {
      Game = game;
      
      _life.Initialize(ChangeTracker);
      _landsPlayedCount.Initialize(ChangeTracker);
      _hasMulligan.Initialize(ChangeTracker);
      _hasLost.Initialize(ChangeTracker);
      _isActive.Initialize(ChangeTracker);
      _hasPriority.Initialize(ChangeTracker);
      _manaPool.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);
      _damagePreventions.Initialize(this, Game);
      _damageRedirections.Initialize(ChangeTracker);
      _assignedDamage.Initialize(ChangeTracker);
      _continuousEffects.Initialize();
    }
    
    public Player(string name, string avatar, ControllerType controller, IEnumerable<string> deck)
    {      
      Name = name;
      Avatar = avatar;
      Controller = controller;                              
                              
      _assignedDamage = new AssignedDamage(this);
      _continuousEffects = new ContiniousEffects();
      _landLimit = new LandLimit(1, game.ChangeTracker, null);

      _battlefield = new Battlefield(this, game);
      _hand = new Hand(this, game);
      _graveyard = new Graveyard(this, game);
      _library = new Library(this, game);
      _exile = new Exile(this, game);


      IEnumerable<Card> cards = LoadCards(deck, _game.CardDatabase);
      foreach (Card card in cards)
      {
        _library.Add(card);
      }

      InitializeManaSources(game.ChangeTracker);
    }

    private Player() {}
    public ControllerType Controller { get; private set; }
    public ManaPool ManaPool { get { return _manaPool; } }
    public string Name { get; private set; }
    public Player Opponent { get { return _game.Players.GetOpponent(this); } }

    public int LandsPlayedCount { get { return _landsPlayedCount.Value; } set { _landsPlayedCount.Value = value; } }

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
        int score = _life.Score +
          _battlefield.Score +
            _hand.Score +
              _graveyard.Score;

        return IsMax ? score : -score;
      }
    }

    public void AddModifier(IModifier modifier)
    {
      foreach (IModifiable modifiableProperty in ModifiableProperties)
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

      bool wasRedirected = _damageRedirections.RedirectDamage(damage);

      if (wasRedirected)
        return;

      Life -= _damagePreventions.PreventLifeloss(damage.Amount);

      if (damage.Source.Has().Lifelink)
      {
        Player controller = damage.Source.Controller;
        controller.Life += damage.Amount;
      }

      _game.Publish(new DamageHasBeenDealt(this, damage));
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

    public void PutCardToBattlefield(Card card)
    {
      _battlefield.Add(card);
    }

    public int GetConvertedMana(ManaUsage usage = ManaUsage.Any)
    {
      return _manaSources.GetMaxConvertedMana(usage);
    }

    public void AddManaToManaPool(IManaAmount manaAmount, bool useOnlyForAbilities = false)
    {
      if (useOnlyForAbilities)
      {
        _manaPool.AddAbilities(manaAmount);
        return;
      }

      _manaPool.Add(manaAmount);
    }

    public IManaAmount GetAmountOfManaInPool(ManaUsage manaUsage = ManaUsage.Any)
    {
      return _manaPool.GetAvailableMana(manaUsage);
    }

    public void AssignDamage(Damage damage)
    {
      _assignedDamage.Assign(damage);
    }

    public void Consume(IManaAmount amount, ManaUsage usage, IManaSource tryNotToConsumeThisSource = null)
    {
      _manaSources.Consume(amount, usage, tryNotToConsumeThisSource);
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
      foreach (Card card in _hand.ToList())
      {
        DiscardCard(card);
      }
    }

    public void DiscardRandomCard()
    {
      if (_hand.IsEmpty)
        return;

      Card card = _hand.RandomCard;
      DiscardCard(card);
    }

    public void DrawCard()
    {
      Card card = _library.Top;

      if (card == null)
      {
        HasLost = true;
        return;
      }

      _hand.Add(card);

      if (_game.Search.InProgress)
      {
        card.Hide();
      }
    }

    public void DrawCards(int cardCount)
    {
      for (int i = 0; i < cardCount; i++)
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
      _manaPool.Empty();
    }


    public IEnumerable<ITarget> GetTargets(Func<Zone, Player, bool> zoneFilter)
    {
      yield return this;

      foreach (Card card in _battlefield.GenerateTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (Card card in _hand.GenerateTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (ITarget card in _graveyard.GenerateTargets(zoneFilter))
      {
        yield return card;
      }

      foreach (ITarget card in _library.GenerateTargets(zoneFilter))
      {
        yield return card;
      }
    }

    public bool HasMana(int amount, ManaUsage usage = ManaUsage.Any)
    {
      return _manaSources.GetMaxConvertedMana(usage) >= amount;
    }

    public bool HasMana(IManaAmount amount, ManaUsage usage = ManaUsage.Any)
    {
      if (amount == null)
        return true;

      return _manaSources.Has(amount, usage);
    }

    public void MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard()
    {
      List<Card> creatures = _battlefield.Creatures.ToList();

      foreach (Card creature in creatures)
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
      foreach (Card card in _battlefield)
      {
        card.ClearDamage();
      }
    }

    public void RemoveRegenerationFromPermanents()
    {
      foreach (Card permanent in _battlefield)
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

      int mulliganSize = _hand.MulliganSize;

      foreach (Card card in Hand.ToList())
      {
        _library.Add(card);
      }

      _library.Shuffle();

      for (int i = 0; i < mulliganSize; i++)
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
      for (int i = 0; i < count; i++)
      {
        Card card = _library.Top;

        if (card == null)
          return;

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

    private IEnumerable<Card> LoadCards(IEnumerable<string> deck, CardDatabase cardDatabase)
    {
      return deck.Select(card => cardDatabase.CreateCard(card, this, _game));
    }

    public void AddManaSources(IEnumerable<IManaSource> manaSources)
    {
      _manaSources.AddRange(manaSources);
    }

    public override string ToString()
    {
      return Name;
    }

    private void InitializeManaSources(ChangeTracker changeTracker)
    {
      IEnumerable<IManaSource> manaSources =
        _manaPool.ToEnumerable().Concat(
          _library
            .Where(x => x.IsManaSource)
            .SelectMany(x => x.ManaSources));

      _manaSources = new ManaSources(manaSources, changeTracker);
    }

    public void AddManaSource(IManaSource manaSource)
    {
      _manaSources.Add(manaSource);
    }

    public void RemoveManaSource(IManaSource manaSource)
    {
      _manaSources.Remove(manaSource);
    }

    public void RevealHand()
    {
      foreach (Card card in _hand)
      {
        card.Reveal();
      }
    }

    public void RevealLibrary()
    {
      foreach (Card card in _library)
      {
        card.Reveal();
      }
    }
  }
}