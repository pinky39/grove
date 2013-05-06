namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Abilities;
  using Artifical;
  using Characteristics;
  using Counters;
  using Damage;
  using Infrastructure;
  using ManaHandling;
  using Messages;
  using Misc;
  using Modifiers;
  using States;
  using Targeting;
  using Zones;

  [Copyable]
  public class Card : GameObject, ITarget, IDamageable, IHashDependancy, IHasColors, IHasLife
  {
    private readonly ActivatedAbilities _activatedAbilities;
    private readonly Trackable<Card> _attachedTo = new Trackable<Card>();
    private readonly Attachments _attachments = new Attachments();
    private readonly Trackable<bool> _canRegenerate = new Trackable<bool>();
    private readonly CastInstructions _castInstructions;
    private readonly CardColors _colors;
    private readonly ContiniousEffects _continuousEffects;
    private readonly Counters.Counters _counters;
    private readonly Trackable<int> _damage = new Trackable<int>();
    private readonly DamagePreventions _damagePreventions;
    private readonly Trackable<bool> _hasLeathalDamage = new Trackable<bool>();
    private readonly Trackable<bool> _hasSummoningSickness = new Trackable<bool>();
    private readonly Trackable<int?> _hash = new Trackable<int?>();
    private readonly Trackable<bool> _isHidden = new Trackable<bool>();
    private readonly Trackable<bool> _isRevealed = new Trackable<bool>();
    private readonly Trackable<bool> _isPeeked = new Trackable<bool>();
    private readonly Trackable<bool> _isTapped = new Trackable<bool>();
    private readonly Level _level;
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();
    private readonly Power _power;
    private readonly Protections _protections;
    private readonly StaticAbilities _staticAbilities;
    private readonly Toughness _toughness;
    private readonly TriggeredAbilities _triggeredAbilities;
    private readonly CardTypeCharacteristic _type;
    private readonly Trackable<int> _usageScore = new Trackable<int>();
    private readonly CardZone _zone = new CardZone();

    public TrackableEvent JoinedBattlefield;
    public TrackableEvent LeftBattlefield;
    private ControllerCharacteristic _controller;
    private bool _isPreview = true;

    protected Card() {}

    public Card(CardParameters p)
    {
      Name = p.Name;
      ManaCost = p.ManaCost;
      OverrideScore = p.OverrideScore;
      Text = p.Text;
      FlavorText = p.FlavorText;
      Illustration = p.Illustration;
      MayChooseNotToUntap = p.MayChooseToUntap;

      _power = new Power(p.Power);
      _toughness = new Toughness(p.Toughness);
      _level = new Level(p.IsLeveler ? 0 : (int?) null);
      _counters = new Counters.Counters(_power, _toughness);
      _type = new CardTypeCharacteristic(p.Type);
      _colors = new CardColors(p.Colors);

      _protections = p.Protections;
      _damagePreventions = p.DamagePreventions;

      _staticAbilities = p.StaticAbilities;
      _triggeredAbilities = p.TriggeredAbilities;
      _activatedAbilities = p.ActivatedAbilities;
      _castInstructions = p.CastInstructions;
      _continuousEffects = p.ContinuousEffects;

      JoinedBattlefield = new TrackableEvent(this);
      LeftBattlefield = new TrackableEvent(this);
    }

    public bool MayChooseNotToUntap { get; private set; }
    public Card AttachedTo { get { return _attachedTo.Value; } private set { _attachedTo.Value = value; } }
    public IEnumerable<Card> Attachments { get { return _attachments.Cards; } }

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
              !Has().CannotAttack;
      }
    }

    private int UsageScore { get { return _usageScore.Value; } set { _usageScore.Value = value; } }
    public bool HasFirstStrike { get { return Has().FirstStrike || Has().DoubleStrike; } }
    public bool HasNormalStrike { get { return !Has().FirstStrike || Has().DoubleStrike; } }
    public bool CanBeTapped { get { return IsPermanent && !IsTapped; } }
    public bool CanRegenerate { get { return _canRegenerate.Value; } set { _canRegenerate.Value = value; } }
    public bool CanTap { get { return !IsTapped && (!HasSummoningSickness || !Is().Creature || Has().Haste); } }
    public bool IsPermanent { get { return Zone == Zone.Battlefield; } }
    public int CharacterCount { get { return FlavorText.CharacterCount + Text.CharacterCount; } }
    public int CountersCount { get { return _counters.Count; } }
    public CardColor[] Colors { get { return _colors.ToArray(); } }
    public Player Controller { get { return _controller.Value; } }
    public Player Owner { get; private set; }
    public int? Counters { get { return _counters.Count; } }
    public int Damage { get { return _damage.Value; } protected set { _damage.Value = value; } }
    public CardText FlavorText { get; private set; }
    public bool HasAttachments { get { return _attachments.Count > 0; } }
    public bool HasLeathalDamage { get { return _hasLeathalDamage.Value; } }
    public bool HasSummoningSickness { get { return _hasSummoningSickness.Value; } set { _hasSummoningSickness.Value = value; } }
    public bool HasXInCost { get { return _castInstructions.HasXInCost; } }
    public string Illustration { get; private set; }
    public bool IsAttached { get { return AttachedTo != null; } }
    public bool IsAttacker { get { return Combat.IsAttacker(this); } }
    public bool HasBlockers { get { return Combat.HasBlockers(this); } }
    public bool IsBlocker { get { return Combat.IsBlocker(this); } }
    public bool IsTapped { get { return _isTapped.Value; } protected set { _isTapped.Value = value; } }

    public IManaAmount ManaCost { get; private set; }

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
        score = score - UsageScore;


        // auras controlled by other player are added to their score
        if (IsPermanent && Is().Aura && AttachedTo.Controller != Controller)
        {
          return -score;
        }

        return score;
      }
    }

    public CardText Text { get; private set; }

    public int? Toughness { get { return _toughness.Value; } }
    public string Type { get { return _type.Value.ToString(); } }
    public Zone Zone { get { return _zone.Current; } }
    public Zone PreviousZone { get { return _zone.Previous; } }

    public int? Level { get { return _level.Value; } }
    public bool CanBeDestroyed { get { return !CanRegenerate && !Has().Indestructible; } }
    public ScoreOverride OverrideScore { get; private set; }
    public bool IsVisibleInUi { get { return _isPreview || IsVisibleToPlayer(Players.Human); } }
    public bool IsVisible { get { return Ai.IsSearchInProgress ? IsVisibleToPlayer(Players.Searching) : IsVisibleToPlayer(Controller); } }
    public bool IsMultiColored { get { return _colors.Count > 1; } }
    public bool HasChangedZoneThisTurn { get { return _zone.HasChangedZoneThisTurn; } }

    public void DealDamage(Damage.Damage damage)
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

    public int CalculateHash(HashCalculator calc)
    {
      if (_hash.Value.HasValue == false)
      {
        if (!IsVisible)
        {
          _hash.Value = calc.Calculate(_zone);
        }
        else
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
            CanRegenerate.GetHashCode(),
            HasLeathalDamage.GetHashCode(),
            Power.GetHashCode(),
            Toughness.GetHashCode(),
            Level.GetHashCode(),
            Counters.GetHashCode(),
            Type.GetHashCode(),
            _isRevealed.Value.GetHashCode(),
            _isPeeked.Value.GetHashCode(),
            calc.Calculate(_staticAbilities),
            calc.Calculate(_triggeredAbilities),
            calc.Calculate(_activatedAbilities),
            calc.Calculate(_protections),
            calc.Calculate(_damagePreventions),
            calc.Calculate(_attachments),
            calc.Calculate(_zone),
            calc.Calculate(_colors)
            );
        }
      }

      return _hash.Value.GetValueOrDefault();
    }

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

    public Card Initialize(Player owner, Game game)
    {
      Game = game;
      Owner = owner;

      _controller = new ControllerCharacteristic(owner);
      _controller.Initialize(game, this);
      _power.Initialize(game, this);
      _toughness.Initialize(game, this);
      _level.Initialize(game, this);
      _counters.Initialize(ChangeTracker, this);
      _type.Initialize(game, this);
      _colors.Initialize(game, this);
      _protections.Initialize(ChangeTracker, this);
      _zone.Initialize(this, game);
      _modifiers.Initialize(ChangeTracker);

      _isTapped.Initialize(ChangeTracker, this);
      _attachedTo.Initialize(ChangeTracker, this);
      _attachments.Initialize(game, this);
      _canRegenerate.Initialize(ChangeTracker, this);
      _damage.Initialize(ChangeTracker, this);
      _hasLeathalDamage.Initialize(ChangeTracker, this);
      _hasSummoningSickness.Initialize(ChangeTracker, this);
      _hash.Initialize(ChangeTracker);
      _isHidden.Initialize(ChangeTracker, this);
      _isRevealed.Initialize(ChangeTracker, this);
      _isPeeked.Initialize(ChangeTracker, this);
      _usageScore.Initialize(ChangeTracker, this);
      _damagePreventions.Initialize(this, game, this);
      _castInstructions.Initialize(this, game);
      _staticAbilities.Initialize(ChangeTracker, this);
      _triggeredAbilities.Initialize(this, game);
      _activatedAbilities.Initialize(this, game);
      _continuousEffects.Initialize(this, game, this);

      _isPreview = false;

      JoinedBattlefield.Initialize(ChangeTracker);
      LeftBattlefield.Initialize(ChangeTracker);

      return this;
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
      UsageScore += Turn.TurnCount;
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

      if (Has().Fear && !card.HasColor(CardColor.Black) && !card.Is().Artifact)
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

    public bool CanBeTargetBySpellsWithColor(CardColor color)
    {
      return !Has().Shroud && !Has().Hexproof && !HasProtectionFrom(color);
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

    public bool IsGoodTarget(ITarget target)
    {
      return _castInstructions.IsGoodTarget(target);
    }

    public List<ActivationPrerequisites> CanCast()
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

    public void EnchantWithoutPayingCost(Card target)
    {
      var activationParameters = new ActivationParameters
        {
          PayCost = false,
          SkipStack = true
        };

      activationParameters.Targets.Effect.Add(target);

      _castInstructions.Cast(0, activationParameters);
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

    public bool HasProtectionFrom(CardColor color)
    {
      return _protections.HasProtectionFrom(color);
    }

    public bool HasProtectionFrom(IEnumerable<CardColor> colors)
    {
      return colors.Any(x => _protections.HasProtectionFrom(x));
    }

    public bool HasProtectionFrom(Card card)
    {
      return HasProtectionFrom(card._colors) ||
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
      Combat.Remove(this);
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
      _zone.ChangeZone(newZone);
    }

    public void OnCardLeftBattlefield()
    {
      Combat.Remove(this);

      DetachAttachments();
      Detach();
      Untap();
      ClearDamage();

      HasSummoningSickness = false;

      LeftBattlefield.Raise();
    }

    public void Tap()
    {
      IsTapped = true;
      UsageScore += ScoreCalculator.CalculateTapPenalty(this, Turn);

      Publish(new PermanentGetsTapped {Permanent = this});
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
      JoinedBattlefield.Raise();
    }

    public void PutToGraveyard()
    {
      Owner.PutCardToGraveyard(this);
    }

    public IManaAmount GetSpellManaCost(int index)
    {
      return _castInstructions.GetManaCost(index);
    }
  }
}