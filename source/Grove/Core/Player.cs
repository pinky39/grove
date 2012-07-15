namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Details.Cards;
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
  public class Player : ITarget, IDamageable
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
    private Battlefield _battlefield;
    private Graveyard _graveyard;
    private Hand _hand;
    private Library _library;
    private ManaSources _manaSources;

    public Player(
      string name,
      string avatar,
      bool isHuman,
      string deck,
      ChangeTracker changeTracker,
      Publisher publisher,
      DeckFactory deckFactory)
    {
      _publisher = publisher;

      Name = name;
      Avatar = avatar;
      IsHuman = isHuman;

      _life = new Life(20, changeTracker);
      _canPlayLands = new Trackable<bool>(true, changeTracker);
      _hasMulligan = new Trackable<bool>(true, changeTracker);
      _hasLost = new Trackable<bool>(changeTracker);
      _isActive = new Trackable<bool>(changeTracker);
      _hasPriority = new Trackable<bool>(changeTracker);
      _manaPool = Bindable.Create<ManaPool>(changeTracker);
      _modifiers = new TrackableList<IModifier>(changeTracker);
      _damagePreventions = new DamagePreventions(changeTracker, null);
      _damageRedirections = new DamageRedirections(changeTracker, null);
      _assignedDamage = new AssignedDamage(this, changeTracker);

      var cards = deckFactory.CreateDeck(deck, this);

      SetupZones(cards, changeTracker);
      InitializeManaSources();
    }

    private Player() {}

    public string Avatar { get; private set; }
    public IBattlefieldQuery Battlefield { get { return _battlefield; } }
    public bool CanMulligan { get { return _hand.CanMulligan && HasMulligan; } }
    public bool CanPlayLands { get { return _canPlayLands.Value; } set { _canPlayLands.Value = value; } }
    public IEnumerable<Card> Graveyard { get { return _graveyard; } }
    public IHandQuery Hand { get { return _hand; } }
    public bool HasLost { get { return _hasLost.Value; } set { _hasLost.Value = value; } }
    public bool HasManaInPool { get { return !_manaPool.IsEmpty; } }
    public bool HasMulligan { get { return _hasMulligan.Value; } set { _hasMulligan.Value = value; } }
    public bool HasPriority { get { return _hasPriority.Value; } set { _hasPriority.Value = value; } }
    public virtual bool IsActive { get { return _isActive.Value; } set { _isActive.Value = value; } }
    public bool IsHuman { get; private set; }
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

    public object ManaPool { get { return _manaPool; } }
    public string Name { get; private set; }

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

    private IEnumerable<IModifiable> ModifiableProperties
    {
      get
      {
        yield return _damagePreventions;
        yield return _damageRedirections;
      }
    }

    public void DealDamage(Damage damage)
    {
      _damagePreventions.PreventDamage(damage);

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

    public void AddManaSources(IEnumerable<IManaSource> manaSources)
    {
      _manaSources.Add(manaSources);
    }

    public void AddManaToManaPool(IManaAmount manaAmount)
    {
      _manaPool.Add(manaAmount);
    }

    public void AssignDamage(Damage damage)
    {
      _assignedDamage.Assign(damage);
    }

    public void CycleCard(Card card)
    {
      _hand.Remove(card);      
      card.CycleInternal();
      Publish(new PlayerHasCycledCard(card));
    }

    public void CastSpell(Card spell, ActivationParameters activationParameters)
    {
      _hand.Remove(spell);     
      spell.CastInternal(activationParameters);      
    }

    public void Consume(IManaAmount amount, IManaSource tryNotToConsumeThisSource = null)
    {
      _manaSources.Consume(amount, tryNotToConsumeThisSource);
    }

    public void DealAssignedDamage()
    {
      _assignedDamage.Deal();
    }

    public void DestroyCard(Card card, bool allowRegenerate)
    {
      if (card.Has().Indestructible)
      {
        return;
      }

      if (card.CanRegenerate && allowRegenerate)
      {
        card.Regenerate();
        return;
      }

      PutCardToGraveyardFromPlay(card);
    }

    public void DiscardCard(Card card)
    {
      _hand.Remove(card);
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
      var card = _library.Draw();

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
          SacrificeCard(creature);
          continue;
        }

        if (creature.HasLeathalDamage || creature.LifepointsLeft <= 0)
        {
          DestroyCard(creature, allowRegenerate: true);
        }
      }
    }

    public void PutCardIntoPlay(Card card)
    {
      _battlefield.Add(card);
    }

    public void PutCardToGraveyard(Card card)
    {
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

    public void ReturnToHand(Card card)
    {
      switch (card.Zone)
      {
        case (Zone.Battlefield):
          PutCardToHandFromPlay(card);
          break;

        case (Zone.Graveyard):
          PutCardToHandFromGraveyard(card);
          break;
      }
    }

    public void SacrificeCard(Card card)
    {
      PutCardToGraveyardFromPlay(card);
    }

    public void SetAiVisibility(Player playerOnTheMove)
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
      _library.Shuffle(card.ToEnumerable());
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

      _library.Shuffle(_hand);
      _hand.Discard();

      for (var i = 0; i < mulliganSize; i++)
      {
        DrawCard();
      }

      HasMulligan = true;
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

    private void PutCardToGraveyardFromPlay(Card card)
    {
      _battlefield.Remove(card);

      if (card.Is().Token)
      {
        card.SetZone(Zone.Exiled);
        return;
      }

      _graveyard.Add(card);
    }

    private void PutCardToHandFromGraveyard(Card card)
    {
      _graveyard.Remove(card);
      _hand.Add(card);
    }

    private void PutCardToHandFromPlay(Card card)
    {
      _battlefield.Remove(card);

      if (card.Is().Token)
      {
        card.SetZone(Zone.Exiled);
        return;
      }

      _hand.Add(card);
    }

    private void SetupZones(IEnumerable<Card> cards, ChangeTracker changeTracker)
    {
      _battlefield = Bindable.Create<Battlefield>(changeTracker);
      _hand = Bindable.Create<Hand>(changeTracker);
      _graveyard = Bindable.Create<Graveyard>(changeTracker);
      _library = Bindable.Create<Library>(cards, changeTracker);
    }

    public void ExileCard(Card card)
    {
      _battlefield.Remove(card);
      card.SetZone(Zone.Exiled);
    }

    public void Mill(int count)
    {
      for (var i = 0; i < count; i++)
      {
        var card = _library.Draw();
        if (card == null)
          return;

        PutCardToGraveyard(card);
      }
    }

    public interface IFactory
    {
      Player Create(string name, string avatar, bool isHuman, string deck);
    }

    public void MoveCardFromGraveyardToHand(Card card)
    {
      _graveyard.Remove(card);
      _hand.Add(card);
    }
  }
}