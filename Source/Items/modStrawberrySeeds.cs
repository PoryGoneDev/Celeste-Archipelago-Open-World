using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modStrawberrySeeds : modItemBase
    {
        public override void Load()
        {
            On.Celeste.StrawberrySeed.Awake += modStrawberrySeed_Awake;
            On.Celeste.StrawberrySeed.Update += modStrawberrySeed_Update;
            On.Celeste.StrawberrySeed.OnPlayer += modStrawberrySeed_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.StrawberrySeed.Awake -= modStrawberrySeed_Awake;
            On.Celeste.StrawberrySeed.Update -= modStrawberrySeed_Update;
            On.Celeste.StrawberrySeed.OnPlayer -= modStrawberrySeed_OnPlayer;
        }

        private static void modStrawberrySeed_Awake(On.Celeste.StrawberrySeed.orig_Awake orig, StrawberrySeed self, Monocle.Scene scene)
        {
            self.level = scene as Level;

            if (self.Components != null)
            {
                foreach (Monocle.Component component in self.Components)
                {
                    component.EntityAwake();
                }
            }

            string strawberryString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.Strawberry.ID}";
            int strawberryFlags = ArchipelagoManager.Instance.GetFlagsForStrawberry(strawberryString);
            bool isActuallyStrawberry = ArchipelagoManager.Instance.IsActuallyStrawberry(strawberryString);

            if (SaveData.Instance.CheckStrawberry(self.Strawberry.ID))
            {
                if (isActuallyStrawberry)
                {
                    self.sprite = GFX.SpriteBank.Create("ghostberrySeed");
                }
                else
                {
                    self.sprite = GFX.SpriteBank.Create("progGhostStrawberrySeed");
                }
                self.sprite.Color = Microsoft.Xna.Framework.Color.White * 0.8f;
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
                    self.sprite = GFX.SpriteBank.Create("strawberrySeed");
                }
                else if ((strawberryFlags & 0b1) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("progStrawberrySeed");
                }
                else if ((strawberryFlags & 0b10) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("usefulStrawberrySeed");
                }
                else if ((strawberryFlags & 0b100) != 0)
                {
                    self.sprite = GFX.SpriteBank.Create("trapStrawberrySeed");
                }
                else
                {
                    self.sprite = GFX.SpriteBank.Create("fillerStrawberrySeed");
                }
            }

            self.sprite.Position = new Microsoft.Xna.Framework.Vector2(self.sine.Value * 2f, self.sine.ValueOverTwo * 1f);
            (self as Monocle.Entity).Add(self.sprite);
            if (self.ghost)
            {
                self.sprite.Color = Microsoft.Xna.Framework.Color.White * 0.8f;
            }
            int num = (self as Monocle.Entity).Scene.Tracker.CountEntities<StrawberrySeed>();
            float num2 = 1f - (float)self.index / ((float)num + 1f);
            num2 = 0.25f + num2 * 0.75f;
            self.sprite.PlayOffset("idle", num2, false);
            self.sprite.OnFrameChange = delegate (string s)
            {
                if (self.Visible && self.sprite.CurrentAnimationID == "idle" && self.sprite.CurrentAnimationFrame == 19)
                {
                    Audio.Play("event:/game/general/seed_pulse", self.Position, "count", (float)self.index);
                    self.lightTween.Start();
                    self.level.Displacement.AddBurst(self.Position, 0.6f, 8f, 20f, 0.2f, null, null);
                }
            };
            StrawberrySeed.P_Burst.Color = self.sprite.Color;

            self.sprite.OnFrameChange = delegate (string s)
            {
                if (HaveReceived() && self.Visible && self.sprite.CurrentAnimationID == "idle" && self.sprite.CurrentAnimationFrame == 19)
                {
                    Audio.Play("event:/game/general/seed_pulse", self.Position, "count", (float)self.index);
                    self.lightTween.Start();
                    self.level.Displacement.AddBurst(self.Position, 0.6f, 8f, 20f, 0.2f, null, null);
                }
            };
        }

        private static void modStrawberrySeed_Update(On.Celeste.StrawberrySeed.orig_Update orig, StrawberrySeed self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.sprite.Position = self.Position;
            }
        }

        private static void modStrawberrySeed_OnPlayer(On.Celeste.StrawberrySeed.orig_OnPlayer orig, StrawberrySeed self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1201F);
        }
    }
}
