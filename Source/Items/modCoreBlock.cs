using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modCoreBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.BounceBlock.WindUpPlayerCheck += modBounceBlock_WindUpPlayerCheck;
            On.Celeste.BounceBlock.Render += modBounceBlock_Render;
        }

        public override void Unload()
        {
            On.Celeste.BounceBlock.WindUpPlayerCheck -= modBounceBlock_WindUpPlayerCheck;
            On.Celeste.BounceBlock.Render -= modBounceBlock_Render;
        }

        private static Player modBounceBlock_WindUpPlayerCheck(On.Celeste.BounceBlock.orig_WindUpPlayerCheck orig, BounceBlock self)
        {
            if (HaveReceived())
            {
                return orig(self);
            }

            Player res = orig(self);

            if (res != null)
            {
                self.Break();
            }

            return null;
        }

        private static void modBounceBlock_Render(On.Celeste.BounceBlock.orig_Render orig, BounceBlock self)
        {
            if (!HaveReceived())
            {
                self.hotCenterSprite.Visible = false;
                self.coldCenterSprite.Visible = false;
            }
            else
            {
                self.ToggleSprite();
            }

            orig(self);
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12014);
        }
    }
}
