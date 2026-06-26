using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modCloud : modItemBase
    {
        static Microsoft.Xna.Framework.Color originalColor;
        static bool setColor = false;

        internal enum CloudColor
        {
            Blue = 0xCA12008,
            Pink = 0xCA12010,
        }

        public override void Load()
        {
            On.Celeste.Cloud.Render += modCloud_Render;
            On.Celeste.Cloud.Update += modCloud_Update;
        }

        public override void Unload()
        {
            On.Celeste.Cloud.Render -= modCloud_Render;
            On.Celeste.Cloud.Update -= modCloud_Update;
        }

        private static void modCloud_Render(On.Celeste.Cloud.orig_Render orig, Cloud self)
        {
            if (!setColor)
            {
                originalColor = self.sprite.Color;
                setColor = true;
            }

            if (HaveReceived(self.fragile ? CloudColor.Pink : CloudColor.Blue))
            {
                self.sprite.Color = originalColor;
            }
            else
            {
                self.sprite.Color = new Microsoft.Xna.Framework.Color(0.7f, 0.7f, 0.7f, 0.5f);
            }

            orig(self);
        }

        private static void modCloud_Update(On.Celeste.Cloud.orig_Update orig, Cloud self)
        {
            orig(self);

            if (HaveReceived(self.fragile ? CloudColor.Pink : CloudColor.Blue))
            {
                if (!self.fragile || (self.waiting && self.respawnTimer <= 0.0f))
                {
                    self.Collidable = true;
                }
            }
            else
            {
                self.Collidable = false;
            }
        }

        public static bool HaveReceived(CloudColor color)
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable((int)color);
        }
    }
}
