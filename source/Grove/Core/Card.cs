namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.CombatRules;
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Card : GameObject, ITarget, IDamageable, IHashDependancy, IHasColors, IHasLife, IModifiable
  {
    private readonly ActivatedAbilities _activatedAbilities;
    private readonly Trackable<Card> _attachedTo = new Trackable<Card>();    
    private readonly TrackableList<Card> _attachments = new TrackableList<Card>();
    private readonly CardBase _base;
    private readonly CastRules _castRules;
    private readonly ColorsOfCard _colors;
    private readonly CombatRules _combatRules;
    private readonly Counters _counters;
    private readonly Trackable<int> _damage = new Trackable<int>();
    private readonly Trackable<bool> _hasLeathalDamage = new Trackable<bool>();
    private readonly Trackable<bool> _hasRegenerationShield = new Trackable<bool>();
    private readonly Trackable<bool> _hasSummoningSickness = new Trackable<bool>();
    private readonly Trackable<int?> _hash = new Trackable<int?>();
    private readonly Trackable<bool> _isHidden = new Trackable<bool>();
    private readonly Trackable<bool> _isPeeked = new Trackable<bool>();
    private readonly Trackable<bool> _isRevealed = new Trackable<bool>();
    private readonly Trackable<bool> _isTapped = new Trackable<bool>();
    private readonly Level _level;
    private readonly CombatCost _combatCost;
    private readonly MinimumBlockerCount _minimumBlockerCount = new MinimumBlockerCount(1);
    private readonly TrackableList<ICardModifier> _modifiers = new TrackableList<ICardModifier>();
    private readonly Protections _protections;
    private readonly SimpleAbilities _simpleAbilities;
    private readonly StaticAbilities _staticAbilities;
    private readonly Strenght _strenght;
    private readonly TriggeredAbilities _triggeredAbilities;
    private readonly TypeOfCard _typeOfCard;
    private readonly Trackable<int> _usageScore = new Trackable<int>();
    private readonly Trackable<IZone> _zone = new Trackable<IZone>(new NullZone());
    public TrackableEvent JoinedBattlefield;
    public TrackableEvent LeftBattlefield;
    private CardController _controller;
    private bool _isPreview = true;

    protected Card() {}

    public Card(CardTemplate template)
    {
      _base = new CardBase(template.CreateCardParameters());

      _strenght = new Strenght(_base);
      _level = new Level(_base);
      _combatCost = new CombatCost(_base);
      _counters = new Counters(_strenght);
      _typeOfCard = new TypeOfCard(_base);
      _colors = new ColorsOfCard(_base);

      _protections = new Protections(_base);

      _simpleAbilities = new SimpleAbilities(_base);
      _triggeredAbilities = new TriggeredAbilities(_base);
      _activatedAbilities = new ActivatedAbilities(_base);
      _staticAbilities = new StaticAbilities(_base);
      _castRules = new CastRules(_base);
      _combatRules = new CombatRules(_base);

      JoinedBattlefield = new TrackableEvent();
      LeftBattlefield = new TrackableEvent();
    }

    public CardTemplate Template
    {
      get { return _base.Value.Template; }
    }

    public bool MayChooseNotToUntap
    {
      get { return _base.Value.MayChooseToUntap; }
    }

    public int MinimalBlockerCount
    {
      get { return _minimumBlockerCount.Value.GetValueOrDefault(); }
    }

    public Card AttachedTo
    {
      get { return _attachedTo.Value; }
      private set { _attachedTo.Value = value; }
    }    

    public IEnumerable<Card> Attachments
    {
      get { return _attachments; }
    }

    public Rarity? Rarity { get; set; }

    public List<int> ProducableManaColors
    {
      get { return _base.Value.ManaColorsThisCardCanProduce; }
    }

    public string Set { get; set; }

    public bool CanAttack
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
          !Has().CannotAttack &&
          (!Has().CanAttackOnlyIfDefenderHasIslands || Controller.Opponent.Battlefield.Any(x => x.Is("island")));
      }
    }

    private int UsageScore
    {
      get { return _usageScore.Value; }
      set { _usageScore.Value = value; }
    }

    public bool HasFirstStrike
    {
      get { return Has().FirstStrike || Has().DoubleStrike; }
    }

    public bool HasNormalStrike
    {
      get { return !Has().FirstStrike || Has().DoubleStrike; }
    }

    public bool CanBeTapped
    {
      get { return IsPermanent && !IsTapped; }
    }

    public bool HasRegenerationShield
    {
      get { return _hasRegenerationShield.Value; }
      set { _hasRegenerationShield.Value = value; }
    }

    public bool CanTap
    {
      get { return !IsTapped && (!HasSummoningSickness || !Is().Creature || Has().Haste); }
    }

    public bool IsPermanent
    {
      get { return Zone == Zone.Battlefield; }
    }

    public int CharacterCount
    {
      get { return FlavorText.CharacterCount + Text.CharacterCount; }
    }

    public CardColor[] Colors
    {
      get { return _colors.ToArray(); }
    }

    public Player Controller
    {
      get { return _controller.Value; }
    }

    public Player Owner { get; private set; }

    public int Counters
    {
      get { return _counters.Count; }
    }

    public int Damage
    {
      get { return _damage.Value; }
      protected set { _damage.Value = value; }
    }

    public CardText FlavorText
    {
      get { return _base.Value.FlavorText; }
    }

    public bool HasAttachments
    {
      get { return _attachments.Count > 0; }
    }

    public bool HasLeathalDamage
    {
      get { return _hasLeathalDamage.Value; }
    }

    public bool HasSummoningSickness
    {
      get { return _hasSummoningSickness.Value; }
      set { _hasSummoningSickness.Value = value; }
    }

    public bool HasXInCost
    {
      get { return _castRules.HasXInCost; }
    }

    public string Illustration
    {
      get { return _base.Value.Illustration; }
    }

    public bool IsAttached
    {
      get { return AttachedTo != null; }
    }

    public bool IsAttacker
    {
      get { return Combat.IsAttacker(this); }
    }

    public bool HasBlockers
    {
      get { return Combat.HasBlockers(this); }
    }

    public bool IsBlocker
    {
      get { return Combat.IsBlocker(this); }
    }

    public bool IsTapped
    {
      get { return _isTapped.Value; }
      protected set { _isTapped.Value = value; }
    }

    public ManaAmount ManaCost
    {
      get { return _base.Value.ManaCost; }
    }

    public int ConvertedCost
    {
      get { return ManaCost == null ? 0 : ManaCost.Converted; }
    }

    public int GenericCost
    {
      get { return ManaCost == null ? 0 : ManaCost.Generic; }
    }

    private IEnumerable<IAcceptsCardModifier> ModifiableProperties
    {
      get
      {
        yield return _minimumBlockerCount;
        yield return _strenght;
        yield return _level;
        yield return _combatCost;
        yield return _counters;
        yield return _colors;
        yield return _typeOfCard;
        yield return _protections;
        yield return _triggeredAbilities;
        yield return _activatedAbilities;
        yield return _simpleAbilities;
        yield return _controller;
        yield return _base;
      }
    }

    public string Name
    {
      get { return _base.Value.Name; }
    }

    public int? BasePower
    {
      get { return _strenght.BasePower; }
    }

    public int? Power
    {
      get { return _strenght.Power; }
    }

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
            score = ScoreCalculator.CalculateCardInHandScore(this, Game.Ai.IsSearchInProgress);
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
        score = score - UsageScore;


        // auras controlled by other player are added to their score
        if (IsPermanent && Is().Aura && AttachedTo != null && AttachedTo.Controller != Controller)
        {
          return -score;
        }

        return score;
      }
    }

    public CardText Text
    {
      get { return _base.Value.Text; }
    }

    public int? Toughness
    {
      get { return _strenght.Toughness; }
    }

    public int? BaseToughness
    {
      get { return _strenght.BaseToughness; }
    }

    public CardType Type
    {
      get { return _typeOfCard.Value; }
    }

    public Zone Zone
    {
      get { return _zone.Value.Name; }
    }

    public int? Level
    {
      get { return _level.Value; }
    }

    public int CombatCost
    {
      get { return _combatCost.Value.GetValueOrDefault(); }
    }

    public bool CanBeDestroyed
    {
      get { return !HasRegenerationShield && !Has().Indestructible; }
    }

    public ScoreOverride OverrideScore
    {
      get { return _base.Value.OverrideScore; }
    }

    public bool IsVisibleInUi
    {
      get { return _isPreview || IsVisibleToPlayer(Players.Human); }
    }

    public bool IsVisibleToSearchingPlayer
    {
      get { return IsVisibleToPlayer(Players.Searching); }
    }

    public bool IsMultiColored
    {
      get { return _colors.Count > 1; }
    }

    public IEnumerable<string> Subtypes
    {
      get { return _typeOfCard.Value.SubTypes; }
    }

    public bool IsEnchanted
    {
      get { return _attachments.Any(x => x.Is().Aura); }
    }    

    public void ReceiveDamage(Damage damage)
    {
      if (!Is().Creature || !IsPermanent)
        return;

      if (HasProtectionFrom(damage.Source))
      {
        return;
      }

      var amountToPrevent = Game.PreventDamage(new PreventDamageParameters
        {
          Amount = damage.Amount,
          Source = damage.Source,
          Target = this,
          IsCombat = damage.IsCombat,
          CanBePrevented = damage.CanBePrevented,
          QueryOnly = false
        });

      damage.Amount -= amountToPrevent;

      if (damage.Amount == 0)
        return;

      var wasRedirected = Game.RedirectDamage(damage, this);

      if (wasRedirected)
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

      Publish(new DamageDealtEvent(this, damage));
    }

    public bool HasColor(CardColor color)
    {
      return _colors.Contains(color);
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

    void IModifiable.RemoveModifier(IModifier modifier)
    {
      RemoveModifier((ICardModifier) modifier);
    }

    public int Id { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      if (Ai.IsSearchInProgress && IsVisibleToSearchingPlayer == false)
      {
        return Zone.GetHashCode();
      }

      if (_hash.Value.HasValue == false)
      {
        // this value can be same for different cards with same NAME,
        // sometimes this is good sometimes not, currently we favor
        // smaller tree sizes and less accurate results.
        // if tree size is no longer a problem we will replace NAME with 
        // a guid.
        _hash.Value = HashCalculator.Combine(
          Name.GetHashCode(),
          _hasSummoningSickness.Value.GetHashCode(),
          UsageScore.GetHashCode(),
          IsTapped.GetHashCode(),
          Damage,
          HasRegenerationShield.GetHashCode(),
          HasLeathalDamage.GetHashCode(),
          calc.Calculate(_strenght),
          Level.GetHashCode(),
          Counters.GetHashCode(),
          calc.Calculate(_typeOfCard.Value),
          Zone.GetHashCode(),
          _isRevealed.Value.GetHashCode(),
          _isPeeked.Value.GetHashCode(),
          _isHidden.Value.GetHashCode(),
          calc.Calculate(_simpleAbilities),
          calc.Calculate(_triggeredAbilities),
          calc.Calculate(_activatedAbilities),
          calc.Calculate(_protections),
          calc.Calculate(_attachments),
          calc.Calculate(_colors)
          );
      }

      return _hash.Value.GetValueOrDefault();
    }

    public Player GetControllerOfACardThisIsAttachedTo()
    {
      return AttachedTo == null ? Controller : AttachedTo.GetControllerOfACardThisIsAttachedTo();
    }

    public void ChangeZone(IZone destination, Action<Card> add)
    {
      var source = _zone.Value;

      Asrt.False(destination == source,
        "Cannot change zones, when source zone and destination zone are the same.");

      source.Remove(this);
      _zone.Value = destination;
      add(this);

      if (destination.Name != source.Name)
      {                        
        if (destination.Name == Zone.Battlefield)
        {
          HasSummoningSickness = true;
          JoinedBattlefield.Raise();
        }
                
        Publish(new ZoneChangedEvent(this, source.Name, destination.Name));                
        
        if (source.Name == Zone.Battlefield)
        {
          Combat.Remove(this);

          DetachAttachments();
          Detach();
          Untap();
          ClearDamage();

          HasSummoningSickness = false;

          LeftBattlefield.Raise();
        }
      }
    }

    public bool IsColorless()
    {
      return _colors.Count == 0 || HasColor(CardColor.Colorless);
    }

    public int CountersCount(CounterType? counterType = null)
    {
      if (counterType == null)
        return _counters.Count;

      return _counters.CountSpecific(counterType.Value);
    }

    public CombatAbilities GetCombatAbilities()
    {
      return _combatRules.GetAbilities();
    }

    public void DealDamageTo(int amount, IDamageable damageable, bool isCombat, bool canBePrevented = true)
    {
      if (amount <= 0)
        return;

      var damage = new Damage(
        amount: amount,
        source: this,
        isCombat: isCombat,
        canBePrevented: canBePrevented);

      damage.Initialize(ChangeTracker);
      damageable.ReceiveDamage(damage);
    }

    public void AddModifier(ICardModifier modifier, ModifierParameters p)
    {
      _modifiers.Add(modifier);
      ActivateModifier(modifier, p);
      
      if (IsPermanent)
      {
        Publish(new PermanentModifiedEvent(this, modifier));
      }
    }

    public void RemoveModifier(ICardModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();

      if (IsPermanent)
      {
        Publish(new PermanentModifiedEvent(this, modifier));
      }
    }

    public Card Initialize(Player owner, Game game)
    {
      Game = game;
      Owner = owner;
      Id = game.Recorder.CreateId(this);

      JoinedBattlefield.Initialize(ChangeTracker);
      LeftBattlefield.Initialize(ChangeTracker);

      _base.Initialize(game, this);
      _controller = new CardController(owner);
      _controller.Initialize(game, this);

      _strenght.Initialize(game, this);
      _level.Initialize(game, this);
      _combatCost.Initialize(game,this);
      _counters.Initialize(this, game);
      _typeOfCard.Initialize(game, this);
      _colors.Initialize(game, this);
      _protections.Initialize(game, this);
      _zone.Initialize(ChangeTracker, this);
      _modifiers.Initialize(ChangeTracker);

      _isTapped.Initialize(ChangeTracker, this);
      _attachedTo.Initialize(ChangeTracker, this);      
      _attachments.Initialize(ChangeTracker, this);
      _hasRegenerationShield.Initialize(ChangeTracker, this);
      _damage.Initialize(ChangeTracker, this);
      _hasLeathalDamage.Initialize(ChangeTracker, this);
      _hasSummoningSickness.Initialize(ChangeTracker, this);
      _hash.Initialize(ChangeTracker);
      _isHidden.Initialize(ChangeTracker, this);
      _isRevealed.Initialize(ChangeTracker, this);
      _isPeeked.Initialize(ChangeTracker, this);
      _usageScore.Initialize(ChangeTracker, this);

      _base.Value.Initialize(this, Game);

      _castRules.Initialize(this, game);
      _simpleAbilities.Initialize(this, game);
      _triggeredAbilities.Initialize(this, game);
      _activatedAbilities.Initialize(this, game);
      _staticAbilities.Initialize(this, game);
      _combatRules.Initialize(this, game);


      _minimumBlockerCount.Initialize(Game, null);
      _isPreview = false;

      return this;
    }

    public int CalculatePreventedDamageAmount(int totalAmount, Card source, bool isCombat = false)
    {
      if (HasProtectionFrom(source))
      {
        return totalAmount;
      }

      var damage = new PreventDamageParameters
        {
          Amount = totalAmount,
          QueryOnly = true,
          Source = source,
          IsCombat = isCombat,
          Target = this,
        };

      return Game.PreventDamage(damage);
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

      var increase = Turn.TurnCount;
      if (Turn.Step < Step.SecondMain)
        increase += 1;

      UsageScore += increase;
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
      _attachments.Add(attachment);
      Publish(new AttachmentAttachedEvent(attachment));
    }

    public List<ActivationPrerequisites> CanActivateAbilities(bool ignoreManaAbilities = false)
    {
      return _activatedAbilities.CanActivate(ignoreManaAbilities);
    }

    public bool CanBeBlockedBy(Card card)
    {
      if (card.IsTapped)
        return false;

      if (Has().Unblockable)
        return false;

      if (Has().Flying && !card.Has().Flying && !card.Has().Reach)
        return false;

      if (!Has().Flying && card.Has().CanBlockOnlyCreaturesWithFlying)
        return false;

      if (Has().CanOnlyBeBlockedByCreaturesWithFlying && !card.Has().Flying)
        return false;

      if (Has().CanOnlyBeBlockedByWalls && !card.Is("wall"))
        return false;

      if (Has().CannotBeBlockedByWalls && card.Is("wall"))
        return false;

      if (Has().Fear && !card.HasColor(CardColor.Black) && !card.Is().Artifact)
        return false;

      if (Has().Intimidate && !card.Is().Artifact && _colors.None(card.HasColor))
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

      if (Has().Forestwalk &&
        card.Controller.Battlefield.Any(x => x.Is("forest")))
      {
        return false;
      }

      if (Has().UnblockableIfDedenderHasArtifacts &&
        card.Controller.Battlefield.Any(x => x.Is().Artifact))
      {
        return false;
      }

      if (Has().UnblockableIfDedenderHasEnchantments &&
        card.Controller.Battlefield.Any(x => x.Is().Enchantment))
      {
        return false;
      }

      if (_base.Value.MinBlockerPower.HasValue)
      {
        return card.Power.GetValueOrDefault(0) >= _base.Value.MinBlockerPower;
      }

      return true;
    }

    public bool CanBeTargetBySpellsWithColor(CardColor color)
    {
      return !Has().Shroud && !Has().Hexproof && !HasProtectionFrom(color);
    }

    public bool CanBeTargetBySpellsOwnedBy(Player player)
    {
      return !Has().Shroud && (player == Controller || !Has().Hexproof);
    }

    public bool CanBlock()
    {
      return IsPermanent && !IsTapped && Is().Creature && !Has().CannotBlock;
    }

    public bool CanTarget(ITarget target)
    {
      return _castRules.CanTarget(target);
    }

    public bool IsGoodTarget(ITarget target, Player controller)
    {
      return _castRules.IsGoodTarget(target, controller);
    }

    public List<ActivationPrerequisites> CanCast()
    {
      return _castRules.CanCast();
    }

    public void Cast(int index, ActivationParameters activationParameters)
    {
      _castRules.Cast(index, activationParameters);
      IncreaseUsageScore();
    }

    public void ClearDamage()
    {
      Damage = 0;
      _hasLeathalDamage.Value = false;
    }

    public void Detach(Card card)
    {
      _attachments.Remove(card);
      card.AttachedTo = null;

      Publish(new AttachmentDetachedEvent(attachment: card, attachedTo: this));
    }

    public void EnchantWithoutPayingCost(Card target)
    {
      var activationParameters = new ActivationParameters
        {
          PayCost = false,
          SkipStack = true
        };

      activationParameters.Targets.Effect.Add(target);

      _castRules.Cast(0, activationParameters);
    }

    public void EquipWithoutPayingCost(Card target)
    {
      var activationParameters = new ActivationParameters
        {
          PayCost = false,
          SkipStack = true
        };

      activationParameters.Targets.Effect.Add(target);

      _activatedAbilities.Activate(0, activationParameters);
    }

    public ManaAmount GetActivatedAbilityManaCost(int index)
    {
      return _activatedAbilities.GetManaCost(index);
    }

    public IEnumerable<ManaAmount> GetActivatedAbilitiesManaCost()
    {
      return _activatedAbilities.GetManaCost();
    }

    public ISimpleAbilities Has()
    {
      return _simpleAbilities;
    }

    public bool HasAttachment(Card card)
    {
      return _attachments.Contains(card);
    }

    public bool HasProtectionFrom(CardColor color)
    {
      return _protections.HasProtectionFrom(color);
    }    

    public bool HasProtectionFromAny(IEnumerable<CardColor> colors)
    {
      return colors.Any(x => _protections.HasProtectionFrom(x));
    }

    public bool HasProtectionFromAny(IEnumerable<string> types)
    {
      return types.Any(x => _protections.HasProtectionFrom(x));
    }

    public bool HasProtectionFrom(Card card)
    {
      return HasProtectionFromAny(card._colors) ||
        HasProtectionFromAny(card._typeOfCard.Value.BaseTypes) ||
        HasProtectionFromAny(card._typeOfCard.Value.SubTypes);
    }

    public void Hide()
    {
      _isHidden.Value = true;
      _isRevealed.Value = false;
    }

    public ITargetType Is()
    {
      return _typeOfCard.Value;
    }

    public bool Is(string type)
    {
      return _typeOfCard.Value.Is(type);
    }

    public void RemoveCounters(CounterType counterType, int? count = null)
    {
      _counters.Remove(counterType, count);
    }

    public void Destroy(bool allowToRegenerate = true)
    {
      if (!IsPermanent)
        return;

      if (Has().Indestructible)
      {
        return;
      }

      if (HasRegenerationShield && allowToRegenerate)
      {
        Regenerate();
        return;
      }

      Owner.PutCardToGraveyard(this);
    }


    public void Sacrifice()
    {
      if (!IsPermanent)
        return;

      Owner.PutCardToGraveyard(this);
    }

    public void Tap()
    {
      IsTapped = true;
      UsageScore += ScoreCalculator.CalculateTapPenalty(this, Turn);

      Publish(new PermanentTappedEvent(this));
    }

    public override string ToString()
    {
      return Name;
    }

    public void Untap()
    {
      IsTapped = false;

      if (Turn.Step != Step.Untap)
      {
        UsageScore -= ScoreCalculator.CalculateTapPenalty(this, Turn);
      }

      Publish(new PermanentUntappedEvent(this));
    }

    public void DetachAttachments()
    {
      foreach (var attachedCard in _attachments.ToList())
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

    public void Exile()
    {
      Owner.PutCardToExile(this);
    }

    public void ExileFrom(Zone from)
    {
      if (from != Zone)
        return;

      Exile();
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
        return _isPeeked && player == Controller;

      return player == Controller;
    }

    public void Reveal()
    {
      // Reveal should only work during actual game.
      // Revealing cards during simulation should have no 
      // effect.

      if (Ai.IsSearchInProgress)
        return;

      _isRevealed.Value = true;
      _isPeeked.Value = true;
      _isHidden.Value = false;

      Publish(new CardWasRevealedEvent(this));
    }

    public void Peek()
    {
      // Peek should only work during actual game.
      // Peeking at cards during simulation should have no 
      // effect.

      if (Ai.IsSearchInProgress)
        return;

      _isHidden.Value = false;
      _isPeeked.Value = true;
    }

    public void ResetVisibility()
    {
      _isRevealed.Value = false;
      _isHidden.Value = false;
      _isPeeked.Value = false;
    }

    public void PutToHandFrom(Zone from)
    {
      if (Zone != from)
        return;

      PutToHand();
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

    public void ShuffleIntoLibraryFrom(Zone from)
    {
      if (Zone != from)
        return;

      ShuffleIntoLibrary();
    }

    public void ShuffleIntoLibrary()
    {
      Owner.ShuffleIntoLibrary(this);
    }

    public bool PutToBattlefieldFrom(Zone from)
    {
      if (Zone != from)
        return false;

      PutToBattlefield();
      return true;
    }

    public void PutToBattlefield()
    {
      Controller.PutCardToBattlefield(this);
    }

    public void PutToGraveyard()
    {
      Owner.PutCardToGraveyard(this);
    }

    public ManaAmount GetSpellManaCost(int index)
    {
      return _castRules.GetManaCost(index);
    }

    public int CalculateCombatDamageAmount(bool singleDamageStep = true, int powerIncrease = 0)
    {
      var total = Power.GetValueOrDefault() + powerIncrease;

      if (Has().DoubleStrike && !singleDamageStep)
        return total*2;

      return total;
    }

    public void PutOnTopOfLibraryFrom(Zone from)
    {
      if (Zone != from)
        return;

      PutOnTopOfLibrary();
    }

    private void ActivateModifier(ICardModifier modifier, ModifierParameters p)
    {
      p.Owner = this;
      modifier.Initialize(p, Game);

      foreach (var modifiable in ModifiableProperties)
      {
        modifiable.Accept(modifier);
      }

      modifier.Activate();
    }

    private void Regenerate()
    {
      Tap();
      ClearDamage();
      HasRegenerationShield = false;
      Combat.Remove(this);
    }
  }
}