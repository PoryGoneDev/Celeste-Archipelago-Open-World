using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modSwapBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.SwapBlock.Update += modSwapBlock_Update;
            On.Celeste.SwapBlock.OnDash += modSwapBlock_OnDash;
        }

        public override void Unload()
        {
            On.Celeste.SwapBlock.Update -= modSwapBlock_Update;
            On.Celeste.SwapBlock.OnDash -= modSwapBlock_OnDash;
        }

        private static void modSwapBlock_Update(On.Celeste.SwapBlock.orig_Update orig, SwapBlock self)
        {
            if (!HaveReceived())
            {
                self.redAlpha = 0.0f;
            }

            orig(self);
        }

        private static void modSwapBlock_OnDash(On.Celeste.SwapBlock.orig_OnDash orig, SwapBlock self, Vector2 direction)
        {
            if (HaveReceived())
            {
                orig(self, direction);
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA1200A);
        }
    }
}
