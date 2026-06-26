using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modBumper : modItemBase
    {
        public override void Load()
        {
            On.Celeste.Bumper.Update += modBumper_Update;
        }

        public override void Unload()
        {
            On.Celeste.Bumper.Update -= modBumper_Update;
        }

        private static void modBumper_Update(On.Celeste.Bumper.orig_Update orig, Bumper self)
        {
            if (!HaveReceived())
            {
                self.respawnTimer = 0.6f;
                self.sprite.Play("hit", false, false);
            }

            orig(self);
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1200E);
        }
    }
}
