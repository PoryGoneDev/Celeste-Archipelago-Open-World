using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modDashSwitch : modItemBase
    {
        public override void Load()
        {
            On.Celeste.DashSwitch.Update += modDashSwitch_Update;
            On.Celeste.DashSwitch.OnDashed += modDashSwitch_OnDashed;
        }

        public override void Unload()
        {
            On.Celeste.DashSwitch.Update -= modDashSwitch_Update;
            On.Celeste.DashSwitch.OnDashed -= modDashSwitch_OnDashed;
        }

        private static void modDashSwitch_Update(On.Celeste.DashSwitch.orig_Update orig, DashSwitch self)
        {
            if (!HaveReceived())
            {
                self.sprite.Play("idle", true);
            }

            orig(self);
        }

        private static DashCollisionResults modDashSwitch_OnDashed(On.Celeste.DashSwitch.orig_OnDashed orig, DashSwitch self, Player player, Microsoft.Xna.Framework.Vector2 direction)
        {
            if (HaveReceived())
            {
                return orig(self, player, direction);
            }
            return DashCollisionResults.NormalCollision;
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1201C);
        }
    }
}
