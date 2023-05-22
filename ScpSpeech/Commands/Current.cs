namespace ScpSpeech
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;

    public class Current : ICommand
    {
        public string Command { get; set; } = "current";

        public string[] Aliases { get; set; } = { "cu" };

        public string Description { get; set; } = "See the current chat.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);
            response = "Current Chat: Scps.";
            if (MainPlugin.ScpsToggled.Contains(ply.ReferenceHub))
                response = "Current Chat: Proximity.";

            return true;
        }
    }
}