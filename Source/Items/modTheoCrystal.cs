using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modTheoCrystal : modItemBase
    {
        public override void Load()
        {
            On.Celeste.TheoCrystal.Update += modTheoCrystal_Update;
        }

        public override void Unload()
        {
            On.Celeste.TheoCrystal.Update += modTheoCrystal_Update;
        }

        private static void modTheoCrystal_Update(On.Celeste.TheoCrystal.orig_Update orig, TheoCrystal self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.Collidable = false;
                self.sprite.Color.R = (byte)(0.5f * 255);
                self.sprite.Color.G = (byte)(0.5f * 255);
                self.sprite.Color.B = (byte)(0.5f * 255);
                self.sprite.Color.A = (byte)(0.1f * 255);
                self.Hold.cannotHoldTimer = 1.0f;
            }
            else
            {
                self.Collidable = true;
                self.sprite.Color.R = (byte)255;
                self.sprite.Color.G = (byte)255;
                self.sprite.Color.B = (byte)255;
                self.sprite.Color.A = (byte)255;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1200C);
        }
    }
}
