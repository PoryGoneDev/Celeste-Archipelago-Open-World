using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Celeste.Mod.Celeste_Multiworld.Items.modCassetteBlock;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modRefill : modItemBase
    {
        internal enum DashCount
        {
            One = 0xCA12018,
            Two = 0xCA12019,
        }

        public override void Load()
        {
            On.Celeste.Refill.Render += modRefill_Render;
            On.Celeste.Refill.OnPlayer += modRefill_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.Refill.Render -= modRefill_Render;
            On.Celeste.Refill.OnPlayer -= modRefill_OnPlayer;
        }

        private static void modRefill_Render(On.Celeste.Refill.orig_Render orig, Refill self)
        {
            if (!HaveReceived(self.twoDashes ? DashCount.Two : DashCount.One))
            {
                self.outline.Visible = true;
                self.sprite.Visible = false;
                self.Collidable = false;
                self.respawnTimer = 2.5f;
            }
            orig(self);
        }

        private static void modRefill_OnPlayer(On.Celeste.Refill.orig_OnPlayer orig, Refill self, Player player)
        {
            if (HaveReceived(self.twoDashes ? DashCount.Two : DashCount.One))
            {
                orig(self, player);
            }
        }

        internal static bool HaveReceived(DashCount count)
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable((int)count);
        }
    }
}
