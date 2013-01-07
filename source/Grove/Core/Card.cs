namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Cards;
  using Cards.Counters;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Infrastructure;
  using Mana;
  using Messages;
  using Targeting;
  using Zones;

  [Copyable]
  public class Card : ITarget, IDamageable, IHashDependancy, IAcceptsModifiers, IHasColors, IHasLife
  {
    private readonly ActivatedAbilities _activatedAbilities;
    private readonly Trackable<Card> _attachedTo;
    private readonly Attachments _attachments;
    private readonly Trackable<bool> _canRegenerate;
    private readonly CastInstructions _castInstructions;
    private readonly CardColors _colors;
    private readonly ContiniousEffects _continuousEffects;
    private readonly ControllerCharacteristic _controller;
    private readonly Counters _counters;
    private readonly Trackable<int> _damage;
    private readonly DamagePreventions _damagePreventions;
    private readonly Game _game;
    private readonly Trackable<bool> _hasLeathalDamage;
    private readonly Trackable<bool> _hasSummoningSickness;
    private readonly Trackable<int?> _hash;
    private readonly Trackable<bool> _isHidden;
    private readonly bool _isPreview;
    private readonly Trackable<bool> _isRevealed;
    private readonly Trackable<bool> _isTapped;
    private readonly Level _level;
    private readonly TrackableList<IModifier> _modifiers;
    private readonly Power _power;
    private readonly Protections _protections;
    private readonly StaticAbilities _staticAbilities;
    private readonly Toughness _toughness;
    private readonly TriggeredAbilities _triggeredAbilities;
    private readonly CardTypeCharacteristic _type;
    private readonly Trackable<int> _usageScore;
    private readonly Trackable<IZone> _zone;

    protected Card() {}

    public Card(CardParameters p)
    {
      Name = p.Name;
      ManaCost = p.ManaCost;
      HasXInCost = p.HasXInCost;

      Text = p.Text;
      FlavorText = p.FlavorText;
      Illustration = p.Illustration;

      _power = new Power(p.Power, null, null);
      _toughness = new Toughness(p.Toughness, null, null);
      _type = new CardTypeCharacteristic(p.Type, null, null);
      _colors = new CardColors(GetCardColorFromManaCost(), null, null);
      _level = new Level(null, null, null);
      _counters = new Counters(_power, _toughness, null, null);

      _damage = new Trackable<int>(null);
      _isTapped = new Trackable<bool>(null);

      _isPreview = true;
    }

    public Card(Player owner, Game game, CardParameters p)
    {
      _game = game;

      Owner = owner;
      Name = p.Name;
      ManaCost = p.ManaCost;
      HasXInCost = p.HasXInCost;
      MayChooseNotToUntap = p.MayChooseNotToUntap;
      OverrideScore = p.OverrideScore;
      Text = p.Text;
      FlavorText = p.FlavorText;
      Illustration = p.Illustration;

      _power = new Power(p.Power, game.ChangeTracker, this);
      _toughness = new Toughness(p.Toughness, game.ChangeTracker, this);
      _level = new Level(p.Isleveler ? 0 : (int?) null, game.ChangeTracker, this);
      _counters = new Counters(_power, _toughness, game.ChangeTracker, this);
      _type = new CardTypeCharacteristic(p.Type, game.ChangeTracker, this);
      _hash = new Trackable<int?>(game.ChangeTracker);

      _colors = new CardColors(
        p.Colors == ManaColors.None
          ? GetCardColorFromManaCost()
          : p.Colors,
        game.ChangeTracker, this);

      _isHidden = new Trackable<bool>(game.ChangeTracker, this);
      _isRevealed = new Trackable<bool>(game.ChangeTracker, this);

      _damage = new Trackable<int>(game.ChangeTracker, this);
      _usageScore = new Trackable<int>(game.ChangeTracker, this);
      _isTapped = new Trackable<bool>(game.ChangeTracker, this);
      _hasLeathalDamage = new Trackable<bool>(game.ChangeTracker, this);
      _attachedTo = new Trackable<Card>(game.ChangeTracker, this);
      _attachments = new Attachments(game.ChangeTracker);
      _canRegenerate = new Trackable<bool>(game.ChangeTracker, this);
      _hasSummoningSickness = new Trackable<bool>(true, game.ChangeTracker, this);
      _controller = new ControllerCharacteristic(owner, this, game, this);
      _protections = new Protections(game.ChangeTracker, this, p.ProtectionsFromColors, p.ProtectionsFromCardTypes);
      _zone = new Trackable<IZone>(new NullZone(), game.ChangeTracker, this);
      _modifiers = new TrackableList<IModifier>(game.ChangeTracker);

      _damagePreventions = new DamagePreventions(
        p.DamagePrevention.Select(x => x.Create(this, game)),
        game.ChangeTracker, this);

      _staticAbilities = new StaticAbilities(p.StaticAbilities, game.ChangeTracker, this);
      var triggeredAbilities =
        p.TriggeredAbilities.Select(x => x.Create(this, this, game));
      _triggeredAbilities = new TriggeredAbilities(triggeredAbilities, game.ChangeTracker, this);

      var activatedAbilities = p.ActivatedAbilities.Select(x => x.Create(this, game)).ToList();
      _activatedAbilities = new ActivatedAbilities(activatedAbilities, game.ChangeTracker, this);

      var castInstructions = p.CastInstructions.Select(x => new CastInstruction(this, _game, x)).ToList();
      _castInstructions = new CastInstructions(castInstructions);

      var continiousEffects =
        p.ContinuousEffects.Select(factory => factory.Create(this, this, game)).ToList();

      _continuousEffects = new ContiniousEffects(continiousEffects, game.ChangeTracker, this);
    }

    public bool MayChooseNotToUntap { get; private set; }
    public Card AttachedTo { get { return _attachedTo.Value; } private set { _attachedTo.Value = value; } }
    public IEnumerable<Card> Attachments { get { return _attachments.Cards; } }

    public bool CanAttackThisTurn
    {
      get
      {
        return
          !IsTapped &&
            (!HasSummoningSickness || Has().Haste) &&
              IsAbleToAttack;
      }
    }

    public bool IsAbleToAttack
    {
      get
      {
        return IsPermanent &&
          Is().Creature &&
            !Has().Defender &&
              !Has().CannotAttack;
      }
    }

    private int UsageScore { get { return _usageScore.Value; } set { _usageScore.Value = value; } }
    public bool HasFirstStrike { get { return Has().FirstStrike || Has().DoubleStrike; } }
    public bool HasNormalStrike { get { return !Has().FirstStrike || Has().DoubleStrike; } }
    public bool CanBeTapped { get { return IsPermanent && !IsTapped; } }
    public bool CanRegenerate { get { return _canRegenerate.Value; } set { _canRegenerate.Value = value; } }
    public bool CanTap { get { return (!Is().Creature || !HasSummoningSickness || Has().Haste) && !IsTapped; } }
    public bool IsPermanent { get { return Zone == Zone.Battlefield; } }
    public int CharacterCount { get { return FlavorText.CharacterCount + Text.CharacterCount; } }
    public int ChargeCountersCount { get { return _counters.SpecifiCount<ChargeCounter>(); } }
    public ManaColors Colors { get { return _colors.Value; } }
    public Player Controller { get { return _controller.Value; } }
    public Player Owner { get; private set; }
    public int? Counters { get { return _counters.Count; } }
    public int Damage { get { return _damage.Value; } protected set { _damage.Value = value; } }
    public CardText FlavorText { get; private set; }
    public bool HasAttachments { get { return _attachments.Count > 0; } }
    public bool HasLeathalDamage { get { return _hasLeathalDamage.Value; } }
    public bool HasSummoningSickness { get { return _hasSummoningSickness.Value; } set { _hasSummoningSickness.Value = value; } }
    public bool HasXInCost { get; private set; }
    public string Illustration { get; private set; }
    public bool IsAttached { get { return AttachedTo != null; } }
    public bool IsAttacker { get { return _game.Combat.IsAttacker(this); } }
    public bool IsBlocker { get { return _game.Combat.IsBlocker(this); } }
    public bool IsManaSource { get { return _activatedAbilities.ManaSources.Count > 0; } }
    public bool IsTapped { get { return _isTapped.Value; } protected set { _isTapped.Value = value; } }

    public IManaAmount ManaCost { get; private set; }
    public IEnumerable<IManaSource> ManaSources { get { return _activatedAbilities.ManaSources; } }

    public int ConvertedCost { get { return ManaCost == null ? 0 : ManaCost.Converted; } }

    private IEnumerable<IModifiable> ModifiableProperties
    {
      get
      {
        yield return _power;
        yield return _toughness;
        yield return _level;
        yield return _counters;
        yield return _colors;
        yield return _type;
        yield return _damagePreventions;
        yield return _protections;
        yield return _triggeredAbilities;
        yield return _activatedAbilities;
        yield return _staticAbilities;
        yield return _controller;
        yield return _continuousEffects;
      }
    }

    public string Name { get; private set; }
    public int? Power { get { return _power.Value < 0 ? 0 : _power.Value; } }

    public int Score
    {
      get
      {
        var score = 0;

        switch (Zone)
        {
          case (Zone.Battlefield):
            score = ScoreCalculator.CalculatePermanentScore(this);
            break;

          case (Zone.Hand):
            score = ScoreCalculator.CalculateCardInHandScore(this);
            break;

          case (Zone.Graveyard):
            score = ScoreCalculator.CalculateCardInGraveyardScore(this);
            break;

          case (Zone.Library):
            score = ScoreCalculator.CalculateCardInLibraryScore(this);
            break;
        }

        // card usage lowers the score slightly, since we want't to 
        // avoid activations that do no good
        return score - UsageScore;
      }
    }

    public CardText Text { get; private set; }

    public int? Toughness { get { return _toughness.Value; } }
    public string Type { get { return _type.Value.ToString(); } }
    public Zone Zone { get { return _zone.Value.Zone; } }

    public int? Level { get { return _level.Value; } }
    public bool CanBeDestroyed { get { return !CanRegenerate && !Has().Indestructible; } }
    public int? OverrideScore { get; private set; }
    public bool IsVisibleInUi { get { return _isPreview || IsVisibleToPlayer(_game.Players.Human); } }
    public bool IsVisible { get { return _game.Search.InProgress ? IsVisibleToPlayer(_game.Players.Searching) : IsVisibleToPlayer(Controller); } }

    public void AddModifier(IModifier modifier)
    {
      foreach (var modifiable in ModifiableProperties)
      {
        modifiable.Accept(modifier);
      }
      _modifiers.Add(modifier);
      modifier.Activate();

      Publish(new PermanentWasModified
        {
          Card = this,
          Modifier = modifier
        });
    }

    public void RemoveModifier(IModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();

      Publish(new PermanentWasModified {Card = this, Modifier = modifier});
    }

    public void DealDamage(Damage damage)
    {
      if (!Is().Creature || !IsPermanent)
        return;

      if (HasProtectionFrom(damage.Source))
      {
        damage.PreventAll();
        return;
      }

      _damagePreventions.PreventReceivedDamage(damage);

      if (damage.Amount == 0)
        return;

      Damage += damage.Amount;

      if (Damage >= Toughness || damage.IsLeathal)
      {
        _hasLeathalDamage.Value = true;
      }

      if (damage.Source.Has().Lifelink)
      {
        var controller = damage.Source.Controller;
        controller.Life += damage.Amount;
      }

      Publish(new DamageHasBeenDealt(this, damage));
    }

    public bool HasColors(ManaColors colors)
    {
      return (Colors & colors) == colors;
    }

    public void InvalidateHash()
    {
      _hash.Value = null;
    }

    public int Life
    {
      get
      {
        if (!Toughness.HasValue)
          return 0;

        return Toughness.Value - Damage;
      }
    }

    public int CalculateHash(HashCalculator calc)
    {
      if (_hash.Value.HasValue == false)
      {
        if (_isHidden)
        {
          _hash.Value = Zone.GetHashCode();
        }
        else
        {
          _hash.Value = HashCalculator.Combine(
            Name.GetHashCode(),
            _hasSummoningSickness.Value.GetHashCode(),
            UsageScore.GetHashCode(),
            IsTapped.GetHashCode(),
            Damage,
            CanRegenerate.GetHashCode(),
            HasLeathalDamage.GetHashCode(),
            Zone.GetHashCode(),
            Power.GetHashCode(),
            Toughness.GetHashCode(),
            Level.GetHashCode(),
            Colors.GetHashCode(),
            Counters.GetHashCode(),
            Type.GetHashCode(),
            _isRevealed.Value.GetHashCode(),
            calc.Calculate(_staticAbilities),
            calc.Calculate(_triggeredAbilities),
            calc.Calculate(_activatedAbilities),
            calc.Calculate(_protections),
            calc.Calculate(_damagePreventions),
            calc.Calculate(_attachments)
            );
        }
      }

      return _hash.Value.GetValueOrDefault();
    }

    public int EvaluateReceivedDamage(Card damageSource, int amount, bool isCombat)
    {
      if (HasProtectionFrom(damageSource))
      {
        return 0;
      }

      return _damagePreventions.EvaluateReceivedDamage(damageSource, amount, isCombat);
    }

    public void ActivateAbility(int index, ActivationParameters activationParameters)
    {
      _activatedAbilities.Activate(index, activationParameters);
      IncreaseUsageScore();
    }

    public void IncreaseUsageScore()
    {
      // to avoid useless moves every move lowers the score a bit
      // this factor increases linearily with elapsed turns
      // AI will prefer playing spells as soon as possible
      UsageScore += _game.Turn.TurnCount;
    }

    public void Attach(Card attachment)
    {
      if (attachment.IsAttached)
      {
        var controller = attachment.AttachedTo.Controller;

        attachment.AttachedTo.Detach(attachment);

        if (controller != Controller)
        {
          Controller.PutCardToBattlefield(attachment);
        }
      }

      attachment.AttachedTo = this;
      _attachments.Add(new Attachment(attachment));
      Publish(new AttachmentAttached {Attachment = attachment});
    }

    public List<SpellPrerequisites> CanActivateAbilities(bool ignoreManaAbilities = false)
    {
      return _activatedAbilities.CanActivate(ignoreManaAbilities);
    }

    public SpellPrerequisites CanActivateAbility(int abilityIndex)
    {
      return _activatedAbilities.CanActivate(abilityIndex);
    }

    public bool CanBeBlockedBy(Card card)
    {
      if (card.IsTapped)
        return false;

      if (Has().Unblockable)
        return false;

      if (Has().Flying && !card.Has().Flying && !card.Has().Reach)
        return false;

      if (Has().Fear && !card.HasColors(ManaColors.Black) && !card.Is().Artifact)
        return false;

      if (HasProtectionFrom(card))
        return false;

      if (Has().Swampwalk &&
        card.Controller.Battlefield.Any(x => x.Is("swamp")))
      {
        return false;
      }

      if (Has().Islandwalk &&
        card.Controller.Battlefield.Any(x => x.Is("island")))
      {
        return false;
      }

      if (Has().Mountainwalk &&
        card.Controller.Battlefield.Any(x => x.Is("mountain")))
      {
        return false;
      }

      return true;
    }

    public bool CanBeTargetBySpellsOwnedBy(Player player)
    {
      return !Has().Shroud && (player == Controller ? true : !Has().Hexproof);
    }

    public bool CanBlock()
    {
      return IsPermanent && !IsTapped && Is().Creature && !Has().CannotBlock;
    }

    public bool CanTarget(ITarget target)
    {
      return _castInstructions.CanTarget(target);
    }

    public List<SpellPrerequisites> CanCast()
    {
      return _castInstructions.CanCast();
    }

    public void Cast(int index, ActivationParameters activationParameters)
    {
      _castInstructions.Cast(index, activationParameters);
      IncreaseUsageScore();
    }

    public void ClearDamage()
    {
      Damage = 0;
      _hasLeathalDamage.Value = false;
    }

    public void Detach(Card card)
    {
      var attachment = _attachments[card];

      _attachments.Remove(attachment);
      card.AttachedTo = null;

      Publish(new AttachmentDetached
        {
          AttachedTo = this,
          Attachment = card
        });
    }

    public void EnchantWith(Card enchantment)
    {
      enchantment._castInstructions.EnchantTarget(this);
    }

    public void EquipWith(Card equipment)
    {
      equipment._activatedAbilities.EquipTarget(this);
    }

    public IManaAmount GetActivatedAbilityManaCost(int index)
    {
      return _activatedAbilities.GetManaCost(index);
    }

    public IStaticAbilities Has()
    {
      return _staticAbilities;
    }

    public bool HasAttachment(Card card)
    {
      return _attachments.Contains(card);
    }

    public bool HasProtectionFrom(ManaColors colors)
    {
      return _protections.HasProtectionFrom(colors);
    }

    public bool HasProtectionFrom(Card card)
    {
      return _protections.HasProtectionFrom(card.Colors) ||
        _protections.HasProtectionFrom(card._type.Value);
    }

    public void Hide()
    {
      _isHidden.Value = true;
      _isRevealed.Value = false;
    }

    public ITargetType Is()
    {
      return _type.Value;
    }

    public bool Is(string type)
    {
      return _type.Value.Is(type);
    }

    public void Regenerate()
    {
      Tap();
      ClearDamage();
      CanRegenerate = false;
      _game.Combat.Remove(this);
    }

    public void RemoveChargeCounter()
    {
      _counters.RemoveAny<ChargeCounter>();
    }

    public void Destroy(bool allowToRegenerate = true)
    {
      if (Has().Indestructible)
      {
        return;
      }

      if (CanRegenerate && allowToRegenerate)
      {
        Regenerate();
        return;
      }

      Owner.PutCardToGraveyard(this);
    }


    public void Sacrifice()
    {
      Owner.PutCardToGraveyard(this);
    }

    public void ChangeZone(IZone newZone)
    {
      var oldZone = _zone.Value;
      _zone.Value = newZone;

      oldZone.Remove(this);

      if (oldZone.Zone != newZone.Zone)
      {
        Publish(new CardChangedZone
          {
            Card = this,
            From = oldZone.Zone,
            To = newZone.Zone
          });

        oldZone.AfterRemove(this);
      }

      newZone.AfterAdd(this);
    }

    public void OnCardLeftBattlefield()
    {
      _game.Combat.Remove(this);

      DetachAttachments();
      Detach();
      Untap();
      ClearDamage();

      _continuousEffects.Deactivate();
    }

    public void Tap()
    {
      IsTapped = true;
    }

    public override string ToString()
    {
      return Name;
    }

    public void Untap()
    {
      IsTapped = false;
      Publish(new PermanentGetsUntapped {Permanent = this});
    }

    public void DetachAttachments()
    {
      foreach (var attachedCard in _attachments.Cards.ToList())
      {
        if (attachedCard.Is().Aura)
        {
          // auras are sacrificed        
          attachedCard.Sacrifice();
        }
        else
        {
          Detach(attachedCard);
        }
      }
    }

    public void Detach()
    {
      if (IsAttached)
      {
        AttachedTo.Detach(this);
      }
    }

    private ManaColors GetCardColorFromManaCost()
    {
      if (ManaCost == null)
        return ManaColors.None;

      if (ManaCost.Count() == 0)
      {
        return ManaColors.Colorless;
      }

      if (ManaCost.None(x => x.IsColored))
      {
        return ManaColors.Colorless;
      }

      var cardColor = ManaColors.None;

      foreach (var mana in ManaCost.Colored())
      {
        cardColor = cardColor | mana.Colors;
      }

      return cardColor;
    }

    private void Publish<T>(T message)
    {
      _game.Publish(message);
    }

    public void Exile()
    {
      Owner.PutCardToExile(this);
    }

    public void RemoveModifier(Type type)
    {
      var modifier = _modifiers.FirstOrDefault(x => x.GetType() == type);

      if (modifier == null)
        return;

      RemoveModifier(modifier);
    }

    public bool IsVisibleToPlayer(Player player)
    {
      if (_isRevealed == true)
        return true;

      if (_isHidden == true)
        return false;

      if (Zone == Zone.Battlefield || Zone == Zone.Graveyard || Zone == Zone.Exile || Zone == Zone.Stack)
        return true;

      if (Zone == Zone.Library)
        return false;

      return player == Controller;
    }

    public void Reveal()
    {
      _isRevealed.Value = true;
      _isHidden.Value = false;
    }

    public void ResetVisibility()
    {
      _isRevealed.Value = false;
      _isHidden.Value = false;
    }

    public int CalculateCombatDamage(bool allDamageSteps = false, int powerIncrease = 0)
    {
      if (!Power.HasValue)
        return 0;

      var amount = Power.Value + powerIncrease;
      amount = _damagePreventions.PreventDealtCombatDamage(amount);

      if (allDamageSteps)
      {
        amount = Has().DoubleStrike ? amount*2 : amount;
      }

      return amount;
    }

    public void PutToHand()
    {
      Owner.PutCardToHand(this);
    }

    public void PutOnTopOfLibrary()
    {
      Owner.PutCardOnTopOfLibrary(this);
    }

    public void Discard()
    {
      Owner.DiscardCard(this);
    }

    public void ShuffleIntoLibrary()
    {
      Owner.ShuffleIntoLibrary(this);
    }

    public void PutToBattlefield()
    {
      Controller.PutCardToBattlefield(this);
    }

    public void OnCardJoinedBattlefield()
    {
      HasSummoningSickness = true;
      _continuousEffects.Activate();
    }

    public void PutToGraveyard()
    {
      Owner.PutCardToGraveyard(this);
    }
  }
}