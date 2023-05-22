namespace ScpSpeech
{
    using Exiled.Events.EventArgs.Player;

    public class EventHandlers
    {
        public EventHandlers() 
        {
        }

        public void OnWaitingForPlayers()
        {
            MainPlugin.ScpsToggled.Clear();
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsScp)
                MainPlugin.ScpsToggled.Remove(ev.Player.ReferenceHub);
        }
    }
}
