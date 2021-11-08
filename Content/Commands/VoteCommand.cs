using HexedSubworlds.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HexedSubworlds.Content.Commands
{
    public class VoteCommand : ModCommand
    {
        public override string Command => "vote";

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            string option = args[0];

            string text;
            Color color;

            if (VotingSystem.Vote(caller.Player.name, option))
            {
                text = Language.GetTextValue("Mods.HexedSubworlds.Vote", option);
                color = Color.LimeGreen;
            }
            else
            {
                text = Language.GetTextValue("Mods.HexedSubworlds.VoteFail");
                color = Color.Red;
            }

            caller.Reply(text, color);
        }
    }
}
