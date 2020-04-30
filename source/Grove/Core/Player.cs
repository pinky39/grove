namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Player : GameObject, ITarget, IDamageable, IHasLife, IModifiable
  {
    public readonly ManaCache ManaCache;
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
    private readonly TrackableList<Emblem> _emblems = new TrackableList<Emblem>();
    private readonly SkipSteps _skipSteps = new SkipSteps();

    public Player(PlayerParameters p, PlayerType controllerType)
    {
      Name = p.Name;
      AvatarId = p.AvatarId;
      Type = controllerType;

      ManaCache = new ManaCache(this);
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

    public Player Opponent { get { return Players.GetOpponent(this); } }

    public int LandsPlayedCount { get { return _landsPlayedCount.Value; } set { _landsPlayedCount.Value = value; } }

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

    public IZoneQuery Exile { get { return _exile; } }

    public bool CanMulligan { get { return _hand.CanMulligan && HasMulligan; } }

    public bool CanPlayLands { get { return LandsPlayedCount < _landLimit.Value; } }

    public IGraveyardQuery Graveyard { get { return _graveyard; } }

    public IHandQuery Hand { get { return _hand; } }

    public bool HasLost { get { return _hasLost.Value; } set { _hasLost.Value = value; } }

    public bool HasMulligan { get { return _hasMulligan.Value; } set { _hasMulligan.Value = value; } }

    public bool HasPriority { get { return _hasPriority.Value; } set { _hasPriority.Value = value; } }

    public virtual bool IsActive { get { return _isActive.Value; } set { _isActive.Value = value; } }

    public bool IsHuman { get { return Type == PlayerType.Human; } }

    public bool IsMachine { get { return Type == PlayerType.Machine; } }

    public bool IsScenario { get { return Type == PlayerType.Scenario; } }

    public bool IsMax { get; set; }

    public ILibraryQuery Library { get { return _library; } }

    public bool HasAttackedThisTurn { get { return IsActive && Game.Turn.Events.HasActivePlayerAttackedThisTurn; } }

    public int NumberOfCardsAboveMaximumHandSize { get { return Math.Max(0, _hand.Count - 7); } }

    public IEnumerable<Emblem> Emblems { get { return _emblems; } }    

    public int Score
    {
      get
      {
        var score = _life.Score +
          _battlefield.Score +
          _hand.Score +
          _graveyard.Score +
          _emblems.Sum(x => x.Score);


        if (HasLost)
        {
          score -= (1000 - Turn.TurnCount)*10000;
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

      var wasRedirected = Game.RedirectDamage(damage, this) ||
        RedirectDamageToPlaneswalker(damage);

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
      _continiousEffects.Initialize(null, Game);
      _landLimit.Initialize(Game, null);
      _battlefield.Initialize(Game);
      _hand.Initialize(Game);
      _graveyard.Initialize(Game);
      _library.Initialize(Game);
      _exile.Initialize(Game);
      _skipSteps.Initialize(ChangeTracker);
      _emblems.Initialize(ChangeTracker);

      LoadLibrary();
    }

    public void PutCardToBattlefield(Card card)
    {
      _battlefield.Add(card);
    }

    public int GetAvailableManaCount(
      ConvokeAndDelveOptions convokeAndDelveOptions = null,
      ManaUsage usage = ManaUsage.Any)
    {
      return GetAvailableMana(convokeAndDelveOptions, usage).Count;
    }

    public List<ManaColor> GetAvailableMana( 
      ConvokeAndDelveOptions convokeAndDelveOptions = null,
      ManaUsage usage = ManaUsage.Any)
    {      
      return ManaCache.GetAvailableMana(usage, convokeAndDelveOptions);
    }

    public void AddManaToManaPool(ManaAmount manaAmount, ManaUsage usageRestriction = ManaUsage.Any)
    {
      ManaCache.AddManaToPool(manaAmount, usageRestriction);
    }

    public void Consume(ManaAmount amount, ManaUsage usage, ConvokeAndDelveOptions convokeAndDelveOptions = null)
    {
      ManaCache.Consume(amount, usage, convokeAndDelveOptions);
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

    public bool HasMana(
      int amount, 
      ManaUsage usage = ManaUsage.Any, 
      ConvokeAndDelveOptions convokeAndDelveOptions = null)
    {
      return HasMana(amount.Colorless(), usage, convokeAndDelveOptions);
    }

    public bool HasMana(ManaAmount amount, ManaUsage usage = ManaUsage.Any,
      ConvokeAndDelveOptions convokeAndDelveOptions = null)
    {
      return ManaCache.Has(amount, usage, convokeAndDelveOptions);
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

    private bool RedirectDamageToPlaneswalker(Damage damage)
    {
      if (damage.IsCombat)
        return false;

      var planeswalkers = Battlefield.Planewalkers.ToList();
      if (planeswalkers.Count > 0)
      {
        var chosenCards = Execute<ChosenCards>(new SelectCards(Opponent, sp =>
          {
            sp.MinCount = 0;
            sp.MaxCount = 1;
            sp.Text = "Select planeswalker to redirect damage to.";
            sp.Instructions = "(or press enter to deal damage to player)";
            sp.ValidCards = planeswalkers;
            sp.SetChooseDecisionResults(_ => new ChosenCards(planeswalkers
              .OrderBy(x => -x.Score)
              .First()));
          }));

        if (chosenCards.Count == 1)
        {
          var chosenPlaneswalker = chosenCards[0];
          chosenPlaneswalker.ReceiveDamage(damage);
          return true;
        }
      }

      return false;
    }

    public void AddEmblem(Emblem emblem)
    {
      _emblems.Add(emblem);
      Publish(new EmblemAddedEvent(emblem));
    }

    public void RemoveEmblem(Emblem emblem)
    {
      _emblems.Remove(emblem);
      Publish(new EmblemRemovedEvent(emblem));
    }
  }
}