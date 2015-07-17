namespace Grove.Decisions
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;

  [Copyable, Serializable]
  public class ChosenAttackers : IEnumerable<ChosenAttackers.Attacker>
  {
    private readonly List<Attacker> _attackers = new List<Attacker>();

    public ChosenAttackers() {}

    public ChosenAttackers(IEnumerable<Card> attackers, Card assignedPlaneswalker = null)
    {
      _attackers.AddRange(attackers.Select(c => new Attacker(c, assignedPlaneswalker)));
    }

    public int Count { get { return _attackers.Count; } }
   
    public IEnumerator<Attacker> GetEnumerator()
    {
      return _attackers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Card attacker, Card planeswalker)
    {
      _attackers.Add(new Attacker(attacker, planeswalker));
    }

    public void Remove(Card attacker)
    {
      var found = _attackers.FirstOrDefault(x => x.Card == attacker);
      if (found != null)
      {
        _attackers.Remove(found);
      }
    }

    [Copyable, Serializable]
    public class Attacker : ISerializable
    {
      public readonly Card Card;
      public readonly Card Planeswalker;

      private Attacker() { }

      public Attacker(Card card, Card planeswalker)
      {
        Card = card;
        Planeswalker = planeswalker;
      }

      private Attacker(SerializationInfo info, StreamingContext context)
      {
        var ctx = (SerializationContext)context.Context;

        var cardId = info.GetInt32("card");
        var planesalkerId = (int?)info.GetValue("planeswalker", typeof(int?));

        Card = (Card)ctx.Recorder.GetObject(cardId);

        if (planesalkerId.HasValue)
        {
          Planeswalker = (Card)ctx.Recorder.GetObject(planesalkerId.Value);
        }
      }

      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
        info.AddValue("card", Card.Id);

        var planeswalkerId = Planeswalker == null
          ? (int?)null
          : Planeswalker.Id;

        info.AddValue("planeswalker", planeswalkerId);
      }
    }
  }
}