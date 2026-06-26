using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using System.IO;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    public class modChapterMenu
    {
        public static Dictionary<string, string> LevelNameToDisplayName = new Dictionary<string, string>()
        {
            { "7a", "The Summit A" },
            { "7b", "The Summit B" },
            { "7c", "The Summit C" },
            { "9a", "Core A" },
            { "9b", "Core B" },
            { "9c", "Core C" },
            { "10a", "Empty Space" },
            { "10b", "Farewell" },
            { "10c", "Farewell Golden" },
            { "poetry", "Poetry Slam" },
        };

        public void Load()
        {
            On.Celeste.OuiChapterPanel.IsStart += modOuiChapterPanel_IsStart;
            On.Celeste.OuiChapterPanel.Reset += modOuiChapterPanel_Reset;
            On.Celeste.OuiChapterPanel.Start += modOuiChapterPanel_Start;
            On.Celeste.OuiChapterPanel.Swap += modOuiChapterPanel_Swap;
            On.Celeste.OuiChapterPanel.UpdateStats += modOuiChapterPanel_UpdateStats;
            On.Celeste.OuiChapterPanel.IncrementStats += modOuiChapterPanel_IncrementStats;
            On.Celeste.OuiChapterPanel.GetModeHeight += modOuiChapterPanel_GetModeHeight;

            On.Celeste.SaveData.SetCheckpoint += modSaveData_SetCheckpoint;

            On.Celeste.Checkpoint.TurnOn += modCheckpoint_TurnOn;

            On.Celeste.AreaComplete.Update += modAreaComplete_Update;
            On.Celeste.Level.CompleteArea_bool_bool_bool += modLevel_CompleteArea_bool_bool_bool;
            On.Celeste.Credits.CreateCredits += modCredits_CreateCredits;
        }

        public void Unload()
        {
            On.Celeste.OuiChapterPanel.IsStart -= modOuiChapterPanel_IsStart;
            On.Celeste.OuiChapterPanel.Reset -= modOuiChapterPanel_Reset;
            On.Celeste.OuiChapterPanel.Start -= modOuiChapterPanel_Start;
            On.Celeste.OuiChapterPanel.Swap -= modOuiChapterPanel_Swap;
            On.Celeste.OuiChapterPanel.UpdateStats -= modOuiChapterPanel_UpdateStats;
            On.Celeste.OuiChapterPanel.GetModeHeight -= modOuiChapterPanel_GetModeHeight;

            On.Celeste.SaveData.SetCheckpoint -= modSaveData_SetCheckpoint;

            On.Celeste.Checkpoint.TurnOn -= modCheckpoint_TurnOn;

            On.Celeste.AreaComplete.Update -= modAreaComplete_Update;
            On.Celeste.Level.CompleteArea_bool_bool_bool -= modLevel_CompleteArea_bool_bool_bool; ;
            On.Celeste.Credits.CreateCredits -= modCredits_CreateCredits;
        }

        private static void modOuiChapterPanel_Start(On.Celeste.OuiChapterPanel.orig_Start orig, OuiChapterPanel self, string checkpoint)
        {
            string AreaName = $"{self.Area.ID}";

            switch (self.Area.Mode)
            {
                case AreaMode.Normal:
                {
                    AreaName += "a";
                    break;
                }
                case AreaMode.BSide:
                {
                    AreaName += "b";
                    break;
                }
                case AreaMode.CSide:
                {
                    AreaName += "c";
                    break;
                }
            }

            bool isGoalArea = (AreaName == ArchipelagoManager.Instance.GoalLevel ||
                              (AreaName == "10a" && (ArchipelagoManager.Instance.GoalLevel == "10b" || ArchipelagoManager.Instance.GoalLevel == "10c")));

            if (!ArchipelagoManager.Instance.ActiveLevels.Contains(AreaName))
            {
                Audio.Play("event:/ui/main/button_back");
            }
            else if (isGoalArea)
            {
                if (!ArchipelagoManager.Instance.LockGoalLevel || (Celeste_MultiworldModule.SaveData.Strawberries >= ArchipelagoManager.Instance.StrawberriesRequired))
                {
                    orig(self, checkpoint);
                }
                else
                {
                    Audio.Play("event:/ui/main/button_back");
                }
            }
            else if (AreaName == "8a")
            {
                if (Celeste_MultiworldModule.SaveData.Strawberries >= ArchipelagoManager.Instance.StrawberriesRequired && Celeste_MultiworldModule.SaveData.GoalItem)
                {
                    orig(self, checkpoint);
                }
                else
                {
                    Audio.Play("event:/ui/main/button_back");
                }
            }
            else if (self.Area.ID == 6 && self.Area.Mode == AreaMode.Normal && checkpoint == null)
            {
                // Special case to skip opening segment of 6A
                orig(self, "00");
            }
            else
            {
                orig(self, checkpoint);
            }
        }

        private static void modOuiChapterPanel_Swap(On.Celeste.OuiChapterPanel.orig_Swap orig, OuiChapterPanel self)
        {
            string AreaName = $"{self.Area.ID}";

            switch (self.Area.Mode)
            {
                case AreaMode.Normal:
                    {
                        AreaName += "a";
                        break;
                    }
                case AreaMode.BSide:
                    {
                        AreaName += "b";
                        break;
                    }
                case AreaMode.CSide:
                    {
                        AreaName += "c";
                        break;
                    }
            }

            if (!ArchipelagoManager.Instance.ActiveLevels.Contains(AreaName))
            {
                Audio.Play("event:/ui/main/button_back");
            }
            else
            {
                orig(self);
            }
        }

        private static bool modSaveData_SetCheckpoint(On.Celeste.SaveData.orig_SetCheckpoint orig, SaveData self, AreaKey area, string level)
        {
            string checkpointString = area.ID.ToString() + "_" + ((int)area.Mode).ToString() + "_" + level;

            if (Celeste_MultiworldModule.SaveData.CheckpointLocations.Contains(checkpointString))
            {
                return false;
            }

            Celeste_MultiworldModule.SaveData.CheckpointLocations.Add(checkpointString);
            return true;
        }

        private static void modCheckpoint_TurnOn(On.Celeste.Checkpoint.orig_TurnOn orig, Checkpoint self, bool animate)
        {
            if (!animate)
            {
                return;
            }

            orig(self, animate);
        }

        private static HashSet<string> modSaveData_GetCheckpoints(On.Celeste.SaveData.orig_GetCheckpoints orig, SaveData self, AreaKey area)
        {
            HashSet<string> checkpoints = new HashSet<string>();

            ModeProperties modeProperties = AreaData.Areas[area.ID].Mode[(int)area.Mode];
            if (modeProperties.Checkpoints != null)
            {
                foreach (CheckpointData checkpointData in modeProperties.Checkpoints)
                {
                    checkpoints.Add(checkpointData.Level);
                }
            }
            checkpoints.RemoveWhere((string a) => !modeProperties.Checkpoints.Any((CheckpointData b) => b.Level == a));

            return checkpoints;
        }

        public static bool modOuiChapterPanel_IsStart(On.Celeste.OuiChapterPanel.orig_IsStart orig, OuiChapterPanel self, Overworld overworld, Overworld.StartMode start)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            if (SaveData.Instance != null && SaveData.Instance.LastArea_Safe.ID == AreaKey.None.ID)
            {
                SaveData.Instance.LastArea_Safe = AreaKey.Default;
                dynamicUI.Set("instantClose", true);
            }
            if (start == Overworld.StartMode.AreaComplete || start == Overworld.StartMode.AreaQuit)
            {
                AreaData areaData = AreaData.Get(SaveData.Instance.LastArea_Safe.ID);
                string text;
                if (areaData == null)
                {
                    text = null;
                }
                else
                {
                    Meta.MapMeta meta = areaData.Meta;
                    text = ((meta != null) ? meta.Parent : null);
                }
                areaData = AreaData.Get(text) ?? areaData;
                if (areaData != null)
                {
                    SaveData.Instance.LastArea_Safe.ID = areaData.ID;
                }
            }

            bool flag = dynamicUI.Invoke<bool>("orig_IsStart", overworld, start);
            if (true && self.option >= self.options.Count && self.options.Count == 1)
            {
                self.AddRemixButton();
            }
            if (true && self.option >= self.options.Count && self.options.Count == 2)
            {
                self.options.Add(new OuiChapterPanel.Option
                {
                    Label = Dialog.Clean("overworld_remix2", null),
                    Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/rmx2")],
                    ID = "C",
                    Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")]
                });
            }
            return true;
        }

        private static void modOuiChapterPanel_Reset(On.Celeste.OuiChapterPanel.orig_Reset orig, OuiChapterPanel self)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);

            self.Area = SaveData.Instance.LastArea_Safe;
            self.Data = AreaData.Areas[self.Area.ID];
            self.RealStats = SaveData.Instance.Areas_Safe[self.Area.ID];
            if (SaveData.Instance.CurrentSession_Safe != null && SaveData.Instance.CurrentSession_Safe.OldStats != null && SaveData.Instance.CurrentSession_Safe.Area.ID == self.Area.ID)
            {
                self.DisplayedStats = SaveData.Instance.CurrentSession_Safe.OldStats;
                SaveData.Instance.CurrentSession_Safe = null;
            }
            else
            {
                self.DisplayedStats = self.RealStats;
            }
            self.height = (float)self.GetModeHeight();
            self.modes.Clear();
            bool flag = false;

            if (!self.Data.Interlude_Safe && self.Area.ID < 10)
            {
                flag = true;
            }

            // A Side Option
            string LevelNameA = $"{self.Area.ID}a";
            string DisplayStringA = Dialog.Clean(self.Data.Interlude_Safe ? "FILE_BEGIN" : "overworld_normal", null).ToUpper();
            bool isGoalArea = (LevelNameA == ArchipelagoManager.Instance.GoalLevel ||
                              (LevelNameA == "10a" && (ArchipelagoManager.Instance.GoalLevel == "10b" || ArchipelagoManager.Instance.GoalLevel == "10c")));
            if (!ArchipelagoManager.Instance.ActiveLevels.Contains(LevelNameA))
            {
                DisplayStringA = "NOT ACTIVE";
            }
            else if (isGoalArea && ArchipelagoManager.Instance.LockGoalLevel)
            {
                DisplayStringA = $"Strawberries: {Celeste_MultiworldModule.SaveData.Strawberries}/{ArchipelagoManager.Instance.StrawberriesRequired}";
            }
            else if (LevelNameA == "8a")
            {
                DisplayStringA = $"{modChapterMenu.LevelNameToDisplayName[ArchipelagoManager.Instance.GoalLevel]} | {Celeste_MultiworldModule.SaveData.Strawberries}/{ArchipelagoManager.Instance.StrawberriesRequired}";
            }
            self.modes.Add(new OuiChapterPanel.Option
            {
                Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")],
                Label = DisplayStringA,
                Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/play")],
                ID = "A"
            });

            // B Side Option
            string LevelNameB = $"{self.Area.ID}b";
            string DisplayStringB = Dialog.Clean("overworld_remix", null);
            if (!ArchipelagoManager.Instance.ActiveLevels.Contains(LevelNameB))
            {
                DisplayStringB = "NOT ACTIVE";
            }
            else if (LevelNameB == ArchipelagoManager.Instance.GoalLevel && ArchipelagoManager.Instance.LockGoalLevel)
            {
                DisplayStringB = $"Strawberries: {Celeste_MultiworldModule.SaveData.Strawberries}/{ArchipelagoManager.Instance.StrawberriesRequired}";
            }
            if (flag)
            {
                OuiChapterPanel.Option option = new OuiChapterPanel.Option
                {
                    Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")],
                    Label = DisplayStringB,
                    Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/remix")],
                    ID = "B",
                };
                self.modes.Insert(1, option);
            }

            // C Side Option
            string LevelNameC = $"{self.Area.ID}c";
            string DisplayStringC = Dialog.Clean("overworld_remix2", null);
            if (!ArchipelagoManager.Instance.ActiveLevels.Contains(LevelNameC))
            {
                DisplayStringC = "NOT ACTIVE";
            }
            else if (LevelNameC == ArchipelagoManager.Instance.GoalLevel && ArchipelagoManager.Instance.LockGoalLevel)
            {
                DisplayStringC = $"Strawberries: {Celeste_MultiworldModule.SaveData.Strawberries}/{ArchipelagoManager.Instance.StrawberriesRequired}";
            }
            bool flag2 = !self.Data.Interlude_Safe && Celeste.PlayMode != Celeste.PlayModes.Event && self.Area.ID < 10;
            if (flag2)
            {
                self.modes.Add(new OuiChapterPanel.Option
                {
                    Bg = GFX.Gui[dynamicUI.Invoke<string>("_ModAreaselectTexture", "areaselect/tab")],
                    Label = DisplayStringC,
                    Icon = GFX.Gui[dynamicUI.Invoke<string>("_ModMenuTexture", "menu/rmx2")],
                    ID = "C"
                });
            }

            self.selectingMode = true;
            self.UpdateStats(false, null, null, null);
            self.SetStatsPosition(false);
            for (int i = 0; i < self.options.Count; i++)
            {
                self.options[i].SlideTowards(i, self.options.Count, true);
            }
            self.chapter = Dialog.Get("area_chapter", null).Replace("{x}", self.Area.ChapterIndex.ToString().PadLeft(2));
            self.contentOffset = new Vector2(440f, 120f);
            self.initialized = true;
        }

        private static void modOuiChapterPanel_UpdateStats(On.Celeste.OuiChapterPanel.orig_UpdateStats orig, OuiChapterPanel self, bool wiggle, bool? overrideStrawberryWiggle, bool? overrideDeathWiggle, bool? overrideHeartWiggle)
        {
            orig(self, wiggle, overrideStrawberryWiggle, overrideDeathWiggle, overrideHeartWiggle);

            self.strawberries.ShowOutOf = self.Area.Mode == AreaMode.Normal;
            self.strawberries.Visible = !self.Data.Interlude_Safe;
            self.deaths.Visible = !self.Data.Interlude_Safe;
        }

        private static System.Collections.IEnumerator modOuiChapterPanel_IncrementStats(On.Celeste.OuiChapterPanel.orig_IncrementStats orig, OuiChapterPanel self, bool shouldAdvance)
        {
            if (SaveData.Instance.Areas_Safe[self.Data.ID].Cassette)
            {
                self.DisplayedStats.Cassette = true;
            }
            return orig(self, shouldAdvance);
        }

        private static int modOuiChapterPanel_GetModeHeight(On.Celeste.OuiChapterPanel.orig_GetModeHeight orig, OuiChapterPanel self)
        {
            AreaModeStats areaModeStats = self.RealStats.Modes[(int)self.Area.Mode];
            bool flag = self.Data.Interlude_Safe;
            if (!self.Data.Interlude_Safe && ((areaModeStats.Deaths > 0 && self.Area.Mode != AreaMode.Normal) || areaModeStats.Completed || areaModeStats.HeartGem))
            {
                flag = false;
            }
            if (!flag)
            {
                return 540;
            }
            return 300;
        }

        private static void modAreaComplete_Update(On.Celeste.AreaComplete.orig_Update orig, AreaComplete self)
        {
            if (!self.Paused)
            {
                self.Entities.Update();
                self.RendererList.Update();
            }

            if (Input.MenuConfirm.Pressed && self.finishedSlide && self.canConfirm)
            {
                self.canConfirm = false;
                if (self.Session.Area.ID == 8 && self.Session.Area.Mode == AreaMode.Normal)
                {
                    new FadeWipe(self, false, delegate
                    {
                        self.Session.Area.ID = 7;
                        self.Session.Level = "credits-summit";
                        self.Session.RespawnPoint = null;
                        self.Session.FirstLevel = false;
                        self.Session.Audio.Music.Event = "event:/music/lvl8/main";
                        self.Session.Audio.Apply(false);
                        Engine.Scene = new LevelLoader(self.Session, null)
                        {
                            PlayerIntroTypeOverride = new Player.IntroTypes?(Player.IntroTypes.None),
                            Level =
                            {
                                new CS07_Credits()
                            }
                        };
                    });
                }
                else
                {
                    HiresSnow outSnow = new HiresSnow(0.45f);
                    outSnow.Alpha = 0f;
                    outSnow.AttachAlphaTo = new FadeWipe(self, false, delegate
                    {
                        Engine.Scene = new OverworldLoader(Overworld.StartMode.AreaComplete, outSnow);
                    });
                    self.RendererList.Add(outSnow);
                }
            }
            self.snow.Alpha = Calc.Approach(self.snow.Alpha, 0f, Engine.DeltaTime * 0.5f);
            self.snow.Direction.Y = Calc.Approach(self.snow.Direction.Y, 1f, Engine.DeltaTime * 24f);
            self.speedrunTimerDelay -= Engine.DeltaTime;
            if (self.speedrunTimerDelay <= 0f)
            {
                self.speedrunTimerEase = Calc.Approach(self.speedrunTimerEase, 1f, Engine.DeltaTime * 2f);
            }
            if (self.title != null)
            {
                self.title.Update();
            }
            if (Celeste.PlayMode == Celeste.PlayModes.Debug)
            {
                if (MInput.Keyboard.Pressed(Keys.F2))
                {
                    Celeste.ReloadAssets(false, true, false, null);
                    Engine.Scene = new LevelExit(LevelExit.Mode.Completed, self.Session, null);
                    return;
                }
                if (MInput.Keyboard.Pressed(Keys.F3))
                {
                    Celeste.ReloadAssets(false, true, true, null);
                    Engine.Scene = new LevelExit(LevelExit.Mode.Completed, self.Session, null);
                }
            }

            MonoMod.Utils.DynamicData dynamicAC = MonoMod.Utils.DynamicData.For(self);
            float buttonTimerDelay = (float)dynamicAC.Get("buttonTimerDelay");
            buttonTimerDelay -= Engine.DeltaTime;
            dynamicAC.Set("buttonTimerDelay", buttonTimerDelay);
            if (buttonTimerDelay <= 0f)
            {
                float buttonTimerEase = (float)dynamicAC.Get("buttonTimerEase");
                buttonTimerEase = Calc.Approach(buttonTimerEase, 1f, Engine.DeltaTime * 4f);
                dynamicAC.Set("buttonTimerEase", buttonTimerEase);
            }
        }

        private static ScreenWipe modLevel_CompleteArea_bool_bool_bool(On.Celeste.Level.orig_CompleteArea_bool_bool_bool orig, Level self, bool spotlightWipe, bool skipScreenWipe, bool skipCompleteScreen)
        {
            self.RegisterAreaComplete();
            self.PauseLock = true;
            Action action;
            if (AreaData.Get(self.Session).ID == 8)
            {
                action = delegate
                {
                    Engine.Scene = new LevelExit(LevelExit.Mode.Completed, self.Session, null);
                };
            }
            else if (AreaData.Get(self.Session).Interlude_Safe || skipCompleteScreen)
            {
                action = delegate
                {
                    Engine.Scene = new LevelExit(LevelExit.Mode.CompletedInterlude, self.Session, self.HiresSnow);
                };
            }
            else
            {
                action = delegate
                {
                    Engine.Scene = new LevelExit(LevelExit.Mode.Completed, self.Session, null);
                };
            }
            if (self.SkippingCutscene || skipScreenWipe)
            {
                Audio.BusStopAll("bus:/gameplay_sfx", true);
                action();
                return null;
            }
            if (spotlightWipe)
            {
                Player entity = self.Tracker.GetEntity<Player>();
                if (entity != null)
                {
                    SpotlightWipe.FocusPoint = entity.Position - self.Camera.Position - new Vector2(0f, 8f);
                }
                return new SpotlightWipe(self, false, action);
            }
            return new FadeWipe(self, false, action);
        }

        private static object modCredits_CreateCredits(On.Celeste.Credits.orig_CreateCredits orig, bool title, bool polaroids)
        {
            List<Credits.CreditNode> credits = (List<Credits.CreditNode>)orig(title, polaroids);

            int insertPoint = polaroids ? credits.Count() - 3 : credits.Count() - 2;

            Credits.Image apImage = new Credits.Image(GFX.Gui, "menu/start");
            Credits.Role apPory = new Credits.Role("PoryGone", new string[] { "Archipelago Implementation" });
            Credits.Role apSterlia = new Credits.Role("Sterlia", new string[] { "Custom Berry Art" });
            Credits.Role apLogic = new Credits.Role("Logic Helpers", new string[] { "BlueRed", "Dorijanko", "LeebyDeeby", "Nitrodon", "NonIdiot", "Seemmetor", "Sun God Moth", "Wither", "Zeeveez", "Zygan" });

            credits.Insert(insertPoint, apImage);
            credits.Insert(insertPoint + 1, apPory);
            credits.Insert(insertPoint + 2, apSterlia);
            credits.Insert(insertPoint + 3, apLogic);

            return credits;
        }
    }
}
