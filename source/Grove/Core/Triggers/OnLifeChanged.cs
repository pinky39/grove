namespace Grove.Triggers
{
    using System;
    using Events;
    using Infrastructure;

    class OnLifeChanged : Trigger, IReceive<LifeChangedEvent>
    {
        private readonly Func<TriggeredAbility, Player, bool> _predicate;
        private readonly bool _ifIncreased;

        private OnLifeChanged() {}

        public OnLifeChanged(Func<TriggeredAbility, Player, bool> predicate, bool ifIncreased = true)
        {
            _predicate = predicate;
            _ifIncreased = ifIncreased;
        }

        public void Receive(LifeChangedEvent message)
        {
            if (message.OldValue == message.NewValue)
                return;

            var triggered = _ifIncreased
                ? message.OldValue < message.NewValue   // Player gained life
                : message.OldValue > message.NewValue;  // Player lost life

            if (_predicate(Ability, message.Player) && triggered)
            {
                Set();
            }
        }
    }
}
