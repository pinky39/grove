namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using Details.Cards;
  using Details.Cards.Effects;
  using Details.Combat;
  using Targeting;
  using Zones;

  public class TimingParameters
  {
    public TimingParameters(Game game, Card card, ActivationParameters activation = null, bool targetsSelf = false)
    {
      TargetsSelf = targetsSelf;
      Game = game;
      Card = card;
      Activation = activation ?? ActivationParameters.Default;
    }

    public bool TargetsSelf { get; private set; }

    public Game Game { get; private set; }
    public Card Card { get; private set; }
    public ActivationParameters Activation { get; private set; }
    public Step Step { get { return Game.Turn.Step; } }
    public IPlayer Controller { get { return Card.Controller; } }
    public IPlayer Opponent { get { return Game.Players.GetOpponent(Controller); } }
    public Effect TopSpell { get { return Game.Stack.TopSpell; } }
    public IEnumerable<Attacker> Attackers { get { return Game.Combat.Attackers; } }
    public bool IsAttached { get { return Card.IsAttached; } }
    public ITarget Target { get { return Activation.Targets.Effect(0); } }
    public Stack Stack { get { return Game.Stack; } }
    public Combat Combat { get { return Game.Combat; } }

    public bool TopSpellCategoryIs(EffectCategories effectCategories)
    {
      return TopSpell != null && TopSpell.HasCategory(effectCategories);
    }

    public bool IsAttackerWithoutBlockersOrIsAttackerWithTrample(Card card)
    {
      return card.IsAttacker && (!Game.Combat.HasBlockers(card) || card.Has().Trample);
    }

    public bool CanBlockAnyAttacker(Card card)
    {
      return Game.Combat.CanBlockAtLeastOneAttacker(card);
    }
  }
}