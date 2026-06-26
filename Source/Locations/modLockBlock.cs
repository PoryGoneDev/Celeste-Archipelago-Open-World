using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modLockBlock : modLocationBase
    {
        static Key animKey = null;

        static Dictionary<string, string> lockToKey = new Dictionary<string, string>()
        {
            // Celestial Resort
            { "3_0_s3:16", "3_0_s3:15" },
            { "3_0_02-a:9", "3_0_02-b:32" },
            { "3_0_07-a:11", "3_0_07-b:2" },
            { "3_0_09-b:2", "3_0_09-b:13" },
            { "3_0_04-c:17", "3_0_02-c:1" },

            // Mirror Temple
            { "5_0_a-13:1", "5_0_a-08:55" }, // Entrance
            { "5_0_b-06:2", "5_0_b-04:3" }, // Depths
            { "5_0_b-14:3", "5_0_b-04:3" }, // Depths
            { "5_0_d-01:310", "5_0_d-04:39" }, // Search 1
            { "5_0_d-01:301", "5_0_d-04:14" }, // Search 2
            { "5_0_d-19:531", "5_0_d-15:216" }, // Search 3

            { "5_1_b-02:226", "5_1_b-02:221" }, // Key 1
            { "5_1_b-00:145", "5_1_b-02:219" }, // Key 2

            // The Summit
            { "7_0_f-05:700", "7_0_f-07:712" }, // 2500 M

            // Farewell
            { "10_0_d-00:197", "10_0_d-04:444" }, // Power Source 1
            { "10_0_d-00:142", "10_0_d-03:315" }, // Power Source 2
            { "10_0_d-00:144", "10_0_d-01:261" }, // Power Source 3
            { "10_0_d-00:145", "10_0_d-02:70" }, // Power Source 4
            { "10_0_d-00:471", "10_0_d-05:593" }, // Power Source 5
        };

        public override void Load()
        {
            On.Celeste.LockBlock.OnPlayer += modLockBlock_OnPlayer;
        }

        public override void Unload()
        {
            On.Celeste.LockBlock.OnPlayer -= modLockBlock_OnPlayer;
        }

        private static void modLockBlock_OnPlayer(On.Celeste.LockBlock.orig_OnPlayer orig, LockBlock self, Player player)
        {
            if (!self.opening)
            {
                string Lock_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_{self.ID}";
                //Logger.Verbose("AP", Lock_ID);

                if (!lockToKey.ContainsKey(Lock_ID))
                {
                    return;
                }

                long AP_ID = APLocationData.KeyIDToAP[lockToKey[Lock_ID]];

                if (!Celeste_MultiworldModule.SaveData.KeyItems.ContainsKey(AP_ID))
                {
                    return;
                }

                if (Celeste_MultiworldModule.SaveData.KeyItems[AP_ID])
                {
                    if (modLockBlock.animKey == null || modLockBlock.animKey.IsUsed)
                    {
                        modLockBlock.animKey = new Key(player, new EntityID("0", 0));
                    }
                    self.SceneAs<Level>().Add(animKey);
                    self.SceneAs<Level>().Session.Keys.Add(animKey.ID);

                    self.TryOpen(player, animKey.follower);
                }
            }
        }
    }
}
