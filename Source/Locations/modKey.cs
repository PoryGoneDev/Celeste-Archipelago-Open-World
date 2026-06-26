using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modKey : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.Key.Added += modKey_Added;
            On.Celeste.Key.OnPlayer += modKey_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.Key.Added -= modKey_Added;
            On.Celeste.Key.OnPlayer -= modKey_OnPlayer;
        }

        private static void modKey_Added(On.Celeste.Key.orig_Added orig, Key self, Monocle.Scene scene)
        {
            string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";
            if (!Celeste_MultiworldModule.SaveData.KeyLocations.Contains(AP_ID))
            {
                orig(self, scene);
            }
            else
            {
                self.Visible = false;
                self.Collidable = false;
            }
        }

        private static void modKey_OnPlayer(On.Celeste.Key.orig_OnPlayer orig, Key self, Player player)
        {
            self.SceneAs<Level>().Particles.Emit(Key.P_Collect, 10, self.Position, Vector2.One * 3f);
            Audio.Play("event:/game/general/key_get", self.Position);
            Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);

            self.Collidable = false;
            self.Visible = false;
            string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";
            Celeste_MultiworldModule.SaveData.KeyLocations.Add(AP_ID);
            //Logger.Verbose("AP", AP_ID);

            if (self.nodes != null && self.nodes.Length >= 2)
            {
                self.Add(new Monocle.Coroutine(self.NodeRoutine(player), true));
            }
        }
    }
}
