namespace Grove
{
  using System;
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using Grove.AI;
  using Grove.AI.CombatRules;
  using Grove.AI.RepetitionRules;
  using Grove.AI.TimingRules;
  using Modifiers;
  using Triggers;

  public class CardTemplate
  {
    private readonly List<Action<CardParameters>> _init = new List<Action<CardParameters>>();

    public string Name { get; private set; }

    public Card CreateCard()
    {
      var p = new CardParameters();

      foreach (var action in _init)
      {
        action(p);
      }

      if (p.Colors.Count == 0)
      {
        p.Colors.AddRange(GetCardColorsFromManaCost(p.ManaCost));
      }

      if (p.CastInstructions.Count == 0)
      {
        var castParams = GetDefaultCastInstructionParameters(p);
        SetDefaultTimingRules(p, castParams);
        p.CastInstructions.Add(new CastRule(castParams));
      }

      return new Card(p);
    }

    private CastRule.Parameters GetDefaultCastInstructionParameters(CardParameters cp)
    {
      return new CastRule.Parameters
        {
          Cost = new PayMana(cp.ManaCost ?? Mana.Zero, ManaUsage.Spells, cp.HasXInCost),
          Text = string.Format("Cast {0}.", cp.Name),
          Effect = () => new PutIntoPlay(),
        };
    }

    public CardTemplate HasXInCost()
    {
      _init.Add(p => p.HasXInCost = true);
      return this;
    }

    public CardTemplate Protections(CardColor color)
    {
      _init.Add(p => p.ProtectionsFromColors.Add(color));
      return this;
    }

    public CardTemplate Cast(Action<CastRule.Parameters> set)
    {
      _init.Add(cp =>
        {
          var p = GetDefaultCastInstructionParameters(cp);

          set(p);

          p.Text = string.Format(p.Text, cp.Name);

          if (p.HasTimingRules == false)
          {
            SetDefaultTimingRules(cp, p);
          }

          cp.CastInstructions.Add(new CastRule(p));
        }
        );

      return this;
    }

    public CardTemplate CombatRule(Func<CombatRule> combatRule)
    {
      _init.Add(cp => cp.CombatRules.Add(combatRule()));

      return this;
    }

    private static IEnumerable<CardColor> GetCardColorsFromManaCost(IManaAmount manaCost)
    {
      if (manaCost == null)
      {
        yield return CardColor.None;
        yield break;
      }

      if (manaCost.Converted == 0)
      {
        yield return CardColor.Colorless;
        yield break;
      }

      var existing = new HashSet<CardColor>();

      foreach (var mana in manaCost)
      {
        if (mana.Color.IsWhite && !existing.Contains(CardColor.White))
        {
          existing.Add(CardColor.White);
          yield return CardColor.White;
        }

        if (mana.Color.IsBlue && !existing.Contains(CardColor.Blue))
        {
          existing.Add(CardColor.Blue);
          yield return CardColor.Blue;
        }

        if (mana.Color.IsBlack && !existing.Contains(CardColor.Black))
        {
          existing.Add(CardColor.Black);
          yield return CardColor.Black;
        }

        if (mana.Color.IsRed && !existing.Contains(CardColor.Red))
        {
          existing.Add(CardColor.Red);
          yield return CardColor.Red;
        }

        if (mana.Color.IsGreen && !existing.Contains(CardColor.Green))
        {
          existing.Add(CardColor.Green);
          yield return CardColor.Green;
        }
      }

      if (existing.Count == 0)
        yield return CardColor.Colorless;
    }

    private static void SetDefaultTimingRules(CardParameters cp, CastRule.Parameters p)
    {
      if (cp.Type.Creature)
      {
        p.TimingRule(new DefaultCreaturesTimingRule());
      }
      else if (cp.Type.Land)
      {
        p.TimingRule(new DefaultLandsTimingRule());
      }
      else if (cp.Type.Artifact)
      {
        p.TimingRule(new OnFirstMain());
      }
    }

    public CardTemplate Protections(string cardType)
    {
      _init.Add(p => p.ProtectionsFromTypes.Add(cardType));

      return this;
    }

    public CardTemplate Echo(string manaCost)
    {
      var amount = manaCost.Parse();

      TriggeredAbility(p =>
        {
          p.Trigger(new OnStepStart(Step.Upkeep, onlyOnceAfterActivated: true));
          p.Text =
            "At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.";
          p.Effect = () => new PayManaOrSacrifice(amount, "Pay echo?");
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });

      return this;
    }

    public CardTemplate Regenerate(IManaAmount cost, string text)
    {
      return ActivatedAbility(p =>
        {
          p.Text = text;
          p.Cost = new PayMana(cost, ManaUsage.Abilities);
          p.Effect = () => new RegenerateOwner();

          p.TimingRule(new RegenerateSelfTimingRule());
        })
        .CombatRule(() => new RegenerateCombatRule(cost));
    }

    public CardTemplate Pump(IManaAmount cost, string text, int powerIncrease, int toughnessIncrease)
    {
      return ActivatedAbility(p =>
        {
          p.Text = text;
          p.Cost = new PayMana(cost, ManaUsage.Abilities, supportsRepetitions: true);
          p.Effect = () =>
            {
              var effect = new ApplyModifiersToSelf(() => new AddPowerAndToughness(
                powerIncrease, toughnessIncrease) {UntilEot = true});

              if (toughnessIncrease > 0)
                effect.SetTags(EffectTag.IncreaseToughness);

              if (powerIncrease > 0)
                effect.SetTags(EffectTag.IncreasePower);

              return effect;
            };
          p.TimingRule(new PumpOwningCardTimingRule(powerIncrease, toughnessIncrease));
          p.RepetitionRule(new RepeatMaxTimes());
        })
        .CombatRule(() => new PumpCombatRule(powerIncrease, toughnessIncrease, cost));
    }

    public CardTemplate ActivatedAbility(Action<ActivatedAbilityParameters> set)
    {
      _init.Add(cp =>
        {
          var p = new ActivatedAbilityParameters();
          set(p);
          cp.ActivatedAbilities.Add(new ActivatedAbility(p));
        });
      return this;
    }

    public CardTemplate ManaAbility(Action<ManaAbilityParameters> set)
    {
      _init.Add(cp =>
        {
          var p = new ManaAbilityParameters
            {              
              Priority = GetDefaultManaSourcePriority(cp),              
            };

          set(p);

          cp.ActivatedAbilities.Add(new ManaAbility(p));
          cp.ManaColorsThisCardCanProduce.AddRange(p.Colors);
        });
      return this;
    }

    private int GetDefaultManaSourcePriority(CardParameters cp)
    {
      if (cp.Type.Creature)
        return ManaSourcePriorities.Creature;
      if (cp.Type.Land)
        return ManaSourcePriorities.Land;

      return ManaSourcePriorities.Land;
    }

    public CardTemplate StaticAbility(Action<StaticAbilityParamaters> set)
    {
      _init.Add(cp =>
        {
          var p = new StaticAbilityParamaters();
          set(p);
          cp.StaticAbilities.Add(new StaticAbility(p));
        });
      return this;
    }

    public CardTemplate TriggeredAbility(Action<TriggeredAbility.Parameters> set)
    {
      _init.Add(cp =>
        {
          var p = new TriggeredAbility.Parameters();
          set(p);
          cp.TriggeredAbilities.Add(new TriggeredAbility(p));
        });
      return this;
    }

    public CardTemplate SimpleAbilities(params Static[] abilities)
    {
      _init.Add(cp =>
        {
          foreach (var ability in abilities)
          {
            cp.SimpleAbilities.Add(ability);
          }
        });
      return this;
    }

    public CardTemplate ContinuousEffect(Action<ContinuousEffectParameters> set)
    {
      _init.Add(cp =>
        {
          var p = new ContinuousEffectParameters();
          set(p);
          cp.ContinuousEffects.Add(new ContinuousEffect(p));
        });
      return this;
    }

    public CardTemplate Colors(params CardColor[] colors)
    {
      _init.Add(p => p.Colors.AddRange(colors));
      return this;
    }

    public CardTemplate IsLeveler()
    {
      _init.Add(p => { p.IsLeveler = true; });
      return this;
    }

    public CardTemplate FlavorText(string flavorText)
    {
      _init.Add(p => { p.FlavorText = flavorText; });
      return this;
    }

    public CardTemplate ManaCost(string manaCost)
    {
      _init.Add(p => { p.ManaCost = manaCost.Parse(); });
      return this;
    }

    public CardTemplate Named(string name)
    {
      Name = name;
      _init.Add(p => { p.Name = name; });
      return this;
    }

    public CardTemplate Cycling(string cost)
    {
      ActivatedAbility(p =>
        {
          p.Text = string.Format("Cycling {0} ({0}, Discard this card: Draw a card.)", cost);
          p.Cost = new AggregateCost(new PayMana(cost.Parse(), ManaUsage.Abilities), new DiscardThis());
          p.Effect = () => new DrawCards(1);
          p.ActivationZone = Zone.Hand;
          p.TimingRule(new DefaultCyclingTimingRule());
        });

      return this;
    }

    public CardTemplate Power(int power)
    {
      _init.Add(p => { p.Power = power; });
      return this;
    }


    public CardTemplate Text(string text)
    {
      _init.Add(p => { p.Text = text; });
      return this;
    }


    public CardTemplate Toughness(int toughness)
    {
      _init.Add(p => { p.Toughness = toughness; });
      return this;
    }

    public CardTemplate IsUnblockableIfNotBlockedByAtLeast(int count)
    {
      _init.Add(p => { p.MinimalBlockerCount = count; });
      return this;
    }

    public CardTemplate Type(string type)
    {
      _init.Add(p => { p.Type = type; });
      return this;
    }

    public CardTemplate Leveler(string cost, EffectTag tag,
      params LevelDefinition[] levels)
    {
      ActivatedAbility(p =>
        {
          p.Text = String.Format("{0}: Put a level counter on this. Level up only as sorcery.", cost);
          p.Cost = new PayMana(cost.Parse(), ManaUsage.Abilities);
          p.Effect = () => new ApplyModifiersToSelf(() => new IncreaseLevel()).SetTags(tag);
          p.TimingRule(new DefaultLevelUpTimingRule(cost.Parse(), levels));
          p.ActivateAsSorcery = true;
        });

      foreach (var level in levels)
      {
        var lvl = level;
        TriggeredAbility(p =>
          {
            p.Trigger(new OnLevelChanged(lvl.Min));
            p.Effect = () => new ApplyModifiersToSelf(
              () =>
                {
                  var modifier = new AddStaticAbility(lvl.StaticAbility);
                  modifier.AddLifetime(new LevelLifetime(lvl.Min, lvl.Max));
                  return modifier;
                },
              () =>
                {
                  var modifier = new SetPowerAndToughness(lvl.Power, lvl.Toughness);
                  modifier.AddLifetime(new LevelLifetime(lvl.Min, lvl.Max));
                  return modifier;
                }
              );
            p.UsesStack = false;
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
      }

      _init.Add(cp => { cp.IsLeveler = true; });

      return this;
    }

    public CardTemplate MayChooseToUntap()
    {
      _init.Add(p => p.MayChooseToUntap = true);
      return this;
    }

    public CardTemplate OverrideScore(ScoreOverride score)
    {
      _init.Add(p => p.OverrideScore = score);
      return this;
    }
  }
}