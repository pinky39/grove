namespace Grove.Modifiers
{
    using System;
    using System.Linq;
    using Events;
    using Infrastructure;

    public class NoPermamentsOnBattlefieldLifetime : Lifetime, IReceive<ZoneChangedEvent>
    {
        private readonly Func<Card, bool> _selector;

        private NoPermamentsOnBattlefieldLifetime() { }

        public NoPermamentsOnBattlefieldLifetime(Func<Card, bool> selector)
        {
            _selector = selector;
        }

        public void Receive(ZoneChangedEvent message)
        {
            if (_selector(message.Card) && Players.Any(p => p.Battlefield.Any(_selector)))
            {
                End();
            }            
        }
    }
}
