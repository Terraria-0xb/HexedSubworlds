using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace HexedSubworlds.Core.Generation
{
    public class SubworldGenerator
    {
        public readonly List<GenPass> GenPasses;
        public GenerationProgress Progress;
        public int Seed;
        public Action PostWorldGen;

        public SubworldGenerator(Subworld subworld)
        {
            GenPasses = subworld.GetGenPasses();
            Seed = subworld.GetSeed();
            PostWorldGen = subworld.PostWorldGen;
        }

        public void GenerateWorld()
        {
            Progress = new GenerationProgress();

            WorldGenConfiguration config = WorldGenConfiguration.FromEmbeddedPath("Terraria.GameContent.WorldBuilding.Configuration.json"); ;

            foreach (GenPass pass in GenPasses)
                Progress.TotalWeight += pass.Weight;

            foreach (GenPass pass in GenPasses)
            {
                WorldGen._genRand = new UnifiedRandom(Seed);
                Main.rand = new UnifiedRandom(Seed);

                Progress.Start(pass.Weight);

                try
                {
                    pass.Apply(Progress, config);
                }
                catch (Exception e)
                {
                    string message = string.Join(
                        "\n",
                        Language.GetTextValue("tModLoader.WorldGenError"),
                        pass.Name,
                        e
                    );

                    HexedSubworlds.Log.Error(message);

                    throw;
                }
            }

            PostWorldGen();
        }
    }
}
