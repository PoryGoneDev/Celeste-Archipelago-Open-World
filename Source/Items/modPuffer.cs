using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modPuffer : modItemBase
    {
        public override void Load()
        {
            On.Celeste.Puffer.Update += modPuffer_Update;
        }

        public override void Unload()
        {
            On.Celeste.Puffer.Update -= modPuffer_Update;
        }

        private static void modPuffer_Update(On.Celeste.Puffer.orig_Update orig, Puffer self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
            else
            {
                self.Collidable = false;
                self.goneTimer = 2.5f;
                self.state = Puffer.States.Gone;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12015);
        }
    }
}
