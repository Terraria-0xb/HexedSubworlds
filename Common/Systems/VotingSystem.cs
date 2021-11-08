using HexedSubworlds.Common.Configs;
using HexedSubworlds.Core.Voting;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HexedSubworlds.Common.Systems
{
    public class VotingSystem : ModSystem
    {
        public static int VoteTimer;
        public static bool Voting;
        public static Poll Poll;
        public static HashSet<string> PlayersWhoHaveAlreadyVoted;

        public override void OnWorldLoad()
        {
            VoteTimer = -1;
            Voting = false;
            Poll = new Poll();
            PlayersWhoHaveAlreadyVoted = new HashSet<string>();
        }

        public override void OnWorldUnload()
        {
            VoteTimer = -1;
            Voting = false;
            Poll = new Poll();
            PlayersWhoHaveAlreadyVoted = new HashSet<string>();
        }

        public override void PostUpdateEverything()
        {
            // If the vote timer expires or everyone votes
            if (Voting && (--VoteTimer <= 0 || Poll.GetVotesSoFar() == Main.player.Count((p) => p.active)))
            {
                Voting = false;
                VoteTimer = -1;
                PollResult result = Poll.End();

                switch (result)
                {
                    case PollResult.NoVotes:
                        // No one voted :(
                        break;

                    case PollResult.Tie:
                        // Tie, don't enter
                        break;

                    case PollResult.ResultChosen:
                        // ENTER
                        break;
                }
            }
        }

        internal static bool Vote(string player, string option)
        {
            if (PlayersWhoHaveAlreadyVoted.Contains(player))
                return false;

            PlayersWhoHaveAlreadyVoted.Add(player);
            return Poll.VoteFor(option);
        }

        public static void StartVote(string player, string subworld, string key)
        {
            if (Voting || Main.netMode == NetmodeID.SinglePlayer)
                return;

            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key, player, subworld), Main.OurFavoriteColor);
            VoteTimer = HSubworldsConfig.Instance.VotingTime * 60;
            Voting = true;
            PlayersWhoHaveAlreadyVoted = new HashSet<string>();

            Poll.Start("yes, no");
        }
    }
}
