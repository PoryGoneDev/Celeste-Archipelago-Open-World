using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modBooster : modItemBase
    {
        internal enum BoosterColor
        {
            Blue = 0xCA12007,
            Red = 0xCA1200B,
        }

        public override void Load()
        {
            On.Celeste.Booster.Render += modBooster_Render;
            On.Celeste.Booster.Update += modBooster_Update;
        }

        public override void Unload()
        {
            On.Celeste.Booster.Render -= modBooster_Render;
            On.Celeste.Booster.Update -= modBooster_Update;
        }

        private static void modBooster_Render(On.Celeste.Booster.orig_Render orig, Booster self)
        {
            if (!HaveReceived(self.red ? BoosterColor.Red : BoosterColor.Blue))
            {
                self.sprite.Visible = false;
                self.outline.Visible = true;
                self.respawnTimer = 1.0f;
            }

            orig(self);
        }

        private static void modBooster_Update(On.Celeste.Booster.orig_Update orig, Booster self)
        {
            orig(self);

            self.Ch9HubBooster = false;
            self.Ch9HubTransition = false;

            if (HaveReceived(self.red ? BoosterColor.Red : BoosterColor.Blue))
            {
                self.Collidable = true;
            }
            else
            {
                self.Collidable = false;
            }
        }

        public static bool HaveReceived(BoosterColor color)
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable((int)color);
        }
    }
}
