using HexedSubworlds.Common.UI;
using Terraria.ModLoader;

namespace HexedSubworlds
{
    public class HSubworldsUI : ILoadable
    {
        public DefaultLoadingUI DefaultLoadingUI;

        public void Load(Mod mod)
        {
            DefaultLoadingUI = new DefaultLoadingUI();
        }

        public void Unload()
        {
            DefaultLoadingUI = null;
        }
    }
}
