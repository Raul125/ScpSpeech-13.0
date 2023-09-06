namespace ScpSpeech
{
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using HarmonyLib;
    using PlayerRoles;
    using PlayerRoles.FirstPersonControl;
    using PlayerRoles.FirstPersonControl.NetworkMessages;
    using PlayerStatsSystem;

    [HarmonyPatch(typeof(FpcNoclipToggleMessage), nameof(FpcNoclipToggleMessage.ProcessMessage))]
    public class NoclipVoicePatch
    {
        private static bool Prefix(Mirror.NetworkConnection sender)
        {
            if (!ReferenceHub.TryGetHubNetID(sender.identity.netId, out ReferenceHub referenceHub))
                return false;

            if (MainPlugin.Instance.Config.UseNoclipButtonFeature && Player.TryGet(referenceHub, out Player ply) 
                && ply.Role.Team is Team.SCPs)
            {
                if (ply.CheckPermission(MainPlugin.Instance.Config.Permission) 
                    || MainPlugin.Instance.Config.AllowedRoleTypes.Contains(ply.Role.Type))
                {
                    if (MainPlugin.ScpsToggled.Contains(referenceHub))
                    {
                        ply.ShowHint(MainPlugin.Instance.Config.ScpsChatHint);
                        MainPlugin.ScpsToggled.Remove(referenceHub);
                    }
                    else
                    {
                        ply.ShowHint(MainPlugin.Instance.Config.ProximityChatHint);
                        MainPlugin.ScpsToggled.Add(referenceHub);
                    }
                }
            }

            if (!FpcNoclip.IsPermitted(referenceHub))
                return false;

            if (referenceHub.roleManager.CurrentRole is IFpcRole)
            {
                referenceHub.playerStats.GetModule<AdminFlagsStat>().InvertFlag(AdminFlags.Noclip);
                return false;
            }

            referenceHub.gameConsoleTransmission.SendToClient("Noclip is not supported for this class.", "yellow");
            return false;
        }
    }
}