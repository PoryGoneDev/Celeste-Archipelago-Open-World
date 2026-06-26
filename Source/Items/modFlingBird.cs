using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modFlingBird : modItemBase
    {
        public override void Load()
        {
            On.Celeste.Player.FinishFlingBird += modPlayer_FinishFlingBird;
            On.Celeste.FlingBird.Render += modFlingBird_Render;
        }

        public override void Unload()
        {
            On.Celeste.Player.FinishFlingBird -= modPlayer_FinishFlingBird;
            On.Celeste.FlingBird.Render -= modFlingBird_Render;
        }

        private static void modPlayer_FinishFlingBird(On.Celeste.Player.orig_FinishFlingBird orig, Player self)
        {
            if (HaveReceived())
            {
                orig(self);
            }
            else
            {
                self.StateMachine.State = 0;
                self.AutoJump = true;
                self.forceMoveX = 1;
                self.forceMoveXTimer = 0.2f;
                self.Speed = new Vector2(0.0f, 400.0f);
                self.varJumpTimer = 0.2f;
                self.varJumpSpeed = self.Speed.Y;
                self.launched = true;
            }
        }

        private static void modFlingBird_Render(On.Celeste.FlingBird.orig_Render orig, FlingBird self)
        {
            orig(self);

            if (!HaveReceived())
            {
                self.sprite.Color.R = (byte)(1.0f * 255.0f);
                self.sprite.Color.G = (byte)(0.0f * 255.0f);
                self.sprite.Color.B = (byte)(0.0f * 255.0f);
                self.sprite.Color.A = (byte)(0.3f * 255.0f);
            }
            else
            {
                self.sprite.Color.R = (byte)255;
                self.sprite.Color.G = (byte)255;
                self.sprite.Color.B = (byte)255;
                self.sprite.Color.A = (byte)255;
            }
        }

        public static bool HaveReceived()
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable(0xCA12023);
        }
    }
}
