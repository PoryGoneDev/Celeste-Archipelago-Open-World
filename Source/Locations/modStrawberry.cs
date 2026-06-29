using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    public class modStrawberry : modLocationBase
    {
        public override void Load()
        {
            On.Monocle.Entity.Awake += modEntity_Awake;
            On.Celeste.Strawberry.ctor += modStrawberry_ctor;
            On.Celeste.Strawberry.Added += modStrawberry_Added;
            On.Celeste.Strawberry.OnPlayer += modStrawberry_OnPlayer;
            On.Celeste.Strawberry.OnCollect += modStrawberry_OnCollect;
            On.Celeste.Player.Added += modPlayer_Added;
            On.Celeste.DetachStrawberryTrigger.DetatchFollower += modDetachStrawberryTrigger_DetatchFollower;
            On.Celeste.SaveData.AddStrawberry_AreaKey_EntityID_bool += modSaveData_AddStrawberry_AreaKey_EntityID_bool;
            On.Celeste.SaveData.CheckStrawberry_EntityID += modSaveData_CheckStrawberry_EntityID;
            On.Celeste.TotalStrawberriesDisplay.Update += modTotalStrawberriesDisplay_Update;
            On.Celeste.StrawberriesCounter.Render += modStrawberriesCounter_Render;
        }

        public override void Unload()
        {
            On.Monocle.Entity.Awake -= modEntity_Awake;
            On.Celeste.Strawberry.ctor -= modStrawberry_ctor;
            On.Celeste.Strawberry.Added -= modStrawberry_Added;
            On.Celeste.Strawberry.OnPlayer -= modStrawberry_OnPlayer;
            On.Celeste.Strawberry.OnCollect -= modStrawberry_OnCollect;
            On.Celeste.Player.Added -= modPlayer_Added;
            On.Celeste.DetachStrawberryTrigger.DetatchFollower -= modDetachStrawberryTrigger_DetatchFollower;
            On.Celeste.SaveData.AddStrawberry_AreaKey_EntityID_bool -= modSaveData_AddStrawberry_AreaKey_EntityID_bool;
            On.Celeste.SaveData.CheckStrawberry_EntityID -= modSaveData_CheckStrawberry_EntityID;
            On.Celeste.TotalStrawberriesDisplay.Update -= modTotalStrawberriesDisplay_Update;
            On.Celeste.StrawberriesCounter.Render -= modStrawberriesCounter_Render;
        }

        private static void modEntity_Awake(On.Monocle.Entity.orig_Awake orig, Monocle.Entity self, Monocle.Scene scene)
        {
            orig(self, scene);

            if (self is Strawberry && ArchipelagoManager.Instance.GoldenAmnesty > 1)
            {
                if (Celeste_MultiworldModule.Session.GoldenStrawberryFollowing.HasValue)
                {
                    if (Celeste_MultiworldModule.Session.GoldenStrawberryFollowing.Value.Equals((self as Strawberry).ID))
                    {
                        Player player = scene.Tracker.GetEntity<Player>();
                        if (player == null)
                        {
                            return;
                        }
                        foreach (Follower follower in player.Leader.Followers)
                        {
                            if (follower.Entity is Strawberry && (follower.Entity as Strawberry).Golden && (follower.Entity as Strawberry).ID.Equals((self as Strawberry).ID))
                            {
                                scene.Remove(self);
                                return;
                            }
                        }
                        (self as Strawberry).OnPlayer(player);
                    }
                }
            }
        }

        private static void modStrawberry_ctor(On.Celeste.Strawberry.orig_ctor orig, Strawberry self, EntityData data, Microsoft.Xna.Framework.Vector2 offset, EntityID gid)
        {
            orig(self, data, offset, gid);

            if (self.Golden)
            {
                if (SaveData.Instance.CurrentSession_Safe.Area.ID == 10)
                {
                    if (ArchipelagoManager.Instance.GoalLevel != "10c")
                    {
                        self.Active = false;
                        self.Visible = false;
                        self.Collidable = false;
                    }
                }
                else if (!ArchipelagoManager.Instance.IncludeGoldens)
                {
                    self.Active = false;
                    self.Visible = false;
                    self.Collidable = false;
                }
            }
        }

        private static void modStrawberry_Added(On.Celeste.Strawberry.orig_Added orig, Strawberry self, Monocle.Scene scene)
        {
            orig(self, scene);

            self.sprite.RemoveSelf();
            self.bloom.RemoveSelf();
            self.light.RemoveSelf();
            self.lightTween.RemoveSelf();
            self.wiggler.RemoveSelf();

            string strawberryString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";
            int strawberryFlags = ArchipelagoManager.Instance.GetFlagsForStrawberry(strawberryString);
            bool isActuallyStrawberry = ArchipelagoManager.Instance.IsActuallyStrawberry(strawberryString);

            if (SaveData.Instance.CheckStrawberry(self.ID))
            {
                if (self.Moon)
                {
                    self.sprite = GFX.SpriteBank.Create("moonghostberry");
                }
                else if (self.Golden)
                {
                    self.sprite = GFX.SpriteBank.Create("goldghostberry");
                }
                else if (isActuallyStrawberry)
                {
                    self.sprite = GFX.SpriteBank.Create("ghostberry");
                }
                else
                {
                    self.sprite = GFX.SpriteBank.Create("ghostprogberry");
                }
                self.sprite.Color = Microsoft.Xna.Framework.Color.White * 0.8f;
            }
            else if (self.Moon)
            {
                self.sprite = GFX.SpriteBank.Create("moonberry");
            }
            else if (self.Golden)
            {
                if (Celeste_MultiworldModule.Settings.ShowBerryLocationClassifications == Celeste_MultiworldModuleSettings.ItemClassificationSpoils.Non_Goldens ||
                    Celeste_MultiworldModule.Settings.ShowBerryLocationClassifications == Celeste_MultiworldModuleSettings.ItemClassificationSpoils.None)
                {
                    strawberryFlags = 0b1;
                }

                if ((strawberryFlags & 0b1) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("goldberry");
                }
                else if ((strawberryFlags & 0b10) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("usefulgoldberry");
                }
                else if ((strawberryFlags & 0b100) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("trapgoldberry");
                }
                else
                {
                    self.sprite = GFX.SpriteBank.Create("fillergoldberry");
                }
            }
            else
            {
                if (Celeste_MultiworldModule.Settings.ShowBerryLocationClassifications == Celeste_MultiworldModuleSettings.ItemClassificationSpoils.Goldens ||
                    Celeste_MultiworldModule.Settings.ShowBerryLocationClassifications == Celeste_MultiworldModuleSettings.ItemClassificationSpoils.None)
                {
                    strawberryFlags = 0b1;
                }

                if (isActuallyStrawberry)
                {
                    self.sprite = GFX.SpriteBank.Create("strawberry");
                }
                else if ((strawberryFlags & 0b1) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("progberry");
                }
                else if ((strawberryFlags & 0b10) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("usefulberry");
                }
                else if ((strawberryFlags & 0b100) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("trapberry");
                }
                else
                {
                    self.sprite = GFX.SpriteBank.Create("fillerberry");
                }
            }
            self.Add(self.sprite);

            if (self.Winged)
            {
                self.sprite.Play("flap", false, false);
            }
            self.sprite.OnFrameChange = new Action<string>(self.OnAnimate);

            self.Add(self.wiggler = Monocle.Wiggler.Create(0.4f, 4f, delegate (float v)
            {
                self.sprite.Scale = Microsoft.Xna.Framework.Vector2.One * (1f + v * 0.35f);
            }, false, false));
            self.Add(self.rotateWiggler = Monocle.Wiggler.Create(0.5f, 4f, delegate (float v)
            {
                self.sprite.Rotation = v * 30f * 0.017453292f;
            }, false, false));

            self.Add(self.bloom = new BloomPoint((self.Golden || self.Moon || self.isGhostBerry) ? 0.3f : 0.6f, 12f));
            self.Add(self.light = new VertexLight(Microsoft.Xna.Framework.Color.White, 1f, 16, 24));
            self.Add(self.lightTween = self.light.CreatePulseTween());
            if (self.Seeds != null && self.Seeds.Count > 0 && !(scene as Level).Session.GetFlag(self.gotSeedFlag))
            {
                self.bloom.Visible = (self.light.Visible = false);
            }
            if ((scene as Level).Session.BloomBaseAdd > 0.1f)
            {
                self.bloom.Alpha *= 0.5f;
            }
        }

        private static void modStrawberry_OnPlayer(On.Celeste.Strawberry.orig_OnPlayer orig, Strawberry self, Player player)
        {
            orig(self, player);

            if (self.Golden)
            {
                Celeste_MultiworldModule.Session.GoldenStrawberryFollowing = self.ID;
            }
        }

        private static void modStrawberry_OnCollect(On.Celeste.Strawberry.orig_OnCollect orig, Strawberry self)
        {
            orig(self);
            string strawberryString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";

            Celeste_MultiworldModule.SaveData.StrawberryLocations.Add(strawberryString);
            //Logger.Verbose("AP", strawberryString);

            if (self.Golden)
            {
                Celeste_MultiworldModule.Session.GoldenStrawberryFollowing = null;
            }
        }

        private static void modPlayer_Added(On.Celeste.Player.orig_Added orig, Player self, Monocle.Scene scene)
        {
            orig(self, scene);
            var following = Celeste_MultiworldModule.Session.GoldenStrawberryFollowing;
            if (following != null)
            {
                var followID = (EntityID)following;
                var data = new EntityData
                {
                    ID = followID.ID,
                    Position = self.Position,
                    Level = self.SceneAs<Level>().Session.LevelData,
                    Name = "goldenBerry"
                };

                scene.Add(new Strawberry(data, Microsoft.Xna.Framework.Vector2.Zero, followID));
                return;
            }
        }

        private static System.Collections.IEnumerator modDetachStrawberryTrigger_DetatchFollower(On.Celeste.DetachStrawberryTrigger.orig_DetatchFollower orig, DetachStrawberryTrigger self, Follower follower)
        {
            yield return new SwapImmediately(orig(self, follower));

            Celeste_MultiworldModule.Session.GoldenStrawberryFollowing = null;

            yield break;
        }

        private static void modSaveData_AddStrawberry_AreaKey_EntityID_bool(On.Celeste.SaveData.orig_AddStrawberry_AreaKey_EntityID_bool orig, SaveData self, AreaKey area, EntityID strawberry, bool golden)
        {
            AreaModeStats areaModeStats = self.Areas_Safe[area.ID].Modes[(int)area.Mode];
            if (!areaModeStats.Strawberries.Contains(strawberry))
            {
                areaModeStats.Strawberries.Add(strawberry);
                areaModeStats.TotalStrawberries += 1;
            }
            Stats.Increment(golden ? Stat.GOLDBERRIES : Stat.BERRIES, 1);
        }

        private static bool modSaveData_CheckStrawberry_EntityID(On.Celeste.SaveData.orig_CheckStrawberry_EntityID orig, SaveData self, EntityID strawberry)
        {
            string AP_ID = $"{self.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{strawberry}";
            return Celeste_MultiworldModule.SaveData.StrawberryLocations.Contains(AP_ID);
        }

        private static void modTotalStrawberriesDisplay_Update(On.Celeste.TotalStrawberriesDisplay.orig_Update orig, TotalStrawberriesDisplay self)
        {
            self.strawberries.showOutOf = true;
            self.strawberries.OutOf = ArchipelagoManager.Instance.StrawberriesRequired;
            orig(self);
        }

        private static void modStrawberriesCounter_Render(On.Celeste.StrawberriesCounter.orig_Render orig, StrawberriesCounter self)
        {
            Microsoft.Xna.Framework.Vector2 vector = self.RenderPosition;
            Microsoft.Xna.Framework.Vector2 vector2 = Monocle.Calc.AngleToVector(self.Rotation, 1f);
            Microsoft.Xna.Framework.Vector2 vector3 = new Microsoft.Xna.Framework.Vector2(-vector2.Y, vector2.X);
            string text = (self.showOutOf ? self.sOutOf : "");
            float num = ActiveFont.Measure(self.sAmount).X;
            float num2 = ActiveFont.Measure(text).X;
            float num3 = 62f + (float)self.x.Width + 2f + num + num2;
            Microsoft.Xna.Framework.Color color = self.Color;
            if (self.flashTimer > 0f && self.Scene != null && self.Scene.BetweenRawInterval(0.05f))
            {
                color = StrawberriesCounter.FlashColor;
            }
            if (self.CenteredX)
            {
                vector -= vector2 * (num3 / 2f) * self.Scale;
            }
            string text2 = (Monocle.Engine.Scene as Level) == null ? (self.Golden ? "collectables/goldberry" : "collectables/strawberry") : "collectables/strawberry_orig";
            GFX.Gui[text2].DrawCentered(vector + vector2 * 60f * 0.5f * self.Scale, Microsoft.Xna.Framework.Color.White, self.Scale);
            self.x.DrawCentered(vector + vector2 * (62f + (float)self.x.Width * 0.5f) * self.Scale + vector3 * 2f * self.Scale, color, self.Scale);
            ActiveFont.DrawOutline(self.sAmount, vector + vector2 * (num3 - num2 - num * 0.5f) * self.Scale + vector3 * (self.wiggler.Value * 18f) * self.Scale, new Microsoft.Xna.Framework.Vector2(0.5f, 0.5f), Microsoft.Xna.Framework.Vector2.One * self.Scale, color, self.Stroke, Microsoft.Xna.Framework.Color.Black);
            if (text != "")
            {
                ActiveFont.DrawOutline(text, vector + vector2 * (num3 - num2 / 2f) * self.Scale, new Microsoft.Xna.Framework.Vector2(0.5f, 0.5f), Microsoft.Xna.Framework.Vector2.One * self.Scale, self.OutOfColor, self.Stroke, Microsoft.Xna.Framework.Color.Black);
            }
        }
    }
}
