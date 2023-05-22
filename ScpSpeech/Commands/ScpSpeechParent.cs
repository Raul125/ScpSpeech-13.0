namespace ScpSpeech
{
    using System;
    using CommandSystem;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpSpeechParent : ParentCommand
    {
        public ScpSpeechParent() => LoadGeneratedCommands();

        public override string Command => "scpspeech";

        public override string[] Aliases { get; } = { "scpsp" };

        public override string Description => "Command for switching the scp chat.";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Current());
            RegisterCommand(new Change());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Use .scpsp current/change";
            return true;
        }
    }
}