using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modCoreToggle : modItemBase
    {
        public override void Load()
        {
            On.Celeste.CoreModeToggle.Update += modCoreModeToggle_Update;
        }

        public override void Unload()
        {
            On.Celeste.CoreModeToggle.Update -= modCoreModeToggle_Update;
        }

        private static void modCoreModeToggle_Update(On.Celeste.CoreModeToggle.orig_Update orig, CoreModeToggle self)
        {
            if (!HaveReceived())
            {
                self.cooldownTimer = 1.6f;
                self.sprite.Play(self.iceMode ? "ice" : "hot", false, false);
            }

            orig(self);
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12013);
        }
    }
}
