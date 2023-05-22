namespace ScpSpeech
{
    using HarmonyLib;
    using PlayerRoles;
    using PlayerRoles.Voice;
    using VoiceChat.Networking;
    using VoiceChat;
    using PlayerRoles.Spectating;
    using UnityEngine;

    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public class VoicePatch
    {
        public static bool Prefix(Mirror.NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId)
                return false;

            if (msg.Speaker.roleManager.CurrentRole is not IVoiceRole voiceRole)
                return false;

            if (!voiceRole.VoiceModule.CheckRateLimit())
                return false;

            VcMuteFlags flags = VoiceChatMutes.GetFlags(msg.Speaker);
            if (flags is VcMuteFlags.GlobalRegular or VcMuteFlags.LocalRegular)
                return false;

            VoiceChatChannel voiceChatChannel = voiceRole.VoiceModule.ValidateSend(msg.Channel);
            if (voiceChatChannel == VoiceChatChannel.None)
                return false;

            if (voiceChatChannel is VoiceChatChannel.ScpChat)
            {
                if (MainPlugin.ScpsToggled.Contains(msg.Speaker))
                {
                    foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
                    {
                        if (referenceHub == msg.Speaker || referenceHub.roleManager.CurrentRole.Team is Team.SCPs)
                            continue;

                        bool allowSpect = false;
                        if (referenceHub.roleManager.CurrentRole.Team == Team.Dead && msg.Speaker.IsSpectatedBy(referenceHub))
                            allowSpect = true;

                        if (!allowSpect && Vector3.Distance(msg.Speaker.transform.position, referenceHub.transform.position) >= MainPlugin.Instance.Config.MaxDistance)
                            continue;

                        msg.Channel = VoiceChatChannel.Proximity;
                        referenceHub.connectionToClient.Send(msg);
                    }

                    return false;
                }
            }

            voiceRole.VoiceModule.CurrentChannel = voiceChatChannel;
            foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
            {
                if (referenceHub.roleManager.CurrentRole is IVoiceRole voiceRole1)
                {
                    VoiceChatChannel voiceChatChannel1 = voiceRole1.VoiceModule.ValidateReceive(msg.Speaker, voiceChatChannel);
                    if (voiceChatChannel1 is not VoiceChatChannel.None)
                    {
                        msg.Channel = voiceChatChannel1;
                        referenceHub.connectionToClient.Send(msg, 0);
                    }
                }
            }

            return false;
        }
    }
}