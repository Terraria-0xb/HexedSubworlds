using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace HexedSubworlds.Common.Configs
{
    public class HSubworldsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static HSubworldsConfig Instance => ModContent.GetInstance<HSubworldsConfig>();

        [Label("Mods.HexedSubworlds.Config.VotingTime.Label")]
        [Tooltip("Mods.HexedSubworlds.Config.VotingTime.Tooltip")]
        [Slider]
        [Range(5, 60)]
        [DefaultValue(30)]
        [Increment(1)]
        public int VotingTime;
    }
}
