namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Player : GameObject, ITarget, IDamageable, IHasLife, IModifiable
  {
    public readonly ManaCache ManaCache = new ManaCache();

    private readonly AssignedDamage _assignedDamage;
    private readonly Battlefield _battlefield;
    private readonly ContiniousEffects _continiousEffects = new ContiniousEffects();
    private readonly Deck _deck;
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
    private readonly TrackableList<IPlayerModifier> _modifiers = new TrackableList<IPlayerModifier>();
    private readonly SkipSteps _skipSteps = new SkipSteps();

    public Player(PlayerParameters p, PlayerType controllerType)
    {
      Name = p.Name;
      AvatarId = p.AvatarId;
      Type = controllerType;

      _assignedDamage = new AssignedDamage(this);
      _battlefield = new Battlefield(this);
      _hand = new Hand(this);
      _graveyard = new Graveyard(this);
      _library = new Library(this);
      _exile = new Exile(this);
      _deck = p.Deck;
    }

    private Player() {}
    public PlayerType Type { get; private set; }
    public string Name { get; private set; }

    public Player Opponent
    {
      get { return Players.GetOpponent(this); }
    }

    public int LandsPlayedCount
    {
      get { return _landsPlayedCount.Value; }
      set { _landsPlayedCount.Value = value; }
    }

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

    public Deck Deck
    {
      get { return _deck; }
    }

    public IBattlefieldQuery Battlefield
    {
      get { return _battlefield; }
    }

    public IEnumerable<Card> Exile
    {
      get { return _exile; }
    }

    public bool CanMulligan
    {
      get { return _hand.CanMulligan && HasMulligan; }
    }

    public bool CanPlayLands
    {
      get { return LandsPlayedCount < _landLimit.Value; }
    }

    public IGraveyardQuery Graveyard
    {
      get { return _graveyard; }
    }

    public IHandQuery Hand
    {
      get { return _hand; }
    }

    public bool HasLost
    {
      get { return _hasLost.Value; }
      set { _hasLost.Value = value; }
    }

    public bool HasMulligan
    {
      get { return _hasMulligan.Value; }
      set { _hasMulligan.Value = value; }
    }

    public bool HasPriority
    {
      get { return _hasPriority.Value; }
      set { _hasPriority.Value = value; }
    }

    public virtual bool IsActive
    {
      get { return _isActive.Value; }
      set { _isActive.Value = value; }
    }

    public bool IsHuman
    {
      get { return Type == PlayerType.Human; }
    }

    public bool IsMachine
    {
      get { return Type == PlayerType.Machine; }
    }

    public bool IsScenario
    {
      get { return Type == PlayerType.Scenario; }
    }

    public bool IsMax { get; set; }

    public ILibraryQuery Library
    {
      get { return _library; }
    }

    public bool HasAttackedThisTurn
    {
      get { return IsActive && Game.Turn.Events.HasActivePlayerAttackedThisTurn; }
    }

    public int NumberOfCardsAboveMaximumHandSize
    {
      get { return Math.Max(0, _hand.Count - 7); }
    }

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
          CanBePrevented = damage.CanBePrevented,
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

      Publish(new DamageDealtEvent(this, damage));
    }

    public int Life
    {
      get { return _life.Value; }
      set
      {
        var oldValue = _life.Value;

        _life.Value = value;

        if (Life <= 0)
          HasLost = true;

        Publish(new LifeChangedEvent(this, value, oldValue));
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
      ManaCache.Initialize(ChangeTracker);
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

    public int GetAvailableConvertedMana(ManaUsage usage = ManaUsage.Any, bool canUseConvoke = false, bool canUseDelve = false)
    {
      var additionalUnits = canUseConvoke
       ? GetConvokeSources()
       : Enumerable.Empty<ManaUnit>();

      var fromDelveUnits = canUseDelve
        ? GetDelveSources()
        : Enumerable.Empty<ManaUnit>();

      additionalUnits = additionalUnits.Concat(fromDelveUnits);

      return ManaCache.GetAvailableConvertedMana(usage, additionalUnits);
    }

    public void AddManaToManaPool(ManaAmount manaAmount, ManaUsage usageRestriction = ManaUsage.Any)
    {
      ManaCache.AddManaToPool(manaAmount, usageRestriction);
    }

    public void AssignDamage(DamageFromSource damage)
    {
      _assignedDamage.Assign(damage);
    }

    public void Consume(ManaAmount amount, ManaUsage usage, bool canUseConvoke = false, bool canUseDelve = false)
    {
      var additionalUnits = canUseConvoke
        ? GetConvokeSources()
        : Enumerable.Empty<ManaUnit>();

      var fromDelveUnits = canUseDelve
        ? GetDelveSources()
        : Enumerable.Empty<ManaUnit>();

      var phyrexianUnits = GetPhyrexiaSources();

      additionalUnits = additionalUnits
        .Concat(fromDelveUnits)
        .Concat(phyrexianUnits);

      ManaCache.Consume(amount, usage, additionalUnits);
    }

    public void DealAssignedDamage()
    {
      _assignedDamage.Deal();
    }

    public void DiscardCard(Card card)
    {
      _graveyard.AddToEnd(card);

      Publish(new PlayerDiscardsCardEvent(this, card));
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

      Publish(new PlayerDrawsCardEvent(this));

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
      ManaCache.EmptyManaPool();
    }

    public void GetTargets(Func<Zone, Player, bool> zoneFilter, List<ITarget> targets)
    {
      targets.Add(this);
      _battlefield.GenerateZoneTargets(zoneFilter, targets);
      _hand.GenerateZoneTargets(zoneFilter, targets);
      _graveyard.GenerateZoneTargets(zoneFilter, targets);
      _library.GenerateZoneTargets(zoneFilter, targets);
    }

    public bool HasMana(int amount, ManaUsage usage = ManaUsage.Any, bool canUseConvoke = false)
    {
      return HasMana(amount.Colorless(), usage, canUseConvoke);
    }

    public bool HasMana(ManaAmount amount, ManaUsage usage = ManaUsage.Any, bool canUseConvoke = false, bool canUseDelve = false)
    {
      var additionalUnits = canUseConvoke
        ? GetConvokeSources()
        : Enumerable.Empty<ManaUnit>();

      var fromDelveUnits = canUseDelve
        ? GetDelveSources()
        : Enumerable.Empty<ManaUnit>();

      var phyrexianUnits = GetPhyrexiaSources();

      additionalUnits = additionalUnits
        .Concat(fromDelveUnits)
        .Concat(phyrexianUnits);

      return ManaCache.Has(amount, usage, additionalUnits);
    }

    private IEnumerable<ManaUnit> GetConvokeSources()
    {
      return Battlefield.Creatures
        .OrderBy(x => x.Power)
        .SelectMany(x => new ConvokeManaSource(x).GetUnits())        
        .ToList();
    }

    private IEnumerable<ManaUnit> GetDelveSources()
    {
      return Graveyard
        .OrderBy(x => x.Score)
        .SelectMany(x => new DelveManaSource(x).GetUnits())
        .ToList();
    }

    private IEnumerable<ManaUnit> GetPhyrexiaSources()
    {
      var result = new List<ManaUnit>();
      var count = this.Life/2;
      for (int i = 0; i < count; i++)
      {
        result.AddRange(new PhyrexiaManaSource(this).GetUnits());
      }
      return result;
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
      Publish(new PlayerTookMulliganEvent(this));
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

    public void PutCardIntoLibraryAtPosition(int positionFromTop, Card card)
    {
      if (card.Is().Token)
      {
        PutCardToExile(card);
        return;
      }

      _library.InsertAt(positionFromTop, card);
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

    public void PeekLibrary()
    {
      foreach (var card in _library)
      {
        card.Peek();
      }

      Publish(new PlayerSearchesLibrary(this));
    }

    public void ReorderTopCardsOfLibrary(int[] permutation)
    {
      _library.ReorderFront(permutation);
    }    

    public bool ShouldSkipStep(Step step)
    {
      return _skipSteps.Contains(step);
    }

    private void LoadLibrary()
    {
      var cards = _deck.Select(cardInfo =>
        {
          var card = Cards.Create(cardInfo.Name);
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

    [Copyable]
    private class AssignedDamage : IHashable
    {
      private readonly TrackableList<DamageFromSource> _assigned = new TrackableList<DamageFromSource>();
      private readonly Player _player;

      private AssignedDamage() {}

      public AssignedDamage(Player player)
      {
        _player = player;
      }

      public int CalculateHash(HashCalculator calc)
      {
        return calc.Calculate(_assigned);
      }

      public void Initialize(ChangeTracker changeTracker)
      {
        _assigned.Initialize(changeTracker);
      }

      public void Assign(DamageFromSource damage)
      {
        _assigned.Add(damage);
      }

      public void Deal()
      {
        foreach (var damage in _assigned)
        {
          damage.Source.DealDamageTo(damage.Amount, _player, isCombat: true);
        }
        _assigned.Clear();
      }

      public override string ToString()
      {
        return _assigned.Sum(x => x.Amount).ToString();
      }
    }
  }
}