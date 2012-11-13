namespace Grove.Core.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class CreateTokens : Effect
  {
    private readonly List<ICardFactory> _tokenFactories = new List<ICardFactory>();

    public Action<Card, Game> AfterTokenComesToPlay = delegate { };
    public Value Count = 1;

    protected override void ResolveEffect()
    {
      for (var i = 0; i < Count.GetValue(X); i++)
      {
        foreach (var tokenFactory in _tokenFactories)
        {
          var token = tokenFactory.CreateCard(Controller, Game);
          token.PutToBattlefield();
          
          AfterTokenComesToPlay(token, Game);
        }
      }
    }

    public void Tokens(params ICardFactory[] tokenFactories)
    {
      _tokenFactories.AddRange(tokenFactories);
    }
  }
}