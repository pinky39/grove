namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Details.Cards.Modifiers;
  using Details.Cards.Preventions;
  using Details.Cards.Redirections;
  using Details.Combat;
  using Details.Mana;
  using Infrastructure;
  using Messages;
  using Targeting;
  using Zones;

  [Copyable]
  public class Player : ICardOwner, ICardController
  {
    private readonly AssignedDamage _assignedDamage;
    private readonly Trackable<bool> _canPlayLands;
    private readonly DamagePreventions _damagePreventions;
    private readonly DamageRedirections _damageRedirections;
    private readonly Trackable<bool> _hasLost;
    private readonly Trackable<bool> _hasMulligan;
    private readonly Trackable<bool> _hasPriority;
    private readonly Trackable<bool> _isActive;
    private readonly Life _life;
    private readonly ManaPool _manaPool;
    private readonly TrackableList<IModifier> _modifiers;
    private readonly Publisher _publisher;
    private readonly PlayerType _type;
    private Battlefield _battlefield;
    private Exile _exile;
    private Graveyard _graveyard;
    private Hand _hand;
    private Library _library;
    private ManaSources _manaSources;

    public Player(
      string name,
      string avatar,
      PlayerType type,
      string deck,
      Game game, 
      DeckFactory deckFactory)
    {
      _publisher = game.Publisher;

      Name = name;
      Avatar = avatar;
      _type = type;

      _life = new Life(20, game.ChangeTracker);
      _canPlayLands = new Trackable<bool>(true, game.ChangeTracker);
      _hasMulligan = new Trackable<bool>(true, game.ChangeTracker);
      _hasLost = new Trackable<bool>(game.ChangeTracker);
      _isActive = new Trackable<bool>(game.ChangeTracker);
      _hasPriority = new Trackable<bool>(game.ChangeTracker);
      _manaPool = Bindable.Create<ManaPool>(game.ChangeTracker);
      _modifiers = new TrackableList<IModifier>(game.ChangeTracker);
      _damagePreventions = new DamagePreventions(game.ChangeTracker, null);
      _damageRedirections = new DamageRedirections(game.ChangeTracker, null);
      _assignedDamage = new AssignedDamage(this, game.ChangeTracker);

      var cards = deckFactory.CreateDeck(deck, this);

      CreateZones(cards, game);
      InitializeManaSources();
    }

    private Player() {}
    public object ManaPool { get { return _manaPool; } }
    public string Name { get; private set; }

    private IEnumerable<IModifiable> ModifiableProperties
    {
      get
      {
        yield return _damagePreventions;
        yield return _damageRedirections;
      }
    }

    public void PutCardToBattlefield(Card card)
    {
      _battlefield.Add(card);
    }

    public string Avatar { get; private set; }
    public IBattlefieldQuery Battlefield { get { return _battlefield; } }
    public bool CanMulligan { get { return _hand.CanMulligan && HasMulligan; } }
    public bool CanPlayLands { get { return _canPlayLands.Value; } set { _canPlayLands.Value = value; } }
    public IEnumerable<Card> Graveyard { get { return _graveyard; } }
    public IHandQuery Hand { get { return _hand; } }
    public bool HasLost { get { return _hasLost.Value; } set { _hasLost.Value = value; } }
    public bool HasMulligan { get { return _hasMulligan.Value; } set { _hasMulligan.Value = value; } }
    public bool HasPriority { get { return _hasPriority.Value; } set { _hasPriority.Value = value; } }
    public virtual bool IsActive { get { return _isActive.Value; } set { _isActive.Value = value; } }
    public bool IsHuman { get { return _type == PlayerType.Human; } }
    public bool IsComputer { get { return _type == PlayerType.Computer; } }
    public bool IsScenario { get { return _type == PlayerType.Scenario; } }
    public bool IsMax { get; set; }

    public IEnumerable<Card> Library { get { return _library; } }

    public virtual int Life
    {
      get { return _life.Value; }
      set
      {
        _life.Value = value;

        if (Life <= 0)
          HasLost = true;
      }
    }

    public int NumberOfCardsAboveMaximumHandSize { get { return Math.Max(0, _hand.Count - 7); } }

    public int Score
    {
      get
      {
        var score = _life.Score +
          _battlefield.Score +
            _hand.Score;

        return IsMax ? score : -score;
      }
    }

    public int ConvertedMana { get { return _manaSources.GetMaxConvertedMana(); } }

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
        calc.Calculate(_hand)
        );
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

    public void AddManaToManaPool(IManaAmount manaAmount)
    {
      _manaPool.Add(manaAmount);
    }

    public void AssignDamage(Damage damage)
    {
      _assignedDamage.Assign(damage);
    }

    public void Consume(IManaAmount amount, IManaSource tryNotToConsumeThisSource = null)
    {
      _manaSources.Consume(amount, tryNotToConsumeThisSource);
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
      _manaPool.Empty();
    }


    public IEnumerable<ITarget> GetTargets()
    {
      yield return this;

      foreach (var card in Battlefield)
      {
        yield return card;
      }

      foreach (var card in Hand)
      {
        yield return card;
      }

      foreach (var card in Graveyard)
      {
        yield return card;
      }
    }

    public bool HasMana(int amount)
    {
      return _manaSources.GetMaxConvertedMana() >= amount;
    }

    public bool HasMana(IManaAmount amount)
    {
      if (amount == null)
        return true;

      return _manaSources.Has(amount);
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

        if (creature.HasLeathalDamage || creature.CalculateLifepointsLeft() <= 0)
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

    public void SetAiVisibility(IPlayer playerOnTheMove)
    {
      _library.Hide();
      _battlefield.Show();
      _graveyard.Show();

      if (playerOnTheMove == this)
      {
        _hand.Show();
        return;
      }

      _hand.Hide();
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

      foreach (var card in Hand)
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

    public void AddManaSources(IEnumerable<IManaSource> manaSources)
    {
      _manaSources.Add(manaSources);
    }

    public override string ToString()
    {
      return Name;
    }

    private void InitializeManaSources()
    {
      var manaSources =
        _manaPool.ToEnumerable().Concat(
          _library
            .Where(x => x.IsManaSource)
            .SelectMany(x => x.ManaSources));


      _manaSources = new ManaSources(manaSources);
    }

    private void Publish<T>(T message)
    {
      _publisher.Publish(message);
    }

    private void CreateZones(IEnumerable<Card> cards, Game game)
    {
      _battlefield = Bindable.Create<Battlefield>(game);
      _hand = Bindable.Create<Hand>(game);
      _graveyard = Bindable.Create<Graveyard>(game);
      _library = Bindable.Create<Library>(game);
      _exile = Bindable.Create<Exile>(game);

      foreach (var card in cards)
      {
        _library.Add(card);
      }
    }

    public interface IFactory
    {
      Player Create(string name, string avatar, PlayerType type, string deck);
    }
  }
}