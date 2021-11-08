using HexedSubworlds.Common.Systems;
using HexedSubworlds.Content.Subworlds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HexedSubworlds.Content.Items
{
    [Autoload]
    public class TestItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            SubworldSystem.Enter<TestSubworld>(player.name);
            return true;
        }
    }
}
