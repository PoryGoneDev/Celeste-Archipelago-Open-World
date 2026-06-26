using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Celeste.Mod.Celeste_Multiworld.Items.modBooster;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    internal class modTorch : modItemBase
    {
        internal enum TorchColor
        {
            Blue = 0xCA12022,
            Yellow = 0xCA12024,
        }

        public override void Load()
        {
            On.Celeste.Torch.Added += modTorch_Added;
            On.Celeste.Torch.OnPlayer += modTorch_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.Torch.Added -= modTorch_Added;
            On.Celeste.Torch.OnPlayer -= modTorch_OnPlayer;
        }

        private static void modTorch_Added(On.Celeste.Torch.orig_Added orig, Torch self, Monocle.Scene scene)
        {
            self.Scene = scene;
            if (self.Components != null)
            {
                foreach (Monocle.Component component in self.Components)
                {
                    component.EntityAdded(scene);
                }
            }
            self.Scene.SetActualDepth(self);

            if ((ArchipelagoManager.Instance.TorchBehavior == 1 && HaveReceived(self.startLit ? TorchColor.Yellow : TorchColor.Blue)) ||
                (ArchipelagoManager.Instance.TorchBehavior == 0 && self.startLit && HaveReceived(TorchColor.Yellow)) ||
                self.SceneAs<Level>().Session.GetFlag(self.FlagName))
            {
                self.bloom.Visible = (self.light.Visible = true);
                self.lit = true;
                self.Collidable = false;
                self.sprite.Play("on", false, false);
            }
            else
            {
                self.bloom.Visible = (self.light.Visible = false);
                self.lit = false;
                self.Collidable = true;
            }
        }

        private static void modTorch_OnPlayer(On.Celeste.Torch.orig_OnPlayer orig, Torch self, Player player)
        {
            if (HaveReceived(self.startLit ? TorchColor.Yellow : TorchColor.Blue))
            {
                orig(self, player);
            }
        }

        public static bool HaveReceived(TorchColor color)
        {
            return Celeste_MultiworldModule.SaveData.HaveInteractable((int)color);
        }
    }
}
