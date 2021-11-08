using HexedSubworlds.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HexedSubworlds.Common.Monomod
{
    public class ILDontSaveSubworldsNormally : ILoadable
    {
        public void Load(Mod mod)
        {
            IL.Terraria.IngameOptions.Draw += IngameOptions_Draw;
        }

        private void IngameOptions_Draw(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Find where the Save and Quit method is called in the ingame settings UI
            if (!c.TryGotoNext((instr)
                => instr.MatchCall(typeof(WorldGen).FullName, "SaveAndQuit")
                ))
                throw new Exception("IL Edit " + GetType().Name + " failed; couldn't match opcodes.");

            // Remove it
            c.Remove();

            // By default, it pushes null onto the stack as a parameter. Pop it off
            c.Emit(OpCodes.Pop);

            c.EmitDelegate<Action>(SubworldSystem.AdaptiveExit);
        }

        public void Unload()
        {
        }
    }
}
