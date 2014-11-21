namespace Grove
{
  using System;
  using System.Collections.Generic;
  using AI;
  using AI.CombatRules;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class CardTemplate
  {
    private readonly List<Action<CardParameters>> _init = new List<Action<CardParameters>>();

    public string Name { get; private set; }

    public CardParameters CreateCardParameters()
    {
      var p = new CardParameters(this);

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

      return p;
    }

    public Card CreateCard()
    {
      return new Card(this);
    }

    public CardTemplate HasXInCost()
    {
      _init.Add(p => p.HasXInCost = true);
      return this;
    }

    public CardTemplate MinBlockerPower(int power)
    {
      _init.Add(p => p.MinBlockerPower = power);
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
        p.Trigger(new OnStepStart(Step.Upkeep, onlyOnce: true));
        p.Text =
          "At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.";
        p.Effect = () => new PayManaOrSacrifice(amount, "Pay echo?");
        p.TriggerOnlyIfOwningCardIsInPlay = true;
      });

      SimpleAbilities(Static.Echo);

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
            powerIncrease, toughnessIncrease) { UntilEot = true });

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

    public CardTemplate StaticAbility(Action<StaticAbilityParameters> set)
    {
      _init.Add(cp =>
      {
        var p = new StaticAbilityParameters();
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
      _init.Add(cp => cp.SimpleAbilities.AddRange(abilities));
      return this;
    }

    public CardTemplate ContinuousEffect(Action<ContinuousEffectParameters> set)
    {
      _init.Add(cp =>
      {
        var p = new StaticAbilityParameters { EnabledInAllZones = false };
        p.Modifier(() =>
        {
          var cep = new ContinuousEffectParameters();
          set(cep);

          var effect = new ContinuousEffect(cep);

          return new AddContiniousEffect(effect);
        });
        cp.StaticAbilities.Add(new StaticAbility(p));
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

    public CardTemplate Convoke()
    {
      SimpleAbilities(Static.Convoke);
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

    public CardTemplate Outlast(string cost)
    {
      ActivatedAbility(p =>
      {
        p.Text =
          string.Format("Outlast {0}({0}, {{T}}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.)",
            cost);
        p.Cost = new AggregateCost(new PayMana(cost.Parse(), ManaUsage.Abilities), new Tap());
        p.Effect =
          () =>
            new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1)).SetTags(
              EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        p.ActivateAsSorcery = true;
        p.TimingRule(new PumpOwningCardTimingRule(1, 1));
      });

      return this;
    }

    public CardTemplate Prowess()
    {
      TriggeredAbility(p =>
      {
        p.Text = "Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.";
        p.Trigger(new OnCastedSpell((a, c) =>
          c.Controller == a.OwningCard.Controller && !c.Is().Creature));

        p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) { UntilEot = true })
          .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

        p.TriggerOnlyIfOwningCardIsInPlay = true;
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

    public CardTemplate OverrideScore(Action<ScoreOverride> setOverride)
    {
      var scoreOverride = new ScoreOverride();
      setOverride(scoreOverride);
      _init.Add(p => p.OverrideScore = scoreOverride);
      return this;
    }

    private CastRule.Parameters GetDefaultCastInstructionParameters(CardParameters cp)
    {
      return new CastRule.Parameters
      {
        Cost = new PayMana(cp.ManaCost ?? Mana.Zero, ManaUsage.Spells, cp.HasXInCost),
        Text = string.Format("Cast {0}.", cp.Name),
        Effect = () => new CastPermanent(),
      };
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

    private int GetDefaultManaSourcePriority(CardParameters cp)
    {
      if (cp.Type.Creature)
        return ManaSourcePriorities.Creature;
      if (cp.Type.Land)
        return ManaSourcePriorities.Land;

      return ManaSourcePriorities.Land;
    }
  }
}