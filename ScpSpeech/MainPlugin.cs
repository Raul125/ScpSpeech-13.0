namespace ScpSpeech
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using System;
    using System.Collections.Generic;

    public class MainPlugin : Plugin<Config>
    {
        public override string Name => "ScpSpeech";
        public override string Prefix => "scp_speech";
        public override string Author => "Raul125";
        public override Version Version => new(1, 0, 1);
        public override Version RequiredExiledVersion => new(8, 0, 0);
        public override PluginPriority Priority => PluginPriority.Default;

        public static MainPlugin Instance;
        public static List<ReferenceHub> ScpsToggled = new();

        public EventHandlers Handlers;
        public Harmony Harmony;

        public override void OnEnabled()
        {
            Instance = this;

            Handlers = new();
            Exiled.Events.Handlers.Server.WaitingForPlayers += Handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole += Handlers.OnChangingRole;

            Harmony = new Harmony($"{Author}.{Prefix}.{DateTime.Now}");
            Harmony.PatchAll();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole -= Handlers.OnChangingRole;

            Handlers = null;
            Instance = null;

            Harmony.UnpatchAll(Harmony.Id);
            Harmony = null;

            base.OnDisabled();
        }
    }
}
