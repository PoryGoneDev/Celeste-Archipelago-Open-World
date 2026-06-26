using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modTrafficBlock : modItemBase
    {
        static Microsoft.Xna.Framework.Color originalColor;
        static bool setColor = false;

        public override void Load()
        {
            On.Celeste.ZipMover.Render += modZipMover_Render;
            On.Celeste.ZipMover.Update += modZipMover_Update;
            setColor = false;
        }

        public override void Unload()
        {
            On.Celeste.ZipMover.Render -= modZipMover_Render;
            On.Celeste.ZipMover.Update -= modZipMover_Update;
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12001);
        }

        private static void modZipMover_Render(On.Celeste.ZipMover.orig_Render orig, ZipMover self)
        {
            orig(self);

            if (!setColor)
            {
                originalColor = self.streetlight.Color;
                setColor = true;
            }

            if (!HaveReceived())
            {
                self.streetlight.Color = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.streetlight.Color = originalColor;
            }
        }

        private static void modZipMover_Update(On.Celeste.ZipMover.orig_Update orig, ZipMover self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
        }
    }
}
