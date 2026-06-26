using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modWhiteBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.WhiteBlock.Update += modWhiteBlock_Update;
        }

        public override void Unload()
        {
            On.Celeste.WhiteBlock.Update -= modWhiteBlock_Update;
        }

        private static void modWhiteBlock_Update(On.Celeste.WhiteBlock.orig_Update orig, WhiteBlock self)
        {
            if (!HaveReceived())
            {
                self.playerDuckTimer = 0.0f;
                self.sprite.Color = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.sprite.Color = self.enabled ? Microsoft.Xna.Framework.Color.White : Microsoft.Xna.Framework.Color.White * 0.25f;
            }

            orig(self);
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12021);
        }
    }
}
