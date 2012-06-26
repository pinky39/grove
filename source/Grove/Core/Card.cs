namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using CardDsl;
  using CastingRules;
  using Counters;
  using DamagePrevention;
  using Effects;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Zones;

  [Copyable]
  public class Card : IEffectSource, ITarget, IDamageable, IHashDependancy
  {
    private static readonly Random Random = new Random();

    private ActivatedAbilities _activatedAbilities;
    private Trackable<Card> _attachedTo;
    private Attachments _attachments;
    private Trackable<bool> _canRegenerate;
    private CardColors _colors;
    private Combat _combat;
    private TrackableList<ContinuousEffect> _continuousEffects;
    private Trackable<Player> _controller;
    private Counters.Counters _counters;
    private Trackable<int> _damage;
    private DamagePreventions _damagePreventions;
    private IEffectFactory _effectFactory;
    private Trackable<bool> _hasLeathalDamage;
    private Trackable<bool> _hasSummoningSickness;
    private Trackable<int?> _hash;
    private Trackable<bool> _isHidden;
    private Trackable<bool> _isTapped;
    private IEffectFactory _kickerEffectFactory;
    private TargetSelector _kickerTargetSelector;
    private Level _level;
    private List<IManaSource> _manaSources;
    private TrackableList<IModifier> _modifiers;
    private Power _power;
    private Protections _protections;
    private Publisher _publisher;
    private Zone _putToZoneAfterResolve;
    private StaticAbilities _staticAbilities;
    private TargetSelector _targetSelector;
    private TimingDelegate _timming;
    private Toughness _toughness;
    private TriggeredAbilities _triggeredAbilities;
    private CardTypeCharacteristic _type;
    private Trackable<int> _usageCount;
    private Trackable<bool> _wasEchoPaid;
    private CalculateX _xCalculator;
    private Trackable<Zone> _zone;

    protected Card() {}

    public Card AttachedTo { get { return _attachedTo.Value; } private set { _attachedTo.Value = value; } }

    public IEnumerable<Card> Attachments { get { return _attachments.Cards; } }

    public bool CanAttack
    {
      get
      {
        return IsPermanent && Is().Creature &&
          !IsTapped && !HasSummoningSickness && !Has().Defender;
      }
    }

    public bool HasFirstStrike { get { return Has().FirstStrike || Has().DoubleStrike; } }

    public bool HasNormalStrike { get { return !Has().FirstStrike || Has().DoubleStrike; } }

    public bool CanBeTapped { get { return IsPermanent && !IsTapped; } }

    public bool CanRegenerate { get { return _canRegenerate.Value; } set { _canRegenerate.Value = value; } }

    public bool CanTap { get { return IsPermanent && !HasSummoningSickness && !IsTapped; } }

    public CastingRule CastingRule { get; private set; }

    public int CharacterCount { get { return FlavorText.CharacterCount + Text.CharacterCount; } }

    public int ChargeCountersCount { get { return _counters.SpecifiCount<ChargeCounter>(); } }

    public virtual ManaColors Colors { get { return _colors.Value; } }

    public Player Controller { get { return _controller.Value; } set { _controller.Value = value; } }

    public int? Counters { get { return _counters.Count; } }

    public virtual int Damage { get { return _damage.Value; } protected set { _damage.Value = value; } }

    public CardText FlavorText { get; private set; }

    public bool HasAttachments { get { return _attachments.Count > 0; } }

    public bool HasKicker { get { return KickerCost != null; } }

    public bool HasLeathalDamage { get { return _hasLeathalDamage.Value; } }

    public bool HasSummoningSickness { get { return _hasSummoningSickness.Value && Is().Creature && !Has().Haste; } }

    public bool HasXInCost { get { return _xCalculator != null; } }

    public string Illustration { get; private set; }

    public bool IsAttached { get { return AttachedTo != null; } }

    public bool IsAttacker { get { return _combat.IsAttacker(this); } }

    public bool IsBlocker { get { return _combat.IsBlocker(this); } }

    public bool IsHidden { get { return _isHidden.Value; } private set { _isHidden.Value = value; } }

    public bool IsManaSource { get { return _manaSources.Count > 0; } }

    public bool IsPermanent { get { return Zone == Zone.Battlefield; } }

    public virtual bool IsTapped { get { return _isTapped.Value; } protected set { _isTapped.Value = value; } }

    public IManaAmount KickerCost { get; private set; }

    public int LifepointsLeft { get { return Toughness.Value - Damage; } }

    public IManaAmount ManaCost { get; private set; }
    public IManaAmount ManaCostWithKicker { get; private set; }

    public IEnumerable<IManaSource> ManaSources { get { return _manaSources; } }

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
      }
    }

    public string Name { get; private set; }

    public int? Power { get { return _power.Value; } }

    public int Score
    {
      get
      {
        var score = 0;

        switch (Zone)
        {
          case (Zone.Battlefield):
            score = Ai.ScoreCalculator.CalculatePermanentScore(this);
            break;

          case (Zone.Hand):
            score = Ai.ScoreCalculator.CalculateCardInHandScore(this);
            break;
        }

        // card usage lowers the score slightly, since we want't to 
        // avoid activations that do no good

        return score - _usageCount.Value;
      }
    }

    public CardText Text { get; private set; }

    public int? Toughness { get { return _toughness.Value; } }

    public string Type { get { return _type.Value.ToString(); } }

    public Zone Zone { get { return _zone.Value; } }

    public int? Level { get { return _level.Value; } }
    public IManaAmount EchoCost { get; private set; }
            
    public void DealDamage(Card damageSource, int amount, bool isCombat)
    {
      var damage = new Damage(damageSource, amount);
      var dealtAmount = CalculateDealtDamageAmount(damage, queryOnly: false);

      if (dealtAmount == 0)
        return;

      Damage += dealtAmount;

      if (Damage >= Toughness || damage.IsLeathal)
      {
        _hasLeathalDamage.Value = true;
      }

      if (damageSource.Has().Lifelink)
      {
        var controller = damageSource.Controller;
        controller.Life += dealtAmount;
      }

      Publish(
        new DamageHasBeenDealt
          {
            Dealer = damageSource,
            Amount = dealtAmount,
            Receiver = this,
            IsCombat = isCombat
          });

      this.Updates("Damage");
    }

    public EffectCategories EffectCategories { get; private set; }

    Card IEffectSource.OwningCard { get { return this; } }

    public void EffectWasCountered()
    {
      Controller.PutCardToGraveyard(this);
    }

    void IEffectSource.EffectWasPushedOnStack()
    {
      SetZone(Zone.Stack);
    }

    void IEffectSource.EffectWasResolved()
    {
      if (!IsPermanent)
      {
        switch (_putToZoneAfterResolve)
        {
          case (Zone.Library):
            Controller.ShuffleIntoLibrary(this);
            break;
          default:
            Controller.PutCardToGraveyard(this);
            break;
        }
      }
    }

    bool IEffectSource.IsTargetValid(ITarget target)
    {
      return _targetSelector.IsValid(target);
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
            calc.Calculate(_hasSummoningSickness),
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

    public void InvalidateHash()
    {
      _hash.Value = null;
    }

    public int CalculateDealtDamageAmount(Damage damage)
    {
      return CalculateDealtDamageAmount(damage, queryOnly: true);
    }

    public bool CanBeDestroyed
    {
      get { return !CanRegenerate && !Has().Indestructible; }
    }

    private int CalculateDealtDamageAmount(Damage damage, bool queryOnly)
    {
      if (HasProtectionFrom(damage.Source))
        return 0;

      return _damagePreventions.PreventDamage(damage.Source, damage.Amount, queryOnly);
    }

    public void ActivateAbility(int index, ActivationParameters activationParameters)
    {
      _activatedAbilities.Activate(index, activationParameters);
      _usageCount.Value++;
    }

    public virtual void AddModifier(IModifier modifier)
    {
      foreach (var modifiable in ModifiableProperties)
      {
        modifiable.Accept(modifier);
      }
      _modifiers.Add(modifier);

      modifier.Activate();
    }

    public void Attach(Card attachment, List<Modifier> attachmentModifiers)
    {
      if (attachment.IsAttached)
      {
        attachment.AttachedTo.Detach(attachment);
      }

      attachment.AttachedTo = this;

      foreach (var modifier in attachmentModifiers)
      {
        AddModifier(modifier);
      }

      _attachments.Add(new Attachment(attachment, attachmentModifiers));

      Publish(new AttachmentAttached {Attachment = attachment});
    }

    public List<SpellPrerequisites> CanActivateAbilities()
    {
      return _activatedAbilities.CanActivate();
    }

    public SpellPrerequisites CanActivateAbility(int abilityIndex)
    {
      return _activatedAbilities.CanActivate(abilityIndex);
    }

    public bool CanBeBlockedBy(Card card)
    {
      if (Has().Unblockable)
        return false;

      if (Has().Flying && !card.Has().Flying && !card.Has().Reach)
        return false;

      if (Has().Fear && !card.HasColor(ManaColors.Black) && !card.Is().Artifact)
        return false;

      if (HasProtectionFrom(card))
        return false;

      if (Has().Swampwalk && card.Controller.Battlefield.Any(x => x.Is("swamp")))
        return false;

      return true;
    }

    public bool CanBeTargetBySpellsOwnedBy(Player player)
    {
      return !Has().Shroud && (player == Controller ? true : !Has().Hexproof);
    }

    public bool CanBlock()
    {
      return IsPermanent && !IsTapped && Is().Creature;
    }

    public SpellPrerequisites CanCast()
    {
      if (!Controller.HasPriority || !CastingRule.CanCast())
        return new SpellPrerequisites {CanBeSatisfied = false};

      var canCastWithKicker = HasKicker ? Controller.HasMana(ManaCostWithKicker) : false;

      return new SpellPrerequisites
        {
          CanBeSatisfied = true,
          EffectTargetSelector = _targetSelector,
          KickerTargetSelector = _kickerTargetSelector,
          CanCastWithKicker = canCastWithKicker,
          MaxX = GetMaxX(),
          XCalculator = _xCalculator,
          Timming = _timming,
        };
    }
   
    public int TotalDamageThisCanDealToPlayerIfNotBlocked
    {
      get
      {                
        return Has().DoubleStrike ? 2 * Power.Value : Power.Value;
      }
    }

    public void CastInternal(ActivationParameters activationParameters)
    {
      var effect = activationParameters.PayKicker
        ? _kickerEffectFactory.CreateEffect(this, x: activationParameters.X, wasKickerPaid: true)
        : _effectFactory.CreateEffect(this, x: activationParameters.X);

      effect.Target = activationParameters.EffectTarget;
      CastingRule.Cast(effect);
      _usageCount.Value++;
    }

    public void ClearDamage()
    {
      Damage = 0;
      _hasLeathalDamage.Value = false;
    }

    public void Destroy()
    {
      Controller.DestroyCard(this);
    }

    public void Detach(Card card)
    {
      var attachment = _attachments[card];

      _attachments.Remove(attachment);
      card.AttachedTo = null;

      if (card.Is().Enchantment)
      {
        card.Controller.SacrificeCard(card);
      }

      Publish(new AttachmentDetached
        {
          AttachedTo = this,
          Attachment = card
        });
    }

    public void EnchantWithoutPayingTheCost(Card enchantment)
    {
      var effect = enchantment._effectFactory.CreateEffect(enchantment);

      if (effect is EnchantCreature == false)
        throw new InvalidOperationException("Card is is not an enchantment.");

      effect.Target = this;
      effect.Resolve();
    }

    public void EquipWithoutPayingTheCost(Card equipment)
    {
      var effect = equipment._activatedAbilities.GetEffect<AttachEquipment>();

      if (effect == null)
        throw new InvalidOperationException("Card is is not an equipment.");

      effect.Target = this;
      effect.Resolve();
    }

    public IManaAmount GetActivatedAbilityManaCost(int index)
    {
      return _activatedAbilities.GetManaCost(index);
    }

    public IHasStaticAbilities Has()
    {
      return _staticAbilities;
    }

    public bool HasAttachment(Card card)
    {
      return _attachments.Contains(card);
    }

    public bool HasColor(ManaColors color)
    {
      return (Colors & color) == color;
    }


    public bool HasProtectionFrom(ManaColors colors)
    {
      return _protections.HasProtectionFrom(colors);
    }

    public bool HasProtectionFrom(Card card)
    {
      return _protections.HasProtectionFrom(card.Colors) ||
        _protections.HasProtectionFrom(card.Type);
    }

    public void Hide()
    {
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
      _combat.Remove(this);
    }

    public void RemoveChargeCounter()
    {
      _counters.RemoveAny<ChargeCounter>();
    }

    public void RemoveModifier(IModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();
    }

    public void RemoveSummoningSickness()
    {
      _hasSummoningSickness.Value = false;
    }

    public void ReturnToHand()
    {
      Controller.ReturnToHand(this);
    }

    public void Sacrifice()
    {
      Controller.SacrificeCard(this);
    }

    public void SetZone(Zone value)
    {
      var oldZone = _zone.Value;
      var newZone = value;

      if (newZone == oldZone)
        return;

      if (oldZone == Zone.Battlefield)
      {
        _combat.Remove(this);

        DetachAttachments();
        DetachSelf();
        Untap();
        ClearDamage();
      }

      if (newZone == Zone.Battlefield)
      {
        _hasSummoningSickness.Value = true;
        _wasEchoPaid.Value = false;
      }

      _zone.Value = newZone;

      Publish(new CardChangedZone
        {
          Card = this,
          From = oldZone,
          To = Zone
        });
    }

    public void Show()
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
    }

    private void DetachAttachments()
    {
      foreach (var attachedCard in _attachments.Cards.ToList())
      {
        Detach(attachedCard);
      }
    }

    private void DetachSelf()
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
        maxX = Controller.ConvertedMana - ManaCostWithKicker.Converted;
      }
      return maxX;
    }

    private void Publish<T>(T message)
    {
      _publisher.Publish(message);
    }

    public void Exile()
    {
      Controller.ExileCard(this);
    }

    public bool PlayerNeedsToPayEchoCost()
    {
      return EchoCost != null && _wasEchoPaid == false;
    }

    public void PayEchoCost()
    {
      Controller.Consume(EchoCost);
      _wasEchoPaid.Value = true;
    }   

    public class CardFactory : ICardFactory
    {
      private readonly List<IActivatedAbilityFactory> _activatedAbilityFactories = new List<IActivatedAbilityFactory>();
      private readonly ChangeTracker _changeTracker;
      private readonly List<IContinuousEffectFactory> _continuousEffectFactories = new List<IContinuousEffectFactory>();
      private readonly Game _game;
      private readonly List<StaticAbility> _staticAbilities = new List<StaticAbility>();
      private readonly List<ITriggeredAbilityFactory> _triggeredAbilityFactories = new List<ITriggeredAbilityFactory>();
      private ManaColors _colors;
      private string _echoCost;
      private EffectCategories _effectCategories;
      private IEffectFactory _effectFactory;
      private string _flavorText;
      private bool _isleveler;
      private string _kickerCost;
      private IEffectFactory _kickerEffectFactory;
      private ITargetSelectorFactory _kickerSelectorFactory;
      private string _manaCost;
      private string _name;
      private int? _power;
      private string[] _protectionsFromCardTypes;
      private ManaColors _protectionsFromColors = ManaColors.None;
      private Zone _putToZoneAfterResolve = Zone.Graveyard;
      private ITargetSelectorFactory _targetSelectorFactory;
      private string _text;
      private TimingDelegate _timing;
      private int? _toughness;
      private CardType _type;
      private CalculateX _xCalculator;

      public CardFactory(Game game)
      {
        _changeTracker = game.ChangeTracker;
        _game = game;
      }

      public string Name { get { return _name; } }

      public Card CreateCard(Player controller)
      {
        var card = _game.Search.InProgress ? new Card() : Bindable.Create<Card>();

        card._publisher = _game.Publisher;
        card._combat = _game.Combat;

        card._effectFactory = _effectFactory ?? new Effect.Factory<PutIntoPlay> {Game = _game};
        card._kickerEffectFactory = _kickerEffectFactory;
        card._hash = new Trackable<int?>(_changeTracker);

        card.Name = _name;
        card._xCalculator = _xCalculator;
        card.ManaCost = _manaCost.ParseManaAmount();
        card.EchoCost = _echoCost.ParseManaAmount();
        card.KickerCost = _kickerCost.ParseManaAmount();
        card.ManaCostWithKicker = card.HasKicker ? card.ManaCost.Add(card.KickerCost) : card.ManaCost;
        card.Text = _text ?? String.Empty;
        card.FlavorText = _flavorText ?? String.Empty;
        card.Illustration = GetIllustration(_name, _type);
        card.CastingRule = CreateCastingRule(card, _type, _game);

        CreateBindablePower(card);
        CreateBindableToughness(card);
        CreateBindableLevel(card);
        CreateBindableCounters(card);
        CreateBindableType(card);
        CreateBindableColors(card);

        card._putToZoneAfterResolve = _putToZoneAfterResolve;
        card._damage = new Trackable<int>(_changeTracker, card);
        card._usageCount = new Trackable<int>(_changeTracker);
        card._isTapped = new Trackable<bool>(_changeTracker, card);
        card._hasLeathalDamage = new Trackable<bool>(_changeTracker, card);
        card._attachedTo = new Trackable<Card>(_changeTracker, card);
        card._attachments = new Attachments(_changeTracker);
        card._isHidden = new Trackable<bool>(_changeTracker, card);
        card._canRegenerate = new Trackable<bool>(_changeTracker, card);
        card._hasSummoningSickness = new Trackable<bool>(true, _changeTracker, card);
        card._wasEchoPaid = new Trackable<bool>(_changeTracker, card);
        card._controller = new Trackable<Player>(controller, _changeTracker, card);
        card._damagePreventions = new DamagePreventions(_changeTracker, card);
        card._protections = new Protections(_changeTracker, card, _protectionsFromColors, _protectionsFromCardTypes);
        card._zone = new Trackable<Zone>(_changeTracker, card);

        card.EffectCategories = _effectCategories;
        card._timming = _timing ?? Timings.MainPhases();

        card._modifiers = new TrackableList<IModifier>(_changeTracker);

        card._targetSelector = _targetSelectorFactory != null
          ? _targetSelectorFactory.Create(card)
          : null;

        card._kickerTargetSelector = _kickerSelectorFactory != null
          ? _kickerSelectorFactory.Create(card)
          : null;

        card._staticAbilities = new StaticAbilities(_staticAbilities, _changeTracker, card);

        var triggeredAbilities = _triggeredAbilityFactories.Select(x => x.Create(card, card));
        card._triggeredAbilities = new TriggeredAbilities(triggeredAbilities, _changeTracker, card);

        var activatedAbilities = _activatedAbilityFactories.Select(x => x.Create(card)).ToList();
        card._activatedAbilities = new ActivatedAbilities(activatedAbilities, _changeTracker, card);

        card._manaSources = activatedAbilities.Where(ability => ability is IManaSource).Cast<IManaSource>().ToList();

        var continiousEffects = _continuousEffectFactories.Select(factory => factory.Create(card)).ToList();
        card._continuousEffects = new TrackableList<ContinuousEffect>(continiousEffects, _changeTracker, card);

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
        card._counters = new Counters.Counters(card._power, card._toughness, null, null);

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
        _echoCost = manaCost;
        return this;
      }

      public CardFactory Abilities(params object[] abilities)
      {
        foreach (var ability in abilities)
        {
          if (ability is StaticAbility)
          {
            _staticAbilities.Add((StaticAbility) ability);
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
        _putToZoneAfterResolve = zone;
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

      public CardFactory Effect<T>(Initializer<T> init = null) where T : Effect, new()
      {
        init = init ?? delegate { };

        _effectFactory = new Effect.Factory<T>
          {
            Game = _game,
            Init = init,
          };

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


      public CardFactory KickerEffect<T>(Initializer<T> init = null) where T : Effect, new()
      {
        init = init ?? delegate { };

        _kickerEffectFactory = new Effect.Factory<T>
          {
            Game = _game,
            Init = init,
          };

        return this;
      }

      public CardFactory KickerTarget(ITargetSelectorFactory targetSelectorFactory)
      {
        _kickerSelectorFactory = targetSelectorFactory;
        return this;
      }

      public CardFactory ManaCost(string manaCost)
      {
        _manaCost = manaCost;
        return this;
      }

      public CardFactory Named(string name)
      {
        _name = name;
        return this;
      }

      public CardFactory Power(int power)
      {
        _power = power;
        return this;
      }

      public CardFactory Target(ITargetSelectorFactory targetSelectorFactory)
      {
        _targetSelectorFactory = targetSelectorFactory;
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

      private static CastingRule CreateCastingRule(Card card, CardType type, Game game)
      {
        if (type.Instant)
          return new Instant(card, game.Stack);

        if (type.Land)
          return new Land(card, game.Stack, game.Turn);

        return new Sorcery(card, game.Stack, game.Turn);
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

      private void CreateBindableColors(Card card)
      {
        card._colors =
          Bindable.Create<CardColors>(_colors == ManaColors.None ? card.GetCardColorFromManaCost() : _colors,
            _changeTracker, card);

        card._colors.Property(x => x.Value)
          .Changes(card).Property<Card, ManaColors>(x => x.Colors);
      }

      private void CreateBindableCounters(Card card)
      {
        card._counters = Bindable.Create<Counters.Counters>(card._power, card._toughness, _changeTracker, card);

        card._counters.Property(x => x.Count)
          .Changes(card).Property<Card, int?>(x => x.Counters);
      }

      private void CreateBindablePower(Card card)
      {
        card._power = Bindable.Create<Power>(_power, _changeTracker, card);

        card._power.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Power);
      }

      private void CreateBindableLevel(Card card)
      {
        card._level = Bindable.Create<Level>(
          _isleveler ? 0 : (int?) null, _changeTracker, card);

        card._level.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Level);
      }

      private void CreateBindableToughness(Card card)
      {
        card._toughness = Bindable.Create<Toughness>(_toughness, _changeTracker, card);

        card._toughness.Property(x => x.Value)
          .Changes(card).Property<Card, int?>(x => x.Toughness);
      }

      private void CreateBindableType(Card card)
      {
        card._type = Bindable.Create<CardTypeCharacteristic>(_type, _changeTracker, card);

        card._type.Property(x => x.Value)
          .Changes(card).Property<Card, string>(x => x.Type);
      }
    }
  }
}