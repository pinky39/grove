namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Abilities;
  using Characteristics;
  using DamageHandling;
  using Infrastructure;
  using ManaHandling;
  using Messages;
  using Misc;
  using Modifiers;
  using States;
  using Targeting;
  using Zones;

  public class Player : GameObject, ITarget, IDamageable, IHasLife, IModifiable
  {
    private readonly AssignedDamage _assignedDamage;
    private readonly Battlefield _battlefield;
    private readonly Deck _deck;
    private readonly Exile _exile;
    private readonly Graveyard _graveyard;
    private readonly Hand _hand;
    private readonly Trackable<bool> _hasLost = new Trackable<bool>();
    private readonly Trackable<bool> _hasMulligan = new Trackable<bool>(true);
    private readonly Trackable<bool> _hasPriority = new Trackable<bool>();
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly SkipSteps _skipSteps = new SkipSteps();
    private readonly LandLimit _landLimit = new LandLimit(1);
    private readonly Trackable<int> _landsPlayedCount = new Trackable<int>(0);
    private readonly Library _library;
    private readonly Life _life = new Life(20);
    private readonly ManaVault _manaVault = new ManaVault();
    private readonly TrackableList<IPlayerModifier> _modifiers = new TrackableList<IPlayerModifier>();
    private readonly ContiniousEffects _continiousEffects = new ContiniousEffects();

    public Player(PlayerParameters p, ControllerType controllerType)
    {
      Name = p.Name;
      AvatarId = p.AvatarId;
      Controller = controllerType;

      _assignedDamage = new AssignedDamage(this);
      _battlefield = new Battlefield(this);
      _hand = new Hand(this);
      _graveyard = new Graveyard(this);
      _library = new Library(this);
      _exile = new Exile(this);
      _deck = p.Deck;
    }

    private Player() {}
    public ControllerType Controller { get; private set; }
    public string Name { get; private set; }
    public Player Opponent { get { return Players.GetOpponent(this); } }
    public int LandsPlayedCount { get { return _landsPlayedCount.Value; } set { _landsPlayedCount.Value = value; } }
    public ManaCounts ManaPool { get { return _manaVault.ManaPool; } }

    private IEnumerable<IAcceptsPlayerModifier> ModifiableProperties
    {
      get
      {
        yield return _landLimit;
        yield return _continiousEffects;
        yield return _skipSteps;
      }
    }

    public int AvatarId { get; private set; }
    public Deck Deck { get { return _deck; } }
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

    public void ReceiveDamage(Damage damage)
    {
      var p = new PreventDamageParameters
        {
          Amount = damage.Amount,
          Source = damage.Source,
          Target = this,
          IsCombat = damage.IsCombat,
          QueryOnly = false
        };

      var prevented = Game.PreventDamage(p);
      damage.Amount -= prevented;

      if (damage.Amount == 0)
        return;

      var wasRedirected = Game.RedirectDamage(damage, this);

      if (wasRedirected)
        return;

      var preventedLifeloss = Game.PreventLifeloss(damage.Amount, this, queryOnly: false);
      
      Life -= (damage.Amount - preventedLifeloss);

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

        Publish(new PlayerLifeChanged {Player = this});
      }
    }

    void IModifiable.RemoveModifier(IModifier modifier)
    {
      RemoveModifier((IPlayerModifier) modifier);
    }

    public int Id { get; private set; }

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

    public int CalculatePreventedReceivedDamageAmount(int totalAmount, Card source, bool isCombat = false)
    {
      var p = new PreventDamageParameters
        {
          Amount = totalAmount,
          Source = source,
          Target = this,
          IsCombat = isCombat,
          QueryOnly = true
        };

      return Game.PreventDamage(p);
    }

    public void AddModifier(IPlayerModifier modifier, ModifierParameters p)
    {
      p.Owner = this;
      _modifiers.Add(modifier);

      modifier.Initialize(p, Game);
      modifier.Activate();

      foreach (var modifiableProperty in ModifiableProperties)
      {
        modifiableProperty.Accept(modifier);
      }
    }

    public void RemoveModifier(IPlayerModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();
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
      Id = game.Recorder.CreateId(this);

      _life.Initialize(ChangeTracker);
      _landsPlayedCount.Initialize(ChangeTracker);
      _hasMulligan.Initialize(ChangeTracker);
      _hasLost.Initialize(ChangeTracker);
      _isActive.Initialize(ChangeTracker);
      _hasPriority.Initialize(ChangeTracker);
      _manaVault.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);      
      _assignedDamage.Initialize(ChangeTracker);
      _continiousEffects.Initialize(null, Game);
      _landLimit.Initialize(Game, null);
      _battlefield.Initialize(Game);
      _hand.Initialize(Game);
      _graveyard.Initialize(Game);
      _library.Initialize(Game);
      _exile.Initialize(Game);
      _skipSteps.Initialize(ChangeTracker);

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

    public void AssignDamage(DamageFromSource damage)
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
      _graveyard.AddToEnd(card);
    }

    public void DiscardHand()
    {
      foreach (var card in _hand.ToList())
      {
        DiscardCard(card);
      }
    }

    public Card DiscardRandomCard()
    {
      if (_hand.IsEmpty)
        return null;

      var card = _hand.RandomCard;
      DiscardCard(card);
      return card;
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

    public void GetTargets(Func<Zone, Player, bool> zoneFilter, List<ITarget> targets)
    {
      targets.Add(this);
      _battlefield.GenerateZoneTargets(zoneFilter, targets);
      _hand.GenerateZoneTargets(zoneFilter, targets);
      _graveyard.GenerateZoneTargets(zoneFilter, targets);
      _library.GenerateZoneTargets(zoneFilter, targets);           
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
        PutCardToExile(card);
        return;
      }
      
      _graveyard.AddToEnd(card);
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
        permanent.HasRegenerationShield = false;
      }
    }

    public void ShuffleIntoLibrary(IEnumerable<Card> cards)
    {
      foreach (var card in cards)
      {
        PutOnBottomOfLibrary(card);
      }

      _library.Shuffle();
    }

    public void ShuffleIntoLibrary(Card card)
    {
      PutOnBottomOfLibrary(card);
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
        _library.PutOnBottom(card);
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

        _graveyard.AddToEnd(card);
      }
    }

    public void PutCardToHand(Card card)
    {            
      if (card.Is().Token)
      {
        PutCardToExile(card);
        return;
      }
      
      _hand.Add(card);
    }

    public void PutCardOnTopOfLibrary(Card card)
    {
      if (card.Is().Token)
      {
        PutCardToExile(card);
        return;
      }
      
      _library.PutOnTop(card);
    }

    public void PutOnBottomOfLibrary(Card card)
    {
      if (card.Is().Token)
      {
        PutCardToExile(card);
        return;
      }
      
      _library.PutOnBottom(card);
    }

    private void LoadLibrary()
    {
      var cards = _deck.Select(cardInfo =>
        {
          var card = CardFactory.CreateCard(cardInfo.Name);
          card.Rarity = cardInfo.Rarity;
          card.Set = cardInfo.Set;

          card.Initialize(this, Game);

          return card;
        });

      foreach (var card in cards)
      {
        _library.PutOnBottom(card);
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

    public void ReorderTopCardsOfLibrary(int[] permutation)
    {
      _library.ReorderFront(permutation);
    }

    public bool ShouldSkipStep(Step step)
    {
      return _skipSteps.Contains(step);
    }
  }
}