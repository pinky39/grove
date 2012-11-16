namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Cards;
  using Cards.Casting;
  using Cards.Costs;
  using Cards.Counters;
  using Cards.Effects;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Cards.Triggers;
  using Dsl;
  using Infrastructure;
  using Mana;
  using Messages;
  using Targeting;
  using Zones;

  [Copyable]
  public class Card : IEffectSource, ITarget, IDamageable, IHashDependancy, IAcceptsModifiers, IHasColors, IHasLife
  {
    private static readonly Random Random = new Random();

    private ActivatedAbilities _activatedAbilities;
    private Cost _additionalCost;
    private Trackable<Card> _attachedTo;
    private Attachments _attachments;
    private Trackable<bool> _canRegenerate;
    private CardColors _colors;
    private ContiniousEffects _continuousEffects;
    private ControllerCharacteristic _controller;
    private Counters _counters;
    private Trackable<int> _damage;
    private DamagePreventions _damagePreventions;
    private bool _distributeDamage;
    private IEffectFactory _effectFactory;
    private Game _game;
    private Trackable<bool> _hasLeathalDamage;
    private Trackable<bool> _hasSummoningSickness;
    private Trackable<int?> _hash;
    private Trackable<bool> _isHidden;
    private Trackable<bool> _isRevealed;
    private Trackable<bool> _isTapped;
    private IEffectFactory _kickerEffectFactory;
    private TargetSelector _kickerTargetSelector;
    private Level _level;
    private TrackableList<IModifier> _modifiers;
    private Power _power;
    private Protections _protections;
    private Zone? _resolveToZone;
    private StaticAbilities _staticAbilities;
    private TargetSelector _targetSelector;
    private TimingDelegate _timming;
    private Toughness _toughness;
    private TriggeredAbilities _triggeredAbilities;
    private CardTypeCharacteristic _type;
    private Trackable<int> _usageScore;
    private CalculateX _xCalculator;
    private Trackable<IZone> _zone;

    protected Card() {}
    public bool MayChooseNotToUntapDuringUntapStep { get; private set; }
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
    public CastingRule CastingRule { get; private set; }
    public int CharacterCount { get { return FlavorText.CharacterCount + Text.CharacterCount; } }
    public int ChargeCountersCount { get { return _counters.SpecifiCount<ChargeCounter>(); } }
    public virtual ManaColors Colors { get { return _colors.Value; } }
    public Player Controller { get { return _controller.Value; } }
    public Player Owner { get; private set; }
    public int? Counters { get { return _counters.Count; } }
    public virtual int Damage { get { return _damage.Value; } protected set { _damage.Value = value; } }
    public CardText FlavorText { get; private set; }
    public bool HasAttachments { get { return _attachments.Count > 0; } }
    public bool HasKicker { get { return KickerCost != null; } }
    public bool HasLeathalDamage { get { return _hasLeathalDamage.Value; } }
    public bool HasSummoningSickness { get { return _hasSummoningSickness.Value; } set { _hasSummoningSickness.Value = value; } }
    public bool HasXInCost { get { return _xCalculator != null; } }
    public string Illustration { get; private set; }
    public bool IsAttached { get { return AttachedTo != null; } }
    public bool IsAttacker { get { return _game.Combat.IsAttacker(this); } }
    public bool IsBlocker { get { return _game.Combat.IsBlocker(this); } }
    public bool IsHidden { get { return _isHidden.Value; } private set { _isHidden.Value = value; } }
    public bool IsManaSource { get { return _activatedAbilities.ManaSources.Count > 0; } }
    public virtual bool IsTapped { get { return _isTapped.Value; } protected set { _isTapped.Value = value; } }
    public IManaAmount KickerCost { get; private set; }

    public IManaAmount ManaCost { get; private set; }
    public IManaAmount ManaCostWithKicker { get; private set; }
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
    public virtual bool IsRevealed { get { return _isRevealed.Value; } set { _isRevealed.Value = value; } }
    public int? OverrideScore { get; private set; }

    public virtual void AddModifier(IModifier modifier)
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

      this.Updates("Damage");
    }

    public EffectCategories EffectCategories { get; private set; }

    Card IEffectSource.OwningCard { get { return this; } }
    Card IEffectSource.SourceCard { get { return this; } }

    void IEffectSource.EffectWasCountered()
    {
      Owner.PutCardToGraveyard(this);
    }

    void IEffectSource.EffectWasPushedOnStack()
    {
      var oldZone = Zone;

      ChangeZone(_game.Stack);

      Publish(new CardChangedZone
        {
          Card = this,
          From = oldZone,
          To = Zone
        });
    }

    void IEffectSource.EffectWasResolved()
    {
      if (Is().Aura)
      {
        AttachedTo.Controller.PutCardToBattlefield(this);
        return;
      }

      if (Is().Instant || Is().Sorcery)
      {
        if (_resolveToZone.HasValue)
        {
          switch (_resolveToZone)
          {
            case (Zone.Library):
              Owner.ShuffleIntoLibrary(this);
              return;
            case (Zone.Hand):
              Owner.PutCardToHand(this);
              return;
          }
        }

        Owner.PutCardToGraveyard(this);
        return;
      }

      Controller.PutCardToBattlefield(this);
    }

    bool IEffectSource.IsTargetStillValid(ITarget target, bool wasKickerPaid)
    {
      return wasKickerPaid
        ? _kickerTargetSelector.IsValidEffectTarget(target)
        : _targetSelector.IsValidEffectTarget(target);
    }

    public int CalculateHash(HashCalculator calc)
    {
      if (_hash.Value.HasValue == false)
      {
        if (IsHidden)
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
            IsRevealed.GetHashCode(),
            calc.Calculate(_staticAbilities),
            calc.Calculate(_triggeredAbilities),
            calc.Calculate(_activatedAbilities),
            calc.Calculate(_protections),
            calc.Calculate(_damagePreventions),
            calc.Calculate(_attachments)
            );
        }
      }

      return _hash.Value.Value;
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
      return _targetSelector.Effect[0].IsValid(target);
    }

    public SpellPrerequisites CanCast()
    {
      if (!CastingRule.CanCast(this))
        return new SpellPrerequisites {CanBeSatisfied = false};

      var canCastWithKicker = HasKicker
        ? Owner.HasMana(ManaCostWithKicker, ManaUsage.Spells)
        : false;

      return new SpellPrerequisites
        {
          CanBeSatisfied = true,
          Description = String.Format("Cast {0}.", this),
          TargetSelector = _targetSelector,
          KickerTargetSelector = _kickerTargetSelector,
          CanCastWithKicker = canCastWithKicker,
          MaxX = GetMaxX(),
          DistributeDamage = _distributeDamage,
          XCalculator = _xCalculator,
          Timming = _timming,
        };
    }

    public void Cast(ActivationParameters activationParameters)
    {
      PayCastingCost(activationParameters);

      var parameters = new EffectParameters(
        source: this,
        activation: activationParameters,
        targets: activationParameters.Targets);

      var effect = activationParameters.PayKicker
        ? _kickerEffectFactory.CreateEffect(parameters, _game)
        : _effectFactory.CreateEffect(parameters, _game);


      CastingRule.Cast(effect);
      IncreaseUsageScore();

      Publish(new PlayerHasCastASpell(this, activationParameters.Targets));
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

    public void EnchantWithoutPayingTheCost(Card enchantment)
    {
      var effect = enchantment._effectFactory.CreateEffect(
        new EffectParameters(enchantment, targets: new Targets().AddEffect(this)),
        _game);

      if (effect is Attach == false)
        throw new InvalidOperationException("Card is is not an enchantment.");

      effect.Resolve();
      effect.FinishResolve();
    }

    public void EquipWithoutPayingTheCost(Card equipment)
    {
      var effect = equipment._activatedAbilities.CreateEffect<Attach>(
        target: this);

      if (effect == null)
        throw new InvalidOperationException("Card is is not an equipment.");

      effect.Resolve();
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
      if (IsRevealed)
        return;

      IsHidden = true;
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

    public void UnHide()
    {
      IsHidden = false;
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

    private int? GetMaxX()
    {
      int? maxX = null;
      if (HasXInCost)
      {
        maxX = Controller.GetConvertedMana(ManaUsage.Spells) - ManaCostWithKicker.Converted;
      }
      return maxX;
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

    public bool CanPayCastingCost()
    {
      return _additionalCost.CanPay() &&
        Controller.HasMana(ManaCost, ManaUsage.Spells);
    }

    public void PayCastingCost(ActivationParameters activationParameters)
    {
      if (Is().Land)
      {
        Owner.CanPlayLands = false;
      }
      else
      {
        var manaCost = activationParameters.PayKicker ? ManaCostWithKicker : ManaCost;
        if (activationParameters.X.HasValue)
        {
          manaCost = manaCost.Add(activationParameters.X.Value);
        }
        Controller.Consume(manaCost, ManaUsage.Spells);
      }

      _additionalCost.Pay(
        activationParameters.Targets.Cost.FirstOrDefault(),
        activationParameters.X);
    }

    public void Reveal()
    {
      IsRevealed = true;
      UnHide();
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

    public void ReturnToHand()
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

    public bool IsGoodTarget(ITarget target)
    {
      var generator = new TargetGenerator(
        _targetSelector,
        this,
        _game,
        0);

      return generator.Any(targets => targets.Effect.Contains(target));
    }

    public void OnCardJoinedBattlefield()
    {
      HasSummoningSickness = true;
      _continuousEffects.Activate();
    }    

    [Copyable]
    public class CardFactory : ICardFactory
    {
      private readonly List<IActivatedAbilityFactory> _activatedAbilityFactories = new List<IActivatedAbilityFactory>();
      private readonly List<IContinuousEffectFactory> _continuousEffectFactories = new List<IContinuousEffectFactory>();
      private readonly List<ITargetValidatorFactory> _costTargetFactories = new List<ITargetValidatorFactory>();
      private readonly List<ITargetValidatorFactory> _effectTargetFactories = new List<ITargetValidatorFactory>();
      private readonly List<ITargetValidatorFactory> _kickerEffectTargetFactories = new List<ITargetValidatorFactory>();
      private readonly List<Static> _staticAbilities = new List<Static>();

      private readonly List<ITriggeredAbilityFactory> _triggeredAbilityFactories = new List<ITriggeredAbilityFactory>();
      private ICostFactory _additionalCost;
      private TargetSelectorAiDelegate _aiTargetSelector;
      private ManaColors _colors;
      private bool _distributeSpellsDamage;
      private EffectCategories _effectCategories;
      private IEffectFactory _effectFactory;
      private string _flavorText;
      private bool _isleveler;
      private TargetSelectorAiDelegate _kickerAiTargetSelector;
      private string _kickerCost;
      private IEffectFactory _kickerEffectFactory;
      private string _manaCost;
      private bool _mayChooseNotToUntap;
      private string _name;
      private int? _overrideScore;
      private int? _power;
      private string[] _protectionsFromCardTypes;
      private ManaColors _protectionsFromColors = ManaColors.None;
      private Zone? _resolveZone;

      private string _text;
      private TimingDelegate _timing;
      private int? _toughness;
      private CardType _type;
      private CalculateX _xCalculator;

      public string Name { get { return _name; } }

      public Card CreateCard(Player owner, Game game)
      {
        var card = game.Search.InProgress ? new Card() : Bindable.Create<Card>();

        card._game = game;
        card.Owner = owner;

        card._effectFactory = _effectFactory ?? new Effect.Factory<PutIntoPlay>();
        card._kickerEffectFactory = _kickerEffectFactory;
        card._hash = new Trackable<int?>(game.ChangeTracker);
        card._distributeDamage = _distributeSpellsDamage;

        card.Name = _name;
        card._xCalculator = _xCalculator;
        card.ManaCost = _manaCost.ParseManaAmount();
        card.MayChooseNotToUntapDuringUntapStep = _mayChooseNotToUntap;
        card.OverrideScore = _overrideScore;
        card.KickerCost = _kickerCost.ParseManaAmount();
        card.ManaCostWithKicker = card.HasKicker ? card.ManaCost.Add(card.KickerCost) : card.ManaCost;
        card.Text = _text ?? String.Empty;
        card.FlavorText = _flavorText ?? String.Empty;
        card.Illustration = GetIllustration(_name, _type);
        card.CastingRule = CreateCastingRule(_type, game);

        CreateBindablePower(card, game.ChangeTracker);
        CreateBindableToughness(card, game.ChangeTracker);
        CreateBindableLevel(card, game.ChangeTracker);
        CreateBindableCounters(card, game.ChangeTracker);
        CreateBindableType(card, game.ChangeTracker);
        CreateBindableColors(card, game.ChangeTracker);

        card._damage = new Trackable<int>(game.ChangeTracker, card);
        card._usageScore = new Trackable<int>(game.ChangeTracker, card);
        card._isTapped = new Trackable<bool>(game.ChangeTracker, card);
        card._hasLeathalDamage = new Trackable<bool>(game.ChangeTracker, card);
        card._attachedTo = new Trackable<Card>(game.ChangeTracker, card);
        card._attachments = new Attachments(game.ChangeTracker);
        card._isHidden = new Trackable<bool>(game.ChangeTracker, card);
        card._isRevealed = new Trackable<bool>(game.ChangeTracker, card);
        card._canRegenerate = new Trackable<bool>(game.ChangeTracker, card);
        card._hasSummoningSickness = new Trackable<bool>(true, game.ChangeTracker, card);
        card._controller = new ControllerCharacteristic(owner, card, game, card);
        card._damagePreventions = new DamagePreventions(game.ChangeTracker, card);
        card._protections = new Protections(game.ChangeTracker, card, _protectionsFromColors, _protectionsFromCardTypes);
        card._zone = new Trackable<IZone>(new NullZone(), game.ChangeTracker, card);


        card.EffectCategories = _effectCategories;
        card._timming = _timing ?? Timings.NoRestrictions();

        card._modifiers = new TrackableList<IModifier>(game.ChangeTracker);

        card._targetSelector = new TargetSelector(
          effectValidators: _effectTargetFactories.Select(x => x.Create(card, game)),
          costValidators: _costTargetFactories.Select(x => x.Create(card, game)),
          aiSelector: _aiTargetSelector
          );

        card._kickerTargetSelector = new TargetSelector(
          effectValidators: _kickerEffectTargetFactories.Select(x => x.Create(card, game)),
          costValidators: _costTargetFactories.Select(x => x.Create(card, game)),
          aiSelector: _kickerAiTargetSelector
          );

        card._additionalCost = _additionalCost == null
          ? new NoCost()
          : _additionalCost.CreateCost(card, card._targetSelector.Cost.FirstOrDefault(), game);

        card._staticAbilities = new StaticAbilities(_staticAbilities, game.ChangeTracker, card);

        var triggeredAbilities = _triggeredAbilityFactories.Select(x => x.Create(card, card, game));
        card._triggeredAbilities = new TriggeredAbilities(triggeredAbilities, game.ChangeTracker, card);

        var activatedAbilities = _activatedAbilityFactories.Select(x => x.Create(card, game)).ToList();

        card._activatedAbilities = new ActivatedAbilities(activatedAbilities, game.ChangeTracker, card);

        var continiousEffects = _continuousEffectFactories.Select(factory => factory.Create(card, card, game)).ToList();
        card._continuousEffects = new ContiniousEffects(continiousEffects, game.ChangeTracker, card);

        card._resolveToZone = _resolveZone;

        return card;
      }

      public Card CreateCardPreview()
      {
        var card = Bindable.Create<Card>();

        card.Name = _name;
        card.ManaCost = _manaCost.ParseManaAmount();
        card.Text = _text ?? String.Empty;
        card.FlavorText = _flavorText ?? String.Empty;
        card.Illustration = GetIllustration(_name, _type);
        card._xCalculator = _xCalculator;

        card._power = new Power(_power, null, null);
        card._toughness = new Toughness(_toughness, null, null);
        card._type = new CardTypeCharacteristic(_type, null, null);
        card._colors = new CardColors(card.GetCardColorFromManaCost(), null, null);
        card._level = new Level(null, null, null);
        card._counters = new Counters(card._power, card._toughness, null, null);

        card._damage = new Trackable<int>(null, card);
        card._isTapped = new Trackable<bool>(null, card);

        return card;
      }

      public CardFactory Protections(ManaColors colors)
      {
        _protectionsFromColors = colors;
        return this;
      }

      public CardFactory Protections(params string[] cardTypes)
      {
        _protectionsFromCardTypes = cardTypes;
        return this;
      }

      public CardFactory Echo(string manaCost)
      {
        var c = new CardBuilder();
        var amount = manaCost.ParseManaAmount();

        var echoFactory = c.TriggeredAbility(
          "At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.",
          c.Trigger<AtBegginingOfStep>(t =>
            {
              t.Step = Step.Upkeep;
              t.OnlyOnceWhenAfterItComesUnderYourControl = true;
            }),
          c.Effect<PayManaOrSacrifice>(e =>
            {
              e.Amount = amount;
              e.Message = String.Format("Pay {0}'s echo?", e.Source.OwningCard);
            }),
          triggerOnlyIfOwningCardIsInPlay: true);

        _triggeredAbilityFactories.Add(echoFactory);
        return this;
      }

      public CardFactory Abilities(params object[] abilities)
      {
        foreach (var ability in abilities)
        {
          if (ability is Static)
          {
            _staticAbilities.Add((Static) ability);
            continue;
          }

          if (ability is IActivatedAbilityFactory)
          {
            _activatedAbilityFactories.Add(ability as IActivatedAbilityFactory);
            continue;
          }

          if (ability is ITriggeredAbilityFactory)
          {
            _triggeredAbilityFactories.Add(ability as ITriggeredAbilityFactory);
            continue;
          }

          if (ability is IContinuousEffectFactory)
          {
            _continuousEffectFactories.Add((IContinuousEffectFactory) ability);
          }
        }
        return this;
      }

      public CardFactory AfterResolvePutToZone(Zone zone)
      {
        _resolveZone = zone;
        return this;
      }

      public CardFactory Category(EffectCategories effectCategories)
      {
        _effectCategories = effectCategories;
        return this;
      }

      public CardFactory Colors(ManaColors colors)
      {
        _colors = colors;
        return this;
      }

      public CardFactory IsLeveler()
      {
        _isleveler = true;
        return this;
      }

      public CardFactory Effect<T>(Action<T> init = null) where T : Effect, new()
      {
        init = init ?? delegate { };

        _effectFactory = new Effect.Factory<T>
          {
            Init = p => init(p.Effect)
          };

        return this;
      }

      public CardFactory Effect<T>(EffectInitializer<T> init) where T : Effect, new()
      {
        init = init ?? delegate { };

        _effectFactory = new Effect.Factory<T>
          {
            Init = init
          };

        return this;
      }

      public CardFactory DistributeSpellsDamage()
      {
        _distributeSpellsDamage = true;
        return this;
      }

      public CardFactory FlavorText(string flavorText)
      {
        _flavorText = flavorText;
        return this;
      }

      public CardFactory KickerCost(string kickerCost)
      {
        _kickerCost = kickerCost;
        return this;
      }

      public CardFactory KickerEffect<T>(Action<T> init = null) where T : Effect, new()
      {
        init = init ?? delegate { };

        _kickerEffectFactory = new Effect.Factory<T>
          {
            Init = p => init(p.Effect)
          };

        return this;
      }

      public CardFactory KickerEffect<T>(EffectInitializer<T> init) where T : Effect, new()
      {
        init = init ?? delegate { };

        _kickerEffectFactory = new Effect.Factory<T>
          {
            Init = init,
          };

        return this;
      }

      public CardFactory ManaCost(string manaCost)
      {
        _manaCost = manaCost;
        return this;
      }

      public CardFactory AdditionalCost<T>(Initializer<T> init = null) where T : Cost, new()
      {
        init = init ?? delegate { };

        _additionalCost = new Cost.Factory<T>
          {
            Init = init,
          };

        return this;
      }

      public CardFactory Named(string name)
      {
        _name = name;
        return this;
      }

      public CardFactory Cycling(string cost)
      {
        var b = new CardBuilder();

        var cycling = b.ActivatedAbility(
          string.Format("Cycling {0} ({0}, Discard this card: Draw a card.)", cost),
          b.Cost<DiscardOwnerPayMana>(c => c.Amount = cost.ParseManaAmount()),
          b.Effect<DrawCards>(e => e.DrawCount = 1),
          timing: Timings.Cycling(),
          activationZone: Zone.Hand);

        _activatedAbilityFactories.Add(cycling);

        return this;
      }

      public CardFactory Power(int power)
      {
        _power = power;
        return this;
      }

      public CardFactory KickerTargets(TargetSelectorAiDelegate aiTargetSelector,
        params ITargetValidatorFactory[] effectValidators)
      {
        _kickerEffectTargetFactories.AddRange(effectValidators);
        _kickerAiTargetSelector = aiTargetSelector;
        return this;
      }

      public CardFactory Targets(TargetSelectorAiDelegate selectorAi,
        params ITargetValidatorFactory[] effectValidators)
      {
        _effectTargetFactories.AddRange(effectValidators);
        _aiTargetSelector = selectorAi;
        return this;
      }

      public CardFactory Targets(TargetSelectorAiDelegate selectorAi,
        ITargetValidatorFactory effectValidator = null,
        ITargetValidatorFactory costValidator = null)
      {
        if (effectValidator != null)
          _effectTargetFactories.Add(effectValidator);

        if (costValidator != null)
          _costTargetFactories.Add(costValidator);

        _aiTargetSelector = selectorAi;
        return this;
      }

      public CardFactory Text(string text)
      {
        _text = text;
        return this;
      }

      public CardFactory Timing(TimingDelegate timing)
      {
        _timing = timing;
        return this;
      }

      public CardFactory Toughness(int toughness)
      {
        _toughness = toughness;
        return this;
      }

      public CardFactory Type(string type)
      {
        _type = type;
        return this;
      }

      public CardFactory XCalculator(CalculateX calculateX)
      {
        _xCalculator = calculateX;
        return this;
      }

      public CardFactory Leveler(IManaAmount cost, EffectCategories category = EffectCategories.Generic,
        params LevelDefinition[] levels)
      {
        var abilities = new List<object>();
        var builder = new CardBuilder();

        abilities.Add(
          builder.ActivatedAbility(
            String.Format("{0}: Put a level counter on this. Level up only as sorcery.", cost),
            builder.Cost<TapOwnerPayMana>(c => c.Amount = cost),
            builder.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(builder.Modifier<IncreaseLevel>())),
            timing: Timings.Leveler(cost, levels), activateAsSorcery: true, category: category));


        foreach (var levelDefinition in levels)
        {
          var definition = levelDefinition;

          abilities.Add(
            builder.StaticAbility(
              builder.Trigger<OnLevelChanged>(c => c.Level = definition.Min),
              builder.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
                builder.Modifier<AddStaticAbility>(m => m.StaticAbility = definition.StaticAbility,
                  minLevel: definition.Min, maxLevel: definition.Max),
                builder.Modifier<SetPowerAndToughness>(m =>
                  {
                    m.Power = definition.Power;
                    m.Tougness = definition.Thoughness;
                  }, minLevel: definition.Min, maxLevel: definition.Max))))
            );
        }

        IsLeveler().Abilities(abilities.ToArray());

        return this;
      }

      private static CastingRule CreateCastingRule(CardType type, Game game)
      {
        if (type.Instant)
          return new Instant(game.Stack);

        if (type.Land)
          return new Land(game.Stack, game.Turn);

        return new Default(game.Stack, game.Turn);
      }

      private static string GetIllustration(string cardName, CardType cardType)
      {
        const int basicLandVersions = 4;

        if (cardType.BasicLand)
        {
          return cardName + Random.Next(1, basicLandVersions + 1);
        }

        return cardName;
      }

      private void CreateBindableColors(Card card, ChangeTracker changeTracker)
      {
        var cardColors = _colors == ManaColors.None ? card.GetCardColorFromManaCost() : _colors;

        card._colors = Bindable.Create<CardColors>(
          cardColors,
          changeTracker,
          card);

        card._colors.Property(x => x.Value)
          .Changes(card).Property<Card, ManaColors>(x => x.Colors);
      }

      private void CreateBindableCounters(Card card, ChangeTracker changeTracker)
      {
        card._counters = Bindable.Create<Counters>(
          card._power,
          card._toughness,
          changeTracker,
          card);

        card._counters.Property(x => x.Count)
          .Changes(card).Property<Card, int?>(x => x.Counters);
      }

      private void CreateBindablePower(Card card, ChangeTracker changeTracker)
      {
        card._power = Bindable.Create<Power>(
          _power,
          changeTracker,
          card);

        card._power.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Power);
      }

      private void CreateBindableLevel(Card card, ChangeTracker changeTracker)
      {
        card._level = Bindable.Create<Level>(
          _isleveler ? 0 : (int?) null,
          changeTracker,
          card);

        card._level.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Level);
      }

      private void CreateBindableToughness(Card card, ChangeTracker changeTracker)
      {
        card._toughness = Bindable.Create<Toughness>(
          _toughness,
          changeTracker,
          card);

        card._toughness.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Toughness);
      }

      private void CreateBindableType(Card card, ChangeTracker changeTracker)
      {
        card._type = Bindable.Create<CardTypeCharacteristic>(
          _type,
          changeTracker,
          card);

        card._type.Property(x => x.Value)
          .Changes(card).Property<Card, string>(x => x.Type);
      }

      public CardFactory MayChooseNotToUntapDuringUntap()
      {
        _mayChooseNotToUntap = true;
        return this;
      }

      public CardFactory OverrideScore(int score)
      {
        _overrideScore = score;
        return this;
      }
    }
  }
}