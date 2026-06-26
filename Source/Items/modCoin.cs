using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public class modCoin : modItemBase
    {
        static Microsoft.Xna.Framework.Color originalColor;
        static bool setColor = false;

        public override void Load()
        {
            On.Celeste.TouchSwitch.Render += modTouchSwitch_Render;
            On.Celeste.TouchSwitch.TurnOn += modTouchSwitch_TurnOn;
            setColor = false;
        }

        public override void Unload()
        {
            On.Celeste.TouchSwitch.Render -= modTouchSwitch_Render;
            On.Celeste.TouchSwitch.TurnOn -= modTouchSwitch_TurnOn;
        }

        private static void modTouchSwitch_Render(On.Celeste.TouchSwitch.orig_Render orig, TouchSwitch self)
        {
            orig(self);

            if (!modCoin.setColor)
            {
                originalColor = self.inactiveColor;
                setColor = true;
            }

            if (!HaveReceived())
            {
                self.inactiveColor = Microsoft.Xna.Framework.Color.DarkMagenta;
            }
            else
            {
                self.inactiveColor = originalColor;
            }
        }
        private static void modTouchSwitch_TurnOn(On.Celeste.TouchSwitch.orig_TurnOn orig, TouchSwitch self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12005);
        }
    }
}
