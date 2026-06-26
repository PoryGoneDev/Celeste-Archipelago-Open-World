using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modMovingPlatform : modItemBase
    {
        public override void Load()
        {
            On.Celeste.MovingPlatform.Update += modMovingPlatform_Update;
        }

        public override void Unload()
        {
            On.Celeste.MovingPlatform.Update -= modMovingPlatform_Update;
        }

        private static void modMovingPlatform_Update(On.Celeste.MovingPlatform.orig_Update orig, MovingPlatform self)
        {
            if (HaveReceived())
            {
                self.Collidable = true;
                orig(self);
            }
            else
            {
                Vector2 offset = new Vector2(Monocle.Calc.Random.NextFloat() * 1.5f, Monocle.Calc.Random.NextFloat() * 1.5f);
                self.Position = self.start + offset;
                self.Collidable = false;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12006);
        }
    }
}
