using FMOD;
using System.Collections.Generic;

namespace Celeste.Mod.Celeste_Multiworld;

public class Celeste_MultiworldModuleSaveData : EverestModuleSaveData
{
    #region AP General
    public int ItemRcv { get; set; } = 0;
    #endregion

    #region AP Items
    public int Strawberries { get; set; } = 0;
    public int Raspberries { get; set; } = 0;
    public int BlueRaspberries { get; set; } = 0;
    public int Blueberries { get; set; } = 0;
    public int Blackberries { get; set; } = 0;
    public int Boysenberries { get; set; } = 0;
    public int Bananas { get; set; } = 0;
    public int Cranberries { get; set; } = 0;
    public int GoldenRaspberries { get; set; } = 0;
    public bool GoalItem { get; set; } = false;

    public Dictionary<long, bool> Interactables { get; set; } = new Dictionary<long, bool>();
    public Dictionary<long, bool> CassetteItems { get; set; } = new Dictionary<long, bool>();
    public List<string> Poem { get; set; } = new List<string>();
    public Dictionary<long, bool> KeyItems { get; set; } = new Dictionary<long, bool>();
    public Dictionary<long, bool> GemItems { get; set; } = new Dictionary<long, bool>();
    public bool UpDash = false;
    public bool UpRightDash = false;
    public bool RightDash = false;
    public bool DownRightDash = false;
    public bool DownDash = false;
    public bool DownLeftDash = false;
    public bool LeftDash = false;
    public bool UpLeftDash = false;
    public bool RightClimb = false;
    public bool LeftClimb = false;
    public bool Crouch = false;
    #endregion

    #region AP Locations
    public HashSet<string> MiscLocations { get; set; } = new HashSet<string>();
    public HashSet<string> LevelClearLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CheckpointLocations { get; set; } = new HashSet<string>();
    public HashSet<string> StrawberryLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CrystalHeartLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CassetteLocations { get; set; } = new HashSet<string>();
    public HashSet<string> CarLocations { get; set; } = new HashSet<string>();
    public HashSet<string> BinocularsLocations { get; set; } = new HashSet<string>();
    public HashSet<string> KeyLocations { get; set; } = new HashSet<string>();
    public HashSet<string> GemLocations { get; set; } = new HashSet<string>();
    public HashSet<string> RoomLocations { get; set; } = new HashSet<string>();
    #endregion

    public int HaveInteractableJournal(int id, int level, int side)
    {
        bool contains = false;
        int result = 0;

        if (ArchipelagoManager.Instance.SplitInteractables == 0)
        {
            if (ArchipelagoManager.Instance.ExistentInteractables.Contains((long)id))
            {
                Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(id, out contains);
                result = contains ? 2 : 1;
            }
        }
        else if (ArchipelagoManager.Instance.SplitInteractables == 1)
        {
            int per_level_id = id + 0x6000 + level * 0x40;
            if (ArchipelagoManager.Instance.ExistentInteractables.Contains((long)per_level_id))
            {
                Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_level_id, out contains);
                result = contains ? 2 : 1;
            }
        }
        else if (ArchipelagoManager.Instance.SplitInteractables == 2)
        {
            int per_side_id = id + 0x5000 + side * 0x100;
            if (ArchipelagoManager.Instance.ExistentInteractables.Contains((long)per_side_id))
            {
                Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_side_id, out contains);
                result = contains ? 2 : 1;
            }
        }
        else if (ArchipelagoManager.Instance.SplitInteractables == 3)
        {
            if (level == 10)
            {
                int per_level_id = id + 0x6000 + level * 0x40;
                if (ArchipelagoManager.Instance.ExistentInteractables.Contains((long)per_level_id))
                {
                    Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_level_id, out contains);
                    result = contains ? 2 : 1;
                }
            }
            else
            {
                int per_level_side_id = id + 0x7000 + level * 0x100 + side * 0x40;
                if (ArchipelagoManager.Instance.ExistentInteractables.Contains((long)per_level_side_id))
                {
                    Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_level_side_id, out contains);
                    result = contains ? 2 : 1;
                }
            }
        }

        return result;
    }

    public bool HaveInteractable(int id)
    {
        bool result = false;

        // Base Interactable
        Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(id, out result);
        if (result)
        {
            return result;
        }

        // Per Side
        int per_side_id = id + 0x5000 + ((int)SaveData.Instance.CurrentSession_Safe.Area.Mode) * 0x100;
        Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_side_id, out result);
        if (result)
        {
            return result;
        }

        // Per Level
        int per_level_id = id + 0x6000 + ((int)SaveData.Instance.CurrentSession_Safe.Area.ID) * 0x40;
        Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_level_id, out result);
        if (result)
        {
            return result;
        }

        // Per Level and Side
        int per_level_side_id = id + 0x7000 + ((int)SaveData.Instance.CurrentSession_Safe.Area.ID) * 0x100 + ((int)SaveData.Instance.CurrentSession_Safe.Area.Mode) * 0x40;
        Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(per_level_side_id, out result);

        return result;
    }
}
