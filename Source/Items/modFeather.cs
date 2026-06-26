using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Celeste.Mod.Celeste_Multiworld.Items.modBooster;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modFeather : modItemBase
    {
        public override void Load()
        {
            On.Celeste.FlyFeather.Render += modFlyFeather_Render;
            On.Celeste.FlyFeather.OnPlayer += modFlyFeather_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.FlyFeather.Render -= modFlyFeather_Render;
            On.Celeste.FlyFeather.OnPlayer -= modFlyFeather_OnPlayer;
        }

        private static void modFlyFeather_Render(On.Celeste.FlyFeather.orig_Render orig, FlyFeather self)
        {
            if (!HaveReceived())
            {
                self.sprite.Visible = false;
                self.Collidable = false;
                self.outline.Visible = true;
                self.respawnTimer = 1.0f;
            }

            orig(self);
        }

        private static void modFlyFeather_OnPlayer(On.Celeste.FlyFeather.orig_OnPlayer orig, FlyFeather self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1200D);
        }
    }
}
