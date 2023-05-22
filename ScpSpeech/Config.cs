namespace ScpSpeech
{
    using Exiled.API.Interfaces;
    using PlayerRoles;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Whether or not to enable the plugin.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not to enable plugin debug logs.")]
        public bool Debug { get; set; } = false;

        public float MaxDistance { get; set; } = 7;

        public bool UseNoclipButtonFeature { get; set; } = true;

        public string ScpsChatHint { get; set; } = "<b>Chat: <color=red>Scps</color></b>";

        public string ProximityChatHint { get; set; } = "<b>Chat: <color=green>Proximity</color></b>";

        public string Permission { get; set; } = "scpproximity";

        public List<RoleTypeId> AllowedRoleTypes { get; set; } = new()
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp0492
        };
    }
}
