namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Ai;
  using Cards;
  using Cards.Costs;
  using Cards.Effects;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Cards.Triggers;
  using Dsl;
  using Mana;
  using Targeting;
  using Zones;

  public interface ICardFactory
  {
    string Name { get; }

    Card CreateCard(Player owner, Game game);
    Card CreateCardPreview();
  }

  public class CardFactory : ICardFactory
  {        
    private readonly CardParameters _p = new CardParameters();    
    public string Name { get { return _p.Name; } }

    public Card CreateCard(Player owner, Game game)
    {                        
      return new Card(owner, game, _p);
    }

    public Card CreateCardPreview()
    {          
      return new Card(_p);
    }

    public CardFactory Protections(ManaColors colors)
    {
      _p.ProtectionsFromColors = colors;
      return this;
    }

    public CardFactory Preventions(params IDamagePreventionFactory[] preventions)
    {
      _p.DamagePrevention = preventions;
      return this;
    }

    public CardFactory Protections(params string[] cardTypes)
    {
      _p.ProtectionsFromCardTypes = cardTypes;
      return this;
    }

    public CardFactory Echo(string manaCost)
    {
      var c = new CardBuilder();
      IManaAmount amount = manaCost.ParseMana();

      TriggeredAbility.Factory echoFactory = c.TriggeredAbility(
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

      _p.TriggeredAbilities.Add(echoFactory);
      return this;
    }

    public CardFactory Abilities(params object[] abilities)
    {
      foreach (object ability in abilities)
      {
        if (ability is Static)
        {
          _p.StaticAbilities.Add((Static) ability);
          continue;
        }

        if (ability is IActivatedAbilityFactory)
        {
          _p.ActivatedAbilities.Add(ability as IActivatedAbilityFactory);
          continue;
        }

        if (ability is ITriggeredAbilityFactory)
        {
          _p.TriggeredAbilities.Add(ability as ITriggeredAbilityFactory);
          continue;
        }

        if (ability is IContinuousEffectFactory)
        {
          _p.ContinuousEffects.Add((IContinuousEffectFactory) ability);
        }
      }
      return this;
    }
   
    public CardFactory Cast(Action<CastInstructionParameters> setParameters)
    {      
      var parameters = new CastInstructionParameters(_p.Name, _p.ManaCost, _p.Type);
      setParameters(parameters);      
      _p.CastInstructions.Add(parameters);
      
      return this;
    }

    public CardFactory Colors(ManaColors colors)
    {
      _p.Colors = colors;
      return this;
    }

    public CardFactory IsLeveler()
    {
      _p.Isleveler = true;
      return this;
    }            

    public CardFactory FlavorText(string flavorText)
    {
      _p.FlavorText = flavorText;
      return this;
    }          

    public CardFactory ManaCost(string manaCost)
    {
      _p.ManaCost = manaCost.ParseMana();
      return this;
    }    

    public CardFactory Named(string name)
    {
      _p.Name = name;
      return this;
    }

    public CardFactory Cycling(string cost)
    {
      var b = new CardBuilder();

      IActivatedAbilityFactory cycling = b.ActivatedAbility(
        string.Format("Cycling {0} ({0}, Discard this card: Draw a card.)", cost),
        b.Cost<PayMana, Discard>(c => c.Amount = cost.ParseMana()),
        b.Effect<DrawCards>(e => e.DrawCount = 1),
        timing: Timings.Cycling(),
        activationZone: Zone.Hand);

      _p.ActivatedAbilities.Add(cycling);

      return this;
    }

    public CardFactory Power(int power)
    {
      _p.Power = power;
      return this;
    } 

   

    public CardFactory Text(string text)
    {
      _p.Text = text;
      return this;
    }

  
    public CardFactory Toughness(int toughness)
    {
      _p.Toughness = toughness;
      return this;
    }

    public CardFactory Type(string type)
    {
      _p.Type = type;
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
          builder.Cost<PayMana>(c => c.Amount = cost),
          builder.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(builder.Modifier<IncreaseLevel>())),
          timing: Timings.Leveler(cost, levels), activateAsSorcery: true, category: category));


      foreach (LevelDefinition levelDefinition in levels)
      {
        LevelDefinition definition = levelDefinition;

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


    public CardFactory MayChooseNotToUntapDuringUntap()
    {
      _p.MayChooseNotToUntap = true;
      return this;
    }

    public CardFactory OverrideScore(int score)
    {
      _p.OverrideScore = score;
      return this;
    }
  }
}