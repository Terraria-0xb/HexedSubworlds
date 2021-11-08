using HexedSubworlds.Common.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HexedSubworlds.Common.Monomod
{
    public class OnChangeSaveAndQuitButtonText : ILoadable
    {
        public void Load(Mod mod)
        {
            On.Terraria.IngameOptions.DrawLeftSide += IngameOptions_DrawLeftSide;
        }

        private bool IngameOptions_DrawLeftSide(On.Terraria.IngameOptions.orig_DrawLeftSide orig, Microsoft.Xna.Framework.Graphics.SpriteBatch sb, string txt, int i, Microsoft.Xna.Framework.Vector2 anchor, Microsoft.Xna.Framework.Vector2 offset, float[] scales, float minscale, float maxscale, float scalespeed)
        {
            // Change the option text if you're in a subworld
            if (txt == Lang.inter[35].Value && SubworldSystem.SubworldActive)
                txt = Language.GetTextValue("Mods.HexedSubworlds.UI.SaveAndQuitInSubworld");

            return orig(sb, txt, i, anchor, offset, scales, minscale, maxscale, scalespeed);
        }

        public void Unload()
        {
        }
    }
}
