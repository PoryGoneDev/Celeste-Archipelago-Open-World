using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modFireBall : modItemBase
    {
        public override void Load()
        {
            On.Celeste.FireBall.Update += modFireBall_Update;
        }

        public override void Unload()
        {
            On.Celeste.FireBall.Update -= modFireBall_Update;
        }

        private static void modFireBall_Update(On.Celeste.FireBall.orig_Update orig, FireBall self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.Collidable = false;
                self.Visible = false;
            }
            else
            {
                if (!self.broken)
                {
                    self.Collidable = true;
                    self.Visible = true;
                }
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12012);
        }
    }
}
