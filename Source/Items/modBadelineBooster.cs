using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modBadelineBooster : modItemBase
    {
        public override void Load()
        {
            On.Celeste.BadelineBoost.OnPlayer += modBadelineBoost_OnPlayer;
            On.Celeste.BadelineBoost.Update += modBadelineBoost_Update;
        }

        public override void Unload()
        {
            On.Celeste.BadelineBoost.OnPlayer -= modBadelineBoost_OnPlayer;
            On.Celeste.BadelineBoost.Update -= modBadelineBoost_Update;
        }

        private static void modBadelineBoost_OnPlayer(On.Celeste.BadelineBoost.orig_OnPlayer orig, BadelineBoost self, Player player)
        {
            if (HaveReceived())
            {
                orig(self, player);
            }
        }

        private static void modBadelineBoost_Update(On.Celeste.BadelineBoost.orig_Update orig, BadelineBoost self)
        {
            if (!HaveReceived())
            {
                self.sprite.Color = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.sprite.Color = Microsoft.Xna.Framework.Color.White;
            }

            orig(self);
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12011);
        }
    }
}
