namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using Effects;

  public class TimingParameters
  {
    public TimingParameters(Game game, Card card, ActivationParameters activation)
    {
      Game = game;
      Card = card;
      Activation = activation;
    }

    public Game Game { get; private set; }
    public Card Card { get; private set; }
    public ActivationParameters Activation { get; private set; }
    public Step Step { get { return Game.Turn.Step; } }
    public Player Controller { get { return Card.Controller; } }
    public Player Opponent { get { return Game.Players.GetOpponent(Controller); } }
    public Effect TopSpell { get { return Game.Stack.TopSpell; } }
    public Player TopSpellController { get { return TopSpell == null ? null : TopSpell.Controller; } }
    public IEnumerable<Attacker> Attackers { get { return Game.Combat.Attackers; } }    
    public bool IsTopSpellTarget { get
    {
      return 
        TopSpell.Target == Activation.EffectTarget ||
          TopSpell.Target == Activation.CostTarget ||
            TopSpell.Target == Card ||
              TopSpell.Target == Card.Controller;
    }}

    public bool IsCannonfodder()
    {
      return Game.Combat.IsBlockerThatWillBeDealtLeathalDamageAndWillNotKillAttacker(Card);
    }
  }
}