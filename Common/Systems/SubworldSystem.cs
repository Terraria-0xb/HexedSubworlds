using HexedSubworlds.Core;
using HexedSubworlds.Core.Generation;
using System;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace HexedSubworlds.Common.Systems
{
    public class SubworldSystem : ModSystem
    {
        public static bool SubworldActive = false;
        public static int ActiveSubworld = -1;

        private static bool oldMapEnabled;

        public static void Enter<T>(string player) where T : Subworld
        {
            if (ModContent.GetInstance<T>().Type == ActiveSubworld)
                return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                throw new Exception("Subworlds must be entered on the server in multiplayer, not the client.");

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                enterSP<T>();
                return;
            }

            // Netmode is server
            VotingSystem.StartVote(player, ModContent.GetInstance<T>().Name, "Mods.HexedSubworlds.Messages.VotingStartedEnter");
        }   

        public static void Exit(string player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                throw new Exception("Subworlds must be exited on the server in multiplayer, not the client.");

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                exitSP(null);
                return;
            }

            // Netmode is server
            VotingSystem.StartVote(player, SubworldLoader.GetByType(ActiveSubworld).Name, "Mods.HexedSubworlds.Messages.VotingStartedExit");
        }

        private static void enterSP<T>() where T : Subworld
        {
            Action callback = () => {
                T t = ModContent.GetInstance<T>();
                Main.gameMenu = true;
                Main.menuMode = 888;
                Main.MenuUI.SetState(t.LoadingUI);

                SubworldGenerator generator = new SubworldGenerator(t);
                generator.GenerateWorld();
                playSubworld(t);
            };

            if (!SubworldActive)
            {
                SystemLoader.PreSaveAndQuit();
                ThreadPool.QueueUserWorkItem(WorldGen.SaveAndQuitCallBack, callback);
            }
            else
                ThreadPool.QueueUserWorkItem(exitSP, callback);
        }

        private static void exitSP(object threadContext)
        {
            SubworldLoader.GetByType(ActiveSubworld).OnQuit();
            Main.menuMode = 888;
            Main.MenuUI.SetState(ModContent.GetInstance<HSubworldsUI>().DefaultLoadingUI);

            SubworldActive = false;
            ActiveSubworld = -1;

            Main.mapEnabled = oldMapEnabled;

            WorldGen.playWorld();

            if (threadContext != null)
                ((Action)threadContext)();
        }

        private static void playSubworld(Subworld s)
        {
            SubworldActive = true;
            ActiveSubworld = s.Type;

            oldMapEnabled = Main.mapEnabled;
            Main.mapEnabled = false;

            if (Main.rand == null)
                Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);

            for (int i = 0; i < 255; i++) {
                if (i != Main.myPlayer)
                    Main.player[i].active = false;
            }

            WorldGen.noMapUpdate = true;

            if (Main.mapEnabled)
                Main.Map.Load();

            if (Main.netMode != 2)
                Main.sectionManager.SetAllFramesLoaded();

            while (Main.loadMapLock) {
                float num = (float)Main.loadMapLastX / (float)Main.maxTilesX;
                Main.statusText = Lang.gen[68].Value + " " + (int)(num * 100f + 1f) + "%";
                Thread.Sleep(0);

                if (!Main.mapEnabled)
                    break;
            }

            if (Main.gameMenu)
                Main.gameMenu = false;

            if (Main.netMode == 0 && Main.anglerWhoFinishedToday.Contains(Main.player[Main.myPlayer].name))
                Main.anglerQuestFinished = true;

            // Move rest of method to main thread to fix concurrent modification exceptions
            Main.OnTickForInternalCodeOnly += finishPlayerSubworld;
        }

        private static void finishPlayerSubworld()
        {
            Main.OnTickForInternalCodeOnly -= finishPlayerSubworld;

            Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
            Main.ActivePlayerFileData.StartPlayTimer();
            Player.Hooks.EnterWorld(Main.myPlayer);
            WorldFile.SetOngoingToTemps();
            Main.resetClouds = true;
            WorldGen.noMapUpdate = false;
        }

        public static void AdaptiveExit()
        {
            // If no subworld is active, leave as normal
            if (!SubworldActive)
            {
                WorldGen.SaveAndQuit(() => {
                    SubworldActive = false;
                    oldMapEnabled = Main.mapEnabled;
                    Main.mapEnabled = false;
                    ActiveSubworld = -1;
                });
            }
            else
                Exit(Main.LocalPlayer.name);
        }

        internal static void LoadSubworldData<T>() where T : Subworld
        {
        }
    }
}
