using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modLookout : modLocationBase
    {
        public override void Load()
        {
            On.Celeste.Lookout.Interact += modLookout_Interact;
        }

        public override void Unload()
        {
            On.Celeste.Lookout.Interact -= modLookout_Interact;
        }

        private static void modLookout_Interact(On.Celeste.Lookout.orig_Interact orig, Lookout self, Player player)
        {
            orig(self, player);

            string binoString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{SaveData.Instance.CurrentSession_Safe.Level}:{self.X}_{self.Y}";

            Celeste_MultiworldModule.SaveData.BinocularsLocations.Add(binoString);
            //Logger.Verbose("AP", binoString);
        }
    }
}
