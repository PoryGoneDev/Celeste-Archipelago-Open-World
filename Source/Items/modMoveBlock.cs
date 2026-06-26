using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modMoveBlock : modItemBase
    {
        public override void Load()
        {
            On.Celeste.MoveBlock.OnStaticMoverTrigger += modMoveBlock_OnStaticMoverTrigger;
            On.Celeste.MoveBlock.UpdateColors += modMoveBlock_UpdateColors;
            On.Celeste.MoveBlock.MoveCheck += modMoveBlock_MoveCheck;
        }

        public override void Unload()
        {
            On.Celeste.MoveBlock.OnStaticMoverTrigger -= modMoveBlock_OnStaticMoverTrigger;
            On.Celeste.MoveBlock.UpdateColors -= modMoveBlock_UpdateColors;
            On.Celeste.MoveBlock.MoveCheck -= modMoveBlock_MoveCheck;
        }

        private static void modMoveBlock_OnStaticMoverTrigger(On.Celeste.MoveBlock.orig_OnStaticMoverTrigger orig, MoveBlock self, StaticMover sm)
        {
            if (HaveReceived())
            {
                orig(self, sm);
            }
        }

        private static void modMoveBlock_UpdateColors(On.Celeste.MoveBlock.orig_UpdateColors orig, MoveBlock self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
            else
            {
                self.fillColor = MoveBlock.breakingBgFill;
                foreach (Monocle.Image image in self.topButton)
                {
                    image.Color = self.fillColor;
                }
                foreach (Monocle.Image image2 in self.leftButton)
                {
                    image2.Color = self.fillColor;
                }
                foreach (Monocle.Image image3 in self.rightButton)
                {
                    image3.Color = self.fillColor;
                }

                self.state = MoveBlock.MovementState.Idling;
            }
        }

        private static bool modMoveBlock_MoveCheck(On.Celeste.MoveBlock.orig_MoveCheck orig, MoveBlock self, Microsoft.Xna.Framework.Vector2 speed)
        {
            if (HaveReceived())
            {
                return orig(self, speed);
            }
            else
            {
                return true;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12009);
        }
    }
}
