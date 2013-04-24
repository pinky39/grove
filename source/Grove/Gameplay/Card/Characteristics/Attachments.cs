﻿namespace Grove.Gameplay.Card.Characteristics
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  [Copyable]
  public class Attachments : IHashable
  {
    private readonly TrackableList<Attachment> _attachedCards = new TrackableList<Attachment>();
    
    public IEnumerable<Card> Cards { get { return _attachedCards.Select(x => x.Card); } }

    public int Count { get { return _attachedCards.Count; } }

    public void Initialize(Game game, Card owner)
    {
      _attachedCards.Initialize(game.ChangeTracker, owner);
    }

    public Attachment this[Card card] { get { return _attachedCards.Single(x => x.Card == card); } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_attachedCards);
    }

    public void Add(Attachment attachment)
    {
      _attachedCards.Add(attachment);
    }

    public bool Contains(Card attachment)
    {
      return _attachedCards.Any(x => x.Card == attachment);
    }

    public void Remove(Attachment attachment)
    {
      _attachedCards.Remove(attachment);
    }
  }
}