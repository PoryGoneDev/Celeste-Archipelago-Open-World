using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.Celeste_Multiworld.Items
{
    public struct CheckpointItemData
    {
        public int Area;
        public int Mode;
        public string Room;

        public CheckpointItemData(int area, int mode, string room)
        {
            Area = area;
            Mode = mode;
            Room = room;
        }
    };

    internal class APItemData
    {
        public static List<modItemBase> modItems { get; } = new List<modItemBase>()
        {
            new modSpring(),
            new modTrafficBlock(),
            new modRefill(),
            new modCassetteBlock(),

            new modDreamBlock(),
            new modCoin(),
            new modStrawberrySeeds(),

            new modSinkingPlatform(),
            new modMovingPlatform(),

            new modCloud(),
            new modBooster(),
            new modMoveBlock(),
            new modWhiteBlock(),

            new modSwapBlock(),
            new modDashSwitch(),
            new modTorch(),
            new modSeeker(),
            new modTheoCrystal(),

            new modFeather(),
            new modBumper(),
            new modKevinBlock(),
            new modBadelineBooster(),

            new modCoreBlock(),
            new modCoreToggle(),
            new modFireBall(),

            new modPuffer(),
            new modJellyfish(),
            new modBreakerBox(),
            new modFlingBird(),
        };

        public static Dictionary<long, string> ItemIDToString { get; set; } = new Dictionary<long, string>
        {
            { 0xCA10000, "Strawberry" },
            { 0xCA10001, "Raspberry" },
            { 0xCA10002, "Blue Raspberry" },
            { 0xCA10003, "Blueberry" },
            { 0xCA10004, "Blackberry" },
            { 0xCA10005, "Boysenberry" },
            { 0xCA10006, "Banana" },
            { 0xCA10007, "Cranberry" },
            { 0xCA10008, "Golden Raspberry" },

            { 0xCA10010, "Granny's House Keys" },

            { 0xCA10020, "Bald Trap" },
            { 0xCA10021, "Literature Trap" },
            { 0xCA10022, "Stun Trap" },
            { 0xCA10023, "Invisible Trap" },
            { 0xCA10024, "Fast Trap" },
            { 0xCA10025, "Slow Trap" },
            { 0xCA10026, "Ice Trap" },
            { 0xCA10028, "Reverse Trap" },
            { 0xCA10029, "Screen Flip Trap" },
            { 0xCA1002A, "Laughter Trap" },
            { 0xCA1002B, "Hiccup Trap" },
            { 0xCA1002C, "Zoom Trap" },
            { 0xCA1002D, "Tiny Trap" },

            { 0xCA10080, "Dash" },
            { 0xCA10081, "Up Dash" },
            { 0xCA10082, "Up-Right Dash" },
            { 0xCA10083, "Right Dash" },
            { 0xCA10084, "Down-Right Dash" },
            { 0xCA10085, "Down Dash" },
            { 0xCA10086, "Down-Left Dash" },
            { 0xCA10087, "Left Dash" },
            { 0xCA10088, "Up-Left Dash" },

            { 0xCA10089, "Climb" },
            { 0xCA1008A, "Right Climb" },
            { 0xCA1008B, "Left Climb" },

            { 0xCA1008C, "Crouch" },

            { 0xCA11000, "Prologue Cassette" },
            { 0xCA11001, "Forsaken City Cassette - A Side" },
            { 0xCA11002, "Forsaken City Cassette - B Side" },
            { 0xCA11003, "Forsaken City Cassette - C Side" },
            { 0xCA11004, "Old Site Cassette - A Side" },
            { 0xCA11005, "Old Site Cassette - B Side" },
            { 0xCA11006, "Old Site Cassette - C Side" },
            { 0xCA11007, "Celestial Resort Cassette - A Side" },
            { 0xCA11008, "Celestial Resort Cassette - B Side" },
            { 0xCA11009, "Celestial Resort Cassette - C Side" },
            { 0xCA1100A, "Golden Ridge Cassette - A Side" },
            { 0xCA1100B, "Golden Ridge Cassette - B Side" },
            { 0xCA1100C, "Golden Ridge Cassette - C Side" },
            { 0xCA1100D, "Mirror Temple Cassette - A Side" },
            { 0xCA1100E, "Mirror Temple Cassette - B Side" },
            { 0xCA1100F, "Mirror Temple Cassette - C Side" },
            { 0xCA11010, "Reflection Cassette - A Side" },
            { 0xCA11011, "Reflection Cassette - B Side" },
            { 0xCA11012, "Reflection Cassette - C Side" },
            { 0xCA11013, "The Summit Cassette - A Side" },
            { 0xCA11014, "The Summit Cassette - B Side" },
            { 0xCA11015, "The Summit Cassette - C Side" },
            { 0xCA11016, "Epilogue Cassette" },
            { 0xCA11017, "Core Cassette - A Side" },
            { 0xCA11018, "Core Cassette - B Side" },
            { 0xCA11019, "Core Cassette - C Side" },
            { 0xCA1101A, "Farewell Cassette" },

            { 0xCA13000, "Crystal Heart 1" },
            { 0xCA13001, "Crystal Heart 2" },
            { 0xCA13002, "Crystal Heart 3" },
            { 0xCA13003, "Crystal Heart 4" },
            { 0xCA13004, "Crystal Heart 5" },
            { 0xCA13005, "Crystal Heart 6" },
            { 0xCA13006, "Crystal Heart 7" },
            { 0xCA13007, "Crystal Heart 8" },
            { 0xCA13008, "Crystal Heart 9" },
            { 0xCA13009, "Crystal Heart 10" },
            { 0xCA1300A, "Crystal Heart 11" },
            { 0xCA1300B, "Crystal Heart 12" },
            { 0xCA1300C, "Crystal Heart 13" },
            { 0xCA1300D, "Crystal Heart 14" },
            { 0xCA1300E, "Crystal Heart 15" },
            { 0xCA1300F, "Crystal Heart 16" },

            { 0xCA14000, "Forsaken City A - Crossing" },
            { 0xCA14001, "Forsaken City A - Chasm" },
            { 0xCA14002, "Forsaken City B - Contraption" },
            { 0xCA14003, "Forsaken City B - Scrap Pit" },
            { 0xCA14004, "Old Site A - Intervention" },
            { 0xCA14005, "Old Site A - Awake" },
            { 0xCA14006, "Old Site B - Combination Lock" },
            { 0xCA14007, "Old Site B - Dream Altar" },
            { 0xCA14008, "Celestial Resort A - Huge Mess" },
            { 0xCA14009, "Celestial Resort A - Elevator Shaft" },
            { 0xCA1400A, "Celestial Resort A - Presidential Suite" },
            { 0xCA1400B, "Celestial Resort B - Staff Quarters" },
            { 0xCA1400C, "Celestial Resort B - Library" },
            { 0xCA1400D, "Celestial Resort B - Rooftop" },
            { 0xCA1400E, "Golden Ridge A - Shrine" },
            { 0xCA1400F, "Golden Ridge A - Old Trail" },
            { 0xCA14010, "Golden Ridge A - Cliff Face" },
            { 0xCA14011, "Golden Ridge B - Stepping Stones" },
            { 0xCA14012, "Golden Ridge B - Gusty Canyon" },
            { 0xCA14013, "Golden Ridge B - Eye of the Storm" },
            { 0xCA14014, "Mirror Temple A - Depths" },
            { 0xCA14015, "Mirror Temple A - Unravelling" },
            { 0xCA14016, "Mirror Temple A - Search" },
            { 0xCA14017, "Mirror Temple A - Rescue" },
            { 0xCA14018, "Mirror Temple B - Central Chamber" },
            { 0xCA14019, "Mirror Temple B - Through the Mirror" },
            { 0xCA1401A, "Mirror Temple B - Mix Master" },
            { 0xCA1401B, "Reflection A - Hollows" },
            { 0xCA1401C, "Reflection A - Reflection" },
            { 0xCA1401D, "Reflection A - Rock Bottom" },
            { 0xCA1401E, "Reflection A - Resolution" },
            { 0xCA1401F, "Reflection B - Reflection" },
            { 0xCA14020, "Reflection B - Rock Bottom" },
            { 0xCA14021, "Reflection B - Reprieve" },
            { 0xCA14022, "The Summit A - 500 M" },
            { 0xCA14023, "The Summit A - 1000 M" },
            { 0xCA14024, "The Summit A - 1500 M" },
            { 0xCA14025, "The Summit A - 2000 M" },
            { 0xCA14026, "The Summit A - 2500 M" },
            { 0xCA14027, "The Summit A - 3000 M" },
            { 0xCA14028, "The Summit B - 500 M" },
            { 0xCA14029, "The Summit B - 1000 M" },
            { 0xCA1402A, "The Summit B - 1500 M" },
            { 0xCA1402B, "The Summit B - 2000 M" },
            { 0xCA1402C, "The Summit B - 2500 M" },
            { 0xCA1402D, "The Summit B - 3000 M" },
            { 0xCA1402E, "Core A - Into the Core" },
            { 0xCA1402F, "Core A - Hot and Cold" },
            { 0xCA14030, "Core A - Heart of the Mountain" },
            { 0xCA14031, "Core B - Into the Core" },
            { 0xCA14032, "Core B - Burning or Freezing" },
            { 0xCA14033, "Core B - Heartbeat" },
            { 0xCA14034, "Farewell - Singular" },
            { 0xCA14035, "Farewell - Power Source" },
            { 0xCA14036, "Farewell - Remembered" },
            { 0xCA14037, "Farewell - Event Horizon" },
            { 0xCA14038, "Farewell - Determination" },
            { 0xCA14039, "Farewell - Stubbornness" },
            { 0xCA1403A, "Farewell - Reconciliation" },
            { 0xCA1403B, "Farewell - Farewell" },

            { 0xCA12000, "Springs" },
            { 0xCA12001, "Traffic Blocks" },
            { 0xCA12002, "Pink Cassette Blocks" },
            { 0xCA12003, "Blue Cassette Blocks" },
            { 0xCA12004, "Dream Blocks" },
            { 0xCA12005, "Coins" },
            { 0xCA12006, "Moving Platforms" },
            { 0xCA12007, "Blue Boosters" },
            { 0xCA12008, "Blue Clouds" },
            { 0xCA12009, "Move Blocks" },
            { 0xCA1200A, "Swap Blocks" },
            { 0xCA1200B, "Red Boosters" },
            { 0xCA1200C, "Theo Crystal" },
            { 0xCA1200D, "Feathers" },
            { 0xCA1200E, "Bumpers" },
            { 0xCA1200F, "Kevins" },
            { 0xCA12010, "Pink Clouds" },
            { 0xCA12011, "Badeline Boosters" },
            { 0xCA12012, "Fire and Ice Balls" },
            { 0xCA12013, "Core Toggles" },
            { 0xCA12014, "Core Blocks" },
            { 0xCA12015, "Pufferfish" },
            { 0xCA12016, "Jellyfish" },
            { 0xCA12017, "Breaker Boxes" },
            { 0xCA12018, "Dash Refills" },
            { 0xCA12019, "Double Dash Refills" },
            { 0xCA1201A, "Yellow Cassette Blocks" },
            { 0xCA1201B, "Green Cassette Blocks" },
            { 0xCA1201C, "Dash Switches" },
            { 0xCA1201D, "Seekers" },
            { 0xCA1201F, "Strawberry Seeds" },
            { 0xCA12020, "Sinking Platforms" },
            { 0xCA12021, "White Block" },
            { 0xCA12022, "Blue Torches" },
            { 0xCA12023, "Bird" },
            { 0xCA12024, "Yellow Torches" },

            { 0xCA16000, "Celestial Resort A - Front Door Key" },
            { 0xCA16001, "Celestial Resort A - Hallway Key 1" },
            { 0xCA16002, "Celestial Resort A - Hallway Key 2" },
            { 0xCA16003, "Celestial Resort A - Huge Mess Key" },
            { 0xCA16004, "Celestial Resort A - Presidential Suite Key" },
            { 0xCA16005, "Mirror Temple A - Entrance Key" },
            { 0xCA16006, "Mirror Temple A - Depths Key" },
            { 0xCA16007, "Mirror Temple A - Search Key 1" },
            { 0xCA16008, "Mirror Temple A - Search Key 2" },
            { 0xCA16009, "Mirror Temple A - Search Key 3" },
            { 0xCA1600A, "Mirror Temple B - Central Chamber Key 1" },
            { 0xCA1600B, "Mirror Temple B - Central Chamber Key 2" },
            { 0xCA1600C, "The Summit A - 2500 M Key" },

            { 0xCA1600D, "Farewell - Power Source Key 1" },
            { 0xCA1600E, "Farewell - Power Source Key 2" },
            { 0xCA1600F, "Farewell - Power Source Key 3" },
            { 0xCA16010, "Farewell - Power Source Key 4" },
            { 0xCA16011, "Farewell - Power Source Key 5" },

            { 0xCA16A00, "The Summit A - Gem 1" },
            { 0xCA16A01, "The Summit A - Gem 2" },
            { 0xCA16A02, "The Summit A - Gem 3" },
            { 0xCA16A03, "The Summit A - Gem 4" },
            { 0xCA16A04, "The Summit A - Gem 5" },
            { 0xCA16A05, "The Summit A - Gem 6" },
        };

        public static Dictionary<string, long> AreaModeToCassetteID { get; set; } = new Dictionary<string, long>
        {
            { "0a", 0xCA11000 },
            { "1a", 0xCA11001 },
            { "1b", 0xCA11002 },
            { "1c", 0xCA11003 },
            { "2a", 0xCA11004 },
            { "2b", 0xCA11005 },
            { "2c", 0xCA11006 },
            { "3a", 0xCA11007 },
            { "3b", 0xCA11008 },
            { "3c", 0xCA11009 },
            { "4a", 0xCA1100A },
            { "4b", 0xCA1100B },
            { "4c", 0xCA1100C },
            { "5a", 0xCA1100D },
            { "5b", 0xCA1100E },
            { "5c", 0xCA1100F },
            { "6a", 0xCA11010 },
            { "6b", 0xCA11011 },
            { "6c", 0xCA11012 },
            { "7a", 0xCA11013 },
            { "7b", 0xCA11014 },
            { "7c", 0xCA11015 },
            { "8a", 0xCA11016 },
            { "9a", 0xCA11017 },
            { "9b", 0xCA11018 },
            { "9c", 0xCA11019 },
            { "10a", 0xCA1101A },
        };

        public static Dictionary<long, CheckpointItemData> CheckpointData { get; set; } = new Dictionary<long, CheckpointItemData>
        {
            { 0xCA14000, new CheckpointItemData(1, 0, "6") },
            { 0xCA14001, new CheckpointItemData(1, 0, "9b") },

            { 0xCA14002, new CheckpointItemData(1, 1, "04") },
            { 0xCA14003, new CheckpointItemData(1, 1, "08") },

            { 0xCA14004, new CheckpointItemData(2, 0, "3") },
            { 0xCA14005, new CheckpointItemData(2, 0, "end_3") },

            { 0xCA14006, new CheckpointItemData(2, 1, "03") },
            { 0xCA14007, new CheckpointItemData(2, 1, "08b") },

            { 0xCA14008, new CheckpointItemData(3, 0, "08-a") },
            { 0xCA14009, new CheckpointItemData(3, 0, "09-d") },
            { 0xCA1400A, new CheckpointItemData(3, 0, "00-d") },

            { 0xCA1400B, new CheckpointItemData(3, 1, "06") },
            { 0xCA1400C, new CheckpointItemData(3, 1, "11") },
            { 0xCA1400D, new CheckpointItemData(3, 1, "16") },

            { 0xCA1400E, new CheckpointItemData(4, 0, "b-00") },
            { 0xCA1400F, new CheckpointItemData(4, 0, "c-00") },
            { 0xCA14010, new CheckpointItemData(4, 0, "d-00") },

            { 0xCA14011, new CheckpointItemData(4, 1, "b-00") },
            { 0xCA14012, new CheckpointItemData(4, 1, "c-00") },
            { 0xCA14013, new CheckpointItemData(4, 1, "d-00") },

            { 0xCA14014, new CheckpointItemData(5, 0, "b-00") },
            { 0xCA14015, new CheckpointItemData(5, 0, "c-00") },
            { 0xCA14016, new CheckpointItemData(5, 0, "d-00") },
            { 0xCA14017, new CheckpointItemData(5, 0, "e-00") },

            { 0xCA14018, new CheckpointItemData(5, 1, "b-00") },
            { 0xCA14019, new CheckpointItemData(5, 1, "c-00") },
            { 0xCA1401A, new CheckpointItemData(5, 1, "d-00") },

            { 0xCA1401B, new CheckpointItemData(6, 0, "04") },
            { 0xCA1401C, new CheckpointItemData(6, 0, "b-00") },
            { 0xCA1401D, new CheckpointItemData(6, 0, "boss-00") },
            { 0xCA1401E, new CheckpointItemData(6, 0, "after-00") },

            { 0xCA1401F, new CheckpointItemData(6, 1, "b-00") },
            { 0xCA14020, new CheckpointItemData(6, 1, "c-00") },
            { 0xCA14021, new CheckpointItemData(6, 1, "d-00") },

            { 0xCA14022, new CheckpointItemData(7, 0, "b-00") },
            { 0xCA14023, new CheckpointItemData(7, 0, "c-00") },
            { 0xCA14024, new CheckpointItemData(7, 0, "d-00") },
            { 0xCA14025, new CheckpointItemData(7, 0, "e-00b") },
            { 0xCA14026, new CheckpointItemData(7, 0, "f-00") },
            { 0xCA14027, new CheckpointItemData(7, 0, "g-00") },

            { 0xCA14028, new CheckpointItemData(7, 1, "b-00") },
            { 0xCA14029, new CheckpointItemData(7, 1, "c-01") },
            { 0xCA1402A, new CheckpointItemData(7, 1, "d-00") },
            { 0xCA1402B, new CheckpointItemData(7, 1, "e-00") },
            { 0xCA1402C, new CheckpointItemData(7, 1, "f-00") },
            { 0xCA1402D, new CheckpointItemData(7, 1, "g-00") },

            { 0xCA1402E, new CheckpointItemData(9, 0, "a-00") },
            { 0xCA1402F, new CheckpointItemData(9, 0, "c-00") },
            { 0xCA14030, new CheckpointItemData(9, 0, "d-00") },

            { 0xCA14031, new CheckpointItemData(9, 1, "a-00") },
            { 0xCA14032, new CheckpointItemData(9, 1, "b-00") },
            { 0xCA14033, new CheckpointItemData(9, 1, "c-01") },

            { 0xCA14034, new CheckpointItemData(10, 0, "a-00") },
            { 0xCA14035, new CheckpointItemData(10, 0, "c-00") },
            { 0xCA14036, new CheckpointItemData(10, 0, "e-00z") },
            { 0xCA14037, new CheckpointItemData(10, 0, "f-door") },
            { 0xCA14038, new CheckpointItemData(10, 0, "h-00b") },
            { 0xCA14039, new CheckpointItemData(10, 0, "i-00") },
            { 0xCA1403A, new CheckpointItemData(10, 0, "j-00") },
            { 0xCA1403B, new CheckpointItemData(10, 0, "j-16") },
        };
        public static Dictionary<long, string> LevelNumToName { get; set; } = new Dictionary<long, string>
        {
            { 0, "Prologue" },
            { 1, "Forsaken City" },
            { 2, "Old Site" },
            { 3, "Celestial Resort" },
            { 4, "Golden Ridge" },
            { 5, "Mirror Temple" },
            { 6, "Reflection" },
            { 7, "The Summit" },
            { 8, "Epilogue" },
            { 9, "Core" },
            { 10, "Farewell" },
        };
        public static string ConvertInteractableName(long itemId)
        {
            string interactableName = "";

            long adjustedId = itemId - 0xCA17000;
            long baseItemId = (adjustedId % 0x40) + 0xCA12000;
            string baseItemName = ItemIDToString[baseItemId];

            long splitType = adjustedId / 0x1000;

            if (splitType == 0)
            {
                // Per Side
                long side = adjustedId / 0x100;
                string sideString = "A";
                if (side == 1)
                {
                    sideString = "B";
                }
                else if (side == 2)
                {
                    sideString = "C";
                }

                interactableName = sideString + "-Side " + baseItemName;
            }
            else if (splitType == 1)
            {
                // Per Level
                adjustedId -= 0x1000;

                long level = adjustedId / 0x40;
                string levelName = LevelNumToName[level];

                interactableName = levelName + " " + baseItemName;
            }
            else if (splitType == 2)
            {
                // Per Level and Side
                adjustedId -= 0x2000;

                long side = (adjustedId % 0x100) / 0x40;
                string sideString = "A";
                if (side == 1)
                {
                    sideString = "B";
                }
                else if (side == 2)
                {
                    sideString = "C";
                }

                long level = adjustedId / 0x100;
                string levelName = LevelNumToName[level];

                if (level == 10)
                {
                    interactableName = levelName + " " + baseItemName;
                }
                else
                {
                    interactableName = levelName + " " + sideString + " " + baseItemName;
                }
            }

            return interactableName;
        }
    }
}
