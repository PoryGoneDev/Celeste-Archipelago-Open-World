using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Monocle;
using MonoMod.RuntimeDetour;

using Celeste.Mod.Celeste_Multiworld.UI;

namespace Celeste.Mod.Celeste_Multiworld.General
{

    public class RoomDisplay : Monocle.Entity
    {
        public static string CurrentRoom = "";
        public static int RoomDisplayTimer = 0;

        public RoomDisplay()
        {
            base.Y = 196f;
            base.Depth = -101;
            base.Tag = Tags.HUD | Tags.Global | Tags.PauseUpdate | Tags.TransitionUpdate;
        }

        public override void Render()
        {
            if (!Celeste_MultiworldModule.Settings.RoomPopups)
            {
                return;
            }

            if (SaveData.Instance == null || SaveData.Instance.CurrentSession_Safe == null)
            {
                return;
            }

            if (SaveData.Instance.CurrentSession_Safe.Level != CurrentRoom && SaveData.Instance.CurrentSession_Safe.Level != "")
            {
                CurrentRoom = SaveData.Instance.CurrentSession_Safe.Level;

                RoomDisplayTimer = 180;
            }

            if (RoomDisplayTimer >= 0)
            {
                float alpha = 1.0f;

                if (RoomDisplayTimer > 150)
                {
                    alpha = (float)(180.0f - RoomDisplayTimer) / 30.0f;
                }
                else if (RoomDisplayTimer < 30)
                {
                    alpha = (float)(RoomDisplayTimer) / 30.0f;
                }

                Color TextColor = new Color(0.96078f, 0.25882f, 0.78431f, alpha);

                ActiveFont.Draw($"Room: {CurrentRoom}", new Vector2(50f, 1030f), new Vector2(0.0f, 0.5f), Vector2.One, TextColor, 5.0f, new Color(0.0f, 0.0f, 0.0f, alpha), 0.0f, new Color(1.0f, 0.0f, 0.0f, 0.0f));
                RoomDisplayTimer--;
            }
        }
    }

    internal class modPlayer
    {
        public static int HairLength = 4;

        public static Hook Hook_DuckingGet;
        public static Hook Hook_DuckingSet;

        public void Load()
        {
            On.Celeste.Player.Die += modPlayer_Die;
            On.Celeste.Player.Update += modPlayer_Update;
            On.Celeste.Player.UpdateSprite += modPlayer_UpdateSprite;
            On.Celeste.Player.CorrectDashPrecision += modPlayer_CorrectDashPrecision;
            On.Celeste.Player.SuperJump += modPlayer_SuperJump;
            On.Celeste.Player.ClimbCheck += modPlayer_ClimbCheck;
            On.Celeste.Player.ClimbJump += modPlayer_ClimbJump;
            On.Celeste.Player.Duck += modPlayer_Duck;
            On.Celeste.PlayerSeeker.Update += modPlayerSeeker_Update;
            On.Celeste.Level.LoadLevel += modLevel_LoadLevel;
            On.Celeste.Level.CompleteArea_bool_bool_bool += modLevel_CompleteArea_bool_bool_bool;

            Hook_DuckingGet = new Hook(typeof(Player).GetProperty("Ducking").GetGetMethod(), modPlayer_DuckingGet);
            Hook_DuckingSet = new Hook(typeof(Player).GetProperty("Ducking").GetSetMethod(), modPlayer_DuckingSet);
        }

        public void Unload()
        {
            On.Celeste.Player.Die -= modPlayer_Die;
            On.Celeste.Player.Update -= modPlayer_Update;
            On.Celeste.Player.UpdateSprite -= modPlayer_UpdateSprite;
            On.Celeste.Player.CorrectDashPrecision -= modPlayer_CorrectDashPrecision;
            On.Celeste.Player.SuperJump -= modPlayer_SuperJump;
            On.Celeste.Player.ClimbCheck -= modPlayer_ClimbCheck;
            On.Celeste.Player.ClimbJump -= modPlayer_ClimbJump;
            On.Celeste.Player.Duck -= modPlayer_Duck;
            On.Celeste.PlayerSeeker.Update -= modPlayerSeeker_Update;
            On.Celeste.Level.LoadLevel -= modLevel_LoadLevel;
            On.Celeste.Level.CompleteArea_bool_bool_bool -= modLevel_CompleteArea_bool_bool_bool;

            Hook_DuckingGet.Dispose();
            Hook_DuckingSet.Dispose();
        }

        private static PlayerDeadBody modPlayer_Die(On.Celeste.Player.orig_Die orig, Player self, Microsoft.Xna.Framework.Vector2 direction, bool evenIfInvincible, bool registerDeathInStats)
        {
            Follower goldenStrawb = null;
            if (ArchipelagoManager.Instance.GoldenAmnesty != 1 && (ArchipelagoManager.Instance.GoldenDeathsCounted + 1 < ArchipelagoManager.Instance.GoldenAmnesty) && !SaveData.Instance.Assists.Invincible)
            {
                foreach (Follower follower in self.Leader.Followers)
                {
                    if (follower.Entity is Strawberry && (follower.Entity as Strawberry).Golden && !(follower.Entity as Strawberry).Winged)
                    {
                        goldenStrawb = follower;
                        break;
                    }
                }
                if (goldenStrawb != null)
                {
                    self.Leader.Followers.Remove(goldenStrawb);
                }
            }

            PlayerDeadBody result = orig(self, direction, evenIfInvincible, registerDeathInStats);

            if (registerDeathInStats && !SaveData.Instance.Assists.Invincible)
            {
                ArchipelagoManager.Instance.SendDeathLinkIfEnabled("couldn't climb the mountain");

                if (goldenStrawb != null)
                {
                    ArchipelagoManager.Instance.GoldenDeathsCounted++;
                    if (ArchipelagoManager.Instance.GoldenDeathsCounted >= ArchipelagoManager.Instance.GoldenAmnesty)
                    {
                        ArchipelagoManager.Instance.GoldenDeathsCounted = 0;
                    }
                }
            }

            Items.Traps.TrapManager.Instance.AddDeathToActiveTraps();

            return result;
        }

        private static void modPlayer_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            if (Items.Traps.TrapManager.Instance.IsTrapActive(Items.Traps.TrapType.Stun))
            {
                self.Speed = Vector2.Zero;
                self.StateMachine.state = 0;
            }
            else
            {
                if (!Celeste_MultiworldModule.SaveData.Crouch)
                {
                    self.Ducking = false;
                }

                orig(self);

                if (!Celeste_MultiworldModule.SaveData.Crouch)
                {
                    self.Ducking = false;
                }
            }

            if (!Celeste_MultiworldModule.SaveData.Crouch && self.onGround && Input.MoveY == 1.0f && Math.Abs(self.Sprite.Scale.X - 1.37f) < 0.01f && Math.Abs(self.Sprite.Scale.Y - 0.63f) < 0.01f)
            {
                self.Sprite.Scale = new Vector2(1.0f, 1.0f);
            }

            HandleMessageQueue(self);

            if (Items.Traps.TrapManager.Instance.IsTrapActive(Items.Traps.TrapType.Bald) && self.Sprite != null)
            {
                self.Sprite.HairCount = 0;
            }
            else if (self.Sprite != null)
            {
                self.Sprite.HairCount = self.StateMachine.state == 19 ? (int)(modPlayer.HairLength * 1.75f) : modPlayer.HairLength;
            }

            if (ArchipelagoManager.Instance.DeathLinkData != null)
            {
                if (self.InControl && !self.Dead && !(self.Scene as Level).InCredits)
                {
                    if (ArchipelagoManager.Instance.DeathLinkReceiptStyle == 0)
                    {
                        self.Die(Vector2.Zero, true, false);
                    }
                    else if(ArchipelagoManager.Instance.DeathLinkReceiptStyle == 1)
                    {
                        Action action = delegate
                        {
                            Engine.Scene = new LevelExit(LevelExit.Mode.Restart, self.level.Session, null);
                        };
                        self.level.DoScreenWipe(false, action, false);
                    }

                    ArchipelagoManager.Instance.ClearDeathLink();
                }
            }

            if (self.InControl && !self.Dead)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";
                Items.Traps.TrapManager.Instance.AddScreenToActiveTraps(AP_ID);

                ArchipelagoManager.Instance.SetRoomStorage(AP_ID);

                if (ArchipelagoManager.Instance.Roomsanity)
                {
                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(AP_ID);
                }
            }
            else if (!self.InControl)
            {
                if (SaveData.Instance.CurrentSession_Safe.Area.ID == 8 && (self.Scene as Level).Completed)
                {
                    ArchipelagoManager.Instance.UpdateGameStatus(Archipelago.MultiClient.Net.Enums.ArchipelagoClientState.ClientGoal);
                }
            }

            if ($"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}" == "10_0_f-door")
            {
                if (self.Position.X < 18980)
                {
                    self.Position.X = 18980;
                }
            }
        }

        private static void modPlayer_UpdateSprite(On.Celeste.Player.orig_UpdateSprite orig, Player self)
        {
            orig(self);

            if (Items.Traps.TrapManager.Instance.IsTrapActive(Items.Traps.TrapType.Tiny))
            {
                if (self.Ducking)
                {
                    self.Sprite.Scale = new Vector2(0.32f, 0.48f);
                }
                else
                {
                    self.Sprite.Scale = new Vector2(0.4f, 0.4f);
                }
            }
        }

        private static Vector2 modPlayer_CorrectDashPrecision(On.Celeste.Player.orig_CorrectDashPrecision orig, Player self, Vector2 dir)
        {
            // TODO: Check for Boosters
            dir = orig(self, dir);

            if (!CanDash(dir) && !self.level.InCredits && (self.CurrentBooster == null))
            {
                dir = Vector2.Zero;
            }

            return dir;
        }

        private static void modPlayer_SuperJump(On.Celeste.Player.orig_SuperJump orig, Player self)
        {
            Vector2 dir = Vector2.Zero;

            dir.X = (int)self.Facing;
            dir.Y = self.Ducking ? 1.0f : 0.0f;

            if (CanDash(dir) || self.level.InCredits)
            {
                orig(self);
            }
            else
            {
                self.Jump();
            }
        }

        private static bool modPlayer_ClimbCheck(On.Celeste.Player.orig_ClimbCheck orig, Player self, int dir, int yAdd)
        {
            if (!CanClimb(self.Facing) && !self.level.InCredits)
            {
                return false;
            }

            return orig(self, dir, yAdd);
        }

        private static void modPlayer_ClimbJump(On.Celeste.Player.orig_ClimbJump orig, Player self)
        {
            if (CanClimb(self.Facing) || self.level.InCredits)
            {
                orig(self);
            }
            else if (self.DashAttacking && self.SuperWallJumpAngleCheck)
            {
                self.SuperWallJump(-(int)self.Facing);
            }
            else
            {
                self.WallJump(-(int)self.Facing);
            }
        }

        private static void modPlayer_Duck(On.Celeste.Player.orig_Duck orig, Player self)
        {
            if (Celeste_MultiworldModule.SaveData.Crouch)
            {
                orig(self);
            }
        }

        private static bool modPlayer_DuckingGet(Func<Player, bool> orig, Player self)
        {
            if (Celeste_MultiworldModule.SaveData.Crouch)
            {
                return orig(self);
            }
            else
            {
                return false;
            }
        }

        private static void modPlayer_DuckingSet(Action<Player, bool> orig, Player self, bool value)
        {
            if (Celeste_MultiworldModule.SaveData.Crouch)
            {
                orig(self, value);
            }
        }

        private static void modPlayerSeeker_Update(On.Celeste.PlayerSeeker.orig_Update orig, PlayerSeeker self)
        {
            orig(self);

            if (ArchipelagoManager.Instance.Roomsanity)
            {
                if (self.enabled)
                {
                    string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}";
                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(AP_ID);
                }
            }
        }

        private static void modLevel_LoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            // Fake the B Side Crystal Hearts so that the Golden Strawberries are spawned
            Dictionary<int, bool> realBSideHearts = new Dictionary<int, bool>();
            foreach (AreaStats area in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Areas[area.ID];

                // TODO: This causes B Side Crystal Hearts to look as Ghosts in-level
                if (areaData.HasMode(AreaMode.BSide) && areaData.Mode[(int)AreaMode.BSide].MapData.DetectedHeartGem)
                {
                    realBSideHearts.Add(area.ID, area.Modes[(int)AreaMode.BSide].HeartGem);
                    area.Modes[(int)AreaMode.BSide].HeartGem = true;
                }
            }

            orig(self, playerIntro, isFromLoader);

            // Undo faked B Side Crystal Hearts
            foreach (AreaStats area in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Areas[area.ID];

                if (areaData.HasMode(AreaMode.BSide) && areaData.Mode[(int)AreaMode.BSide].MapData.DetectedHeartGem)
                {
                    area.Modes[(int)AreaMode.BSide].HeartGem = realBSideHearts[area.ID];
                }
            }


            if (self.Session.Area.ID == 2 && self.Session.Area.Mode == 0)
            {
                // Always start Old Site A with Mirror Pre-Broken, for Logic reasons
                self.Session.Inventory.DreamDash = true;
            }

            // Pause UI Entities
            if (ArchipelagoManager.Instance.DeathLinkActive && self.Entities.FindFirst<DeathDisplay>() == null)
            {
                self.Entities.Add(new DeathDisplay());
            }
            if (ArchipelagoManager.Instance.GoldenAmnesty > 1 && self.Entities.FindFirst<GoldenDisplay>() == null)
            {
                self.Entities.Add(new GoldenDisplay());
            }
            if (self.Entities.FindFirst<RoomDisplay>() == null)
            {
                self.Entities.Add(new RoomDisplay());
            }

            self.SaveQuitDisabled = true;
        }

        private static ScreenWipe modLevel_CompleteArea_bool_bool_bool(On.Celeste.Level.orig_CompleteArea_bool_bool_bool orig, Level self, bool spotlightWipe, bool skipScreenWipe, bool skipCompleteScreen)
        {
            if (SaveData.Instance != null)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_Clear";
                Celeste_MultiworldModule.SaveData.LevelClearLocations.Add(AP_ID);
            }

            return orig(self, spotlightWipe, skipScreenWipe, skipCompleteScreen);
        }

        private static bool ShouldShowMessage(ArchipelagoMessage message)
        {
            if (message.Type == ArchipelagoMessage.MessageType.Literature)
            {
                return true;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.Chat)
            {
                return Celeste_MultiworldModule.Settings.ChatMessages;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.Server)
            {
                return Celeste_MultiworldModule.Settings.ServerMessages;
            }
            else if (message.Type == ArchipelagoMessage.MessageType.ItemReceive)
            {
                Celeste_MultiworldModuleSettings.ItemReceiveStyle style = Celeste_MultiworldModule.Settings.ItemReceiveMessages;

                if (!message.Strawberry && (message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.None)
                {
                    return true;
                }
                else if ((message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.Non_Strawberry_Progression)
                {
                    return true;
                }
                else if (style > Celeste_MultiworldModuleSettings.ItemReceiveStyle.Progression)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (message.Type == ArchipelagoMessage.MessageType.ItemSend)
            {
                Celeste_MultiworldModuleSettings.ItemSendStyle style = Celeste_MultiworldModule.Settings.ItemSendMessages;

                if ((message.Flags & Archipelago.MultiClient.Net.Enums.ItemFlags.Advancement) != 0 && style > Celeste_MultiworldModuleSettings.ItemSendStyle.None)
                {
                    return true;
                }
                else if (style > Celeste_MultiworldModuleSettings.ItemSendStyle.Progression)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private static bool CanDash(Vector2 dir)
        {
            if (dir.Y < 0.0f)
            {
                if (dir.X < 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.UpLeftDash;
                }
                else if (dir.X > 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.UpRightDash;
                }
                else
                {
                    return Celeste_MultiworldModule.SaveData.UpDash;
                }
            }
            else if (dir.Y > 0.0f)
            {
                if (dir.X < 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.DownLeftDash;
                }
                else if (dir.X > 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.DownRightDash;
                }
                else
                {
                    return Celeste_MultiworldModule.SaveData.DownDash;
                }
            }
            else
            {
                if (dir.X < 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.LeftDash;
                }
                else if (dir.X > 0.0f)
                {
                    return Celeste_MultiworldModule.SaveData.RightDash;
                }
            }
            return false;
        }

        private static bool CanClimb(Facings facing)
        {
            if (facing == Facings.Right)
            {
                return Celeste_MultiworldModule.SaveData.RightClimb;
            }
            else
            {
                return Celeste_MultiworldModule.SaveData.LeftClimb;
            }
        }

        private static void HandleMessageQueue(Player self)
        {
            if (ArchipelagoManager.Instance.MessageLog.Count > 0)
            {
                if (self.Scene.Tracker.GetEntity<modMiniTextbox>() == null)
                {
                    ArchipelagoMessage message = ArchipelagoManager.Instance.MessageLog[0];
                    ArchipelagoManager.Instance.MessageLog.RemoveAt(0);

                    if (ShouldShowMessage(message))
                    {
                        self.Scene.Add(new modMiniTextbox(message.Text, (message.Type == ArchipelagoMessage.MessageType.Literature)));
                        Logger.Verbose("AP", message.Text);
                    }
                }
            }
        }
    }
}
