using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace HexedSubworlds.Core
{
    public abstract class Subworld : ModType
    {
        /// <summary>
        /// This is the internal ID of this Subworld.
        /// </summary>
        public int Type { get; internal set; }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        protected sealed override void Register()
        {
            ModTypeLookup<Subworld>.Register(this);

            Type = SubworldLoader.NextId;
            SubworldLoader.Subworlds.Add(this);
        }

        public virtual UIState LoadingUI => ModContent.GetInstance<HSubworldsUI>().DefaultLoadingUI;

        /// <summary>
        /// Allows you to modify the seed used when generating your subworld. By default, this is a random number
        /// </summary>
        public virtual int GetSeed() => Main.rand.Next();

        /// <summary>
        /// Return a list of <see cref="GenPass" />es that should be used to generate this subworld.
        /// It is highly recommended to add a <see cref="Passes.Clear"/> (from <c>Terraria.WorldBuilding</c>) as the first gen pass
        /// </summary>
        public abstract List<GenPass> GetGenPasses();


        /// <summary>
        /// This is the last method called during subworld generation.
        /// </summary>
        public virtual void PostWorldGen() { }

        /// <summary>
        /// Called when the subworld is exited.
        /// </summary>
        public virtual void OnQuit() { }
    }
}
