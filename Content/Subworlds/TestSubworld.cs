using HexedSubworlds.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace HexedSubworlds.Content.Subworlds
{
    public class TestSubworld : Subworld
    {
        public override List<GenPass> GetGenPasses()
        {
            List<GenPass> passes = new List<GenPass>();

            passes.Add(new Passes.Clear());
            passes.Add(new PassLegacy("dab", new WorldGenLegacyMethod((progress, config) => {
                for (int j = Main.maxTilesY / 2; j < Main.maxTilesY; j++)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        WorldGen.PlaceTile(i, j, TileID.Tungsten);
                    }
                }
            })));

            return passes;
        }
    }
}
