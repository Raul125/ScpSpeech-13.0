namespace ScpSpeech
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using PlayerRoles;

    public class Change : ICommand
    {
        public string Command { get; set; } = "change";

        public string[] Aliases { get; set; } = { "c" };

        public string Description { get; set; } = "Change the chat.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);

            bool isAllowed = false;
            if (ply.CheckPermission(MainPlugin.Instance.Config.Permission))
                isAllowed = true;

            if (MainPlugin.Instance.Config.AllowedRoleTypes.Contains(ply.Role.Type))
                isAllowed = true;

            if (!isAllowed)
            {
                response = "You don't have permissions to use this command!";
                return false;
            }

            if (ply.Role.Team is not Team.SCPs)
            {
                response = "You are not a scp!";
                return false;
            }

            if (MainPlugin.ScpsToggled.Contains(ply.ReferenceHub))
            {
                response = MainPlugin.Instance.Config.ScpsChatHint;
                MainPlugin.ScpsToggled.Remove(ply.ReferenceHub);
            }
            else
            {
                response = MainPlugin.Instance.Config.ProximityChatHint;
                MainPlugin.ScpsToggled.Add(ply.ReferenceHub);
            }

            ply.ShowHint(response);
            return true;
        }
    }
}