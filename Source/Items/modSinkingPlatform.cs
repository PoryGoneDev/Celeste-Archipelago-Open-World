using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modSinkingPlatform : modItemBase
    {
        public override void Load()
        {
            On.Celeste.SinkingPlatform.Update += modSinkingPlatform_Update;
        }

        public override void Unload()
        {
            On.Celeste.SinkingPlatform.Update -= modSinkingPlatform_Update;
        }

        private static void modSinkingPlatform_Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
        {
            if (HaveReceived())
            {
                self.Collidable = true;
                orig(self);
            }
            else
            {
                Vector2 shakeAmount = new Vector2(Monocle.Calc.Random.NextFloat(), Monocle.Calc.Random.NextFloat());
                self.shaker.On = true;
                self.shaker.Value = shakeAmount;
                self.shaker.ShakeFor(0.1f, false);
                self.Collidable = false;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12020);
        }
    }
}
