namespace Grove.Events
{
    public class LifeChangedEvent
    {
        public readonly Player Player;

        public readonly int OldValue;
        public readonly int NewValue;

        public LifeChangedEvent(Player player, int oldValue)
        {
            Player = player;

            NewValue = player.Life;
            OldValue = oldValue;
        }

        public override string ToString()
        {
            var name = Player.Name == "You" ? "Your" : Player.Name;
            return string.Format("{0} life total is {1}.", name, Player.Life);
        }
    }
}