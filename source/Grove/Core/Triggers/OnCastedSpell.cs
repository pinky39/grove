namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnCastedSpell : Trigger, IReceive<SpellPutOnStackEvent>
  {
    private readonly CardSelector _filter;

    private OnCastedSpell() {}

    public OnCastedSpell(CardSelector selector = null)
    {
      _filter = selector ?? delegate { return true; };
    }

    public void Receive(SpellPutOnStackEvent message)
    {
      if (_filter(message.Card, Ctx))
      {
        Set(message);
      }
    }
  }
}