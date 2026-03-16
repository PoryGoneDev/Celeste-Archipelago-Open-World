using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Microsoft.Xna.Framework;
using M_Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Net.Sockets;
using Archipelago.MultiClient.Net.Converters;
using Newtonsoft.Json;




namespace Celeste.Mod.Celeste_Multiworld
{
    public struct ArchipelagoMessage
    {
        public enum MessageType
        {
            General,
            Chat,
            Server,
            ItemReceive,
            ItemSend,
            ItemHint,
            Literature
        }

        public string Text { get; init; } = "";
        public MessageType Type { get; set; } = MessageType.General;
        public ItemFlags Flags { get; set; } = ItemFlags.Advancement;
        public bool Strawberry { get; set; } = false;

        public ArchipelagoMessage(string text, MessageType type = MessageType.General, ItemFlags flags = ItemFlags.Advancement, bool strawberry = false)
        {
            Text = text;
            Type = type;
            Flags = flags;
            Strawberry = strawberry;
        }
    }


    public class ArchipelagoManager : DrawableGameComponent
    {
        public static ArchipelagoManager Instance { get; private set; }

        private static readonly Version _supportedArchipelagoVersion = new(0, 6, 5);
        public static readonly int _modVersion = 10007;
        private static readonly int _minAPWorldVersion = 10000;

        private ArchipelagoSession _session;
        private DeathLinkService _deathLinkService;
        private DateTime _lastDeath;

        public bool GoalSent = false;

        public DeathLink DeathLinkData { get; private set; }
        public int DeathsCounted = 0;
        public bool IsDeathLinkSafe { get; set; }
        public bool Ready { get; private set; }
        public bool WasConnected { get; private set; }
        public List<Tuple<int, ItemInfo>> ItemQueue { get; private set; } = new();
        public List<long> CollectedLocations { get; private set; } = new();
        public Dictionary<long, ItemInfo> LocationDictionary { get; private set; } = new();
        public HashSet<long> SentLocations { get; set; } = [];
        public List<ArchipelagoMessage> MessageLog { get; set; } = new();

        public int Slot => _session.ConnectionInfo.Slot;
        public bool DeathLink => _session.ConnectionInfo.Tags.Contains("DeathLink");
        public int HintPoints => _session.RoomState.HintPoints;
        public int HintCost => _session.RoomState.HintCost;
        public Hint[] Hints => _session.DataStorage.GetHints();

        public int ServerItemsRcv = -1;
        private bool ItemRcvCallbackSet = false;

        public string StoredRoom = "";

        #region Slot Data
        public int StrawberriesRequired { get; set; }
        public bool DeathLinkActive { get; set; }
        public int DeathLinkAmnesty { get; set; }
        public bool TrapLinkActive { get; set; }
        public bool Binosanity = false;
        public bool Roomsanity = false;
        public bool Carsanity = false;
        public bool IncludeGoldens = false;
        public bool IncludeCore = false;
        public bool IncludeFarewell = false;
        public bool IncludeBSides = false;
        public bool IncludeCSides = false;
        public List<string> ActiveLevels { get; set; } = new();
        public string GoalLevel = "";
        public bool LockGoalLevel = true;
        public Dictionary<int, int> MusicMap { get; set; } = new();
        public int MusicShuffle = 0;
        public bool RequireCassettes = false;
        public int ChosenPoem = 0;
        #endregion

        private static string commandHolder = null;
        private static FieldInfo currentText = typeof(Monocle.Commands).GetField("currentText", BindingFlags.NonPublic | BindingFlags.Instance);

        public ArchipelagoManager(Game game) : base(game)
        {
            game.Components.Add(this);
            Instance = this;

            On.Monocle.Commands.EnterCommand += copyConsoleCommandToHolder;
            On.Monocle.Commands.ExecuteCommand += customParseDisplayMessageCommandConsole;
        }

        public override void Update(GameTime gameTime)
        {
            if (Ready)
            {
                try
                {
                    CheckReceivedItemQueue();
                    CheckLocationsToSend();
                    HandleCollectedLocations();

                    Level level = (Monocle.Engine.Scene as Level);

                    if (level == null)
                    {
                        this.SetRoomStorage("");
                    }
                }
                catch (ArchipelagoSocketClosedException)
                {
                    Disconnect();
                }
            }
        }

        public async Task<LoginFailure> TryConnect()
        {
            _lastDeath = DateTime.MinValue;
            _session = ArchipelagoSessionFactory.CreateSession(Celeste_MultiworldModule.Settings.Address);

            // (Re-)initialize state.
            DeathLinkData = null;
            IsDeathLinkSafe = false;
            Ready = false;
            ItemQueue = new();
            LocationDictionary = new();
            GoalSent = false;

            // Watch for the following events.
            _session.Socket.ErrorReceived += OnError;
            _session.Socket.SocketClosed += OnSocketClosed;
            _session.Socket.PacketReceived += OnPacketReceived;
            _session.MessageLog.OnMessageReceived += OnMessageReceived;
            _session.Items.ItemReceived += OnItemReceived;
            _session.Locations.CheckedLocationsUpdated += OnLocationReceived;

            // Attempt to connect to the server.
            try
            {
                await _session.ConnectAsync();
            }
            catch (Exception ex)
            {
                Disconnect();
                string message = $"Unable to establish an initial connection to the Archipelago server @ {Celeste_MultiworldModule.Settings.Address} : {ex.Message}";
                Monocle.Engine.Commands.Log(message, M_Color.Red);
                return new(message);
            }

            var result = await _session.LoginAsync(
                "Celeste (Open World)",
                Celeste_MultiworldModule.Settings.SlotName,
                ItemsHandlingFlags.AllItems,
                _supportedArchipelagoVersion,
                uuid: Guid.NewGuid().ToString(),
                password: Celeste_MultiworldModule.Settings.Password
            );

            if (!result.Successful)
            {
                Disconnect();
                Monocle.Engine.Commands.Log((result as LoginFailure).ToString(), M_Color.Red);
                return result as LoginFailure;
            }

            // Load randomizer data.
            object value;

            int apworldVersion = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("apworld_version", out value) ? value : 0);
            int minModVersion = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("min_mod_version", out value) ? value : 0);

            if (apworldVersion < _minAPWorldVersion)
            {
                Disconnect();
                int major = _minAPWorldVersion / 10000;
                int minor = (_minAPWorldVersion / 100) % 100;
                int bugfix = _minAPWorldVersion % 100;
                Monocle.Engine.Commands.Log($"Mod is too new for APWorld.\nUpdate your APWorld to v{major}.{minor}.{bugfix} and regenerate, or downgrade your mod.", M_Color.Red);
                return new("Mod Version Too New");
            }
            if (_modVersion < minModVersion)
            {
                Disconnect();
                int major = minModVersion / 10000;
                int minor = (minModVersion / 100) % 100;
                int bugfix = minModVersion % 100;
                Monocle.Engine.Commands.Log($"Mod is too old for APWorld.\nUpdate your mod to v{major}.{minor}.{bugfix}.", M_Color.Red);
                return new("Mod Version Too Old");
            }

            int apworld_major = apworldVersion / 10000;
            int apworld_minor = (apworldVersion / 100) % 100;
            int apworld_bugfix = apworldVersion % 100;
            Monocle.Engine.Commands.Log($"Connected to APWorld v{apworld_major}.{apworld_minor}.{apworld_bugfix}.", M_Color.Green);

            int hairLengthInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_hair_length", out value) ? value : 4);
            General.modPlayer.HairLength = hairLengthInt;
            int normalHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_one_dash_hair_color", out value) ? value : 0xdb2c00);
            int twoDashHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_two_dash_hair_color", out value) ? value : 0xfa91ff);
            int noDashHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_no_dash_hair_color", out value) ? value : 0x6ec0ff);
            int featherHairInt = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("madeline_feather_hair_color", out value) ? value : 0xf2d450);
            Player.NormalHairColor = new M_Color((normalHairInt >> 16) & 0xFF, (normalHairInt >> 8) & 0xFF, (normalHairInt) & 0xFF);
            Player.TwoDashesHairColor = new M_Color((twoDashHairInt >> 16) & 0xFF, (twoDashHairInt >> 8) & 0xFF, (twoDashHairInt) & 0xFF);
            Player.UsedHairColor = new M_Color((noDashHairInt >> 16) & 0xFF, (noDashHairInt >> 8) & 0xFF, (noDashHairInt) & 0xFF);
            Player.FlyPowerHairColor = new M_Color((featherHairInt >> 16) & 0xFF, (featherHairInt >> 8) & 0xFF, (featherHairInt) & 0xFF);

            Items.Traps.TrapManager.EnabledTraps = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, int>>(((LoginSuccessful)result).SlotData["active_traps"].ToString());
            Items.Traps.TrapManager.ExpirationAction = (Items.Traps.TrapExpirationAction)Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("trap_expiration_action", out value) ? value : 100);
            Items.Traps.TrapManager.ExpirationAmount = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("trap_expiration_amount", out value) ? value : 100);

            StrawberriesRequired = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("strawberries_required", out value) ? value : 100);
            DeathLinkActive = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("death_link", out value) ? value : false);
            DeathLinkAmnesty = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("death_link_amnesty", out value) ? value : 10);
            TrapLinkActive = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("trap_link", out value) ? value : false);
            Binosanity = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("binosanity", out value) ? value : false);
            Roomsanity = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("roomsanity", out value) ? value : false);
            Carsanity = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("carsanity", out value) ? value : false);
            IncludeGoldens = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_goldens", out value) ? value : false);
            IncludeCore = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_core", out value) ? value : false);
            IncludeFarewell = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_farewell", out value) ? value : false);
            IncludeBSides = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_b_sides", out value) ? value : false);
            IncludeCSides = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("include_c_sides", out value) ? value : false);

            ActiveLevels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(((LoginSuccessful)result).SlotData["active_levels"].ToString());
            GoalLevel = Convert.ToString(((LoginSuccessful)result).SlotData.TryGetValue("goal_area", out value) ? value : false);
            LockGoalLevel = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("lock_goal_area", out value) ? value : false);

            MusicMap = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, int>>(((LoginSuccessful)result).SlotData["music_map"].ToString());
            MusicShuffle = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("music_shuffle", out value) ? value : 0);
            RequireCassettes = Convert.ToBoolean(((LoginSuccessful)result).SlotData.TryGetValue("require_cassettes", out value) ? value : false);
            ChosenPoem = Convert.ToInt32(((LoginSuccessful)result).SlotData.TryGetValue("chosen_poem", out value) ? value : 0);
            ChosenPoem = ChosenPoem % UI.modJournal.Poems.Count;

            // Initialize DeathLink service.
            _deathLinkService = _session.CreateDeathLinkService();
            _deathLinkService.OnDeathLinkReceived += OnDeathLink;
            if (DeathLinkActive)
            {
                _deathLinkService.EnableDeathLink();
            }

            if (TrapLinkActive)
            {
                _session.ConnectionInfo.UpdateConnectionOptions(_session.ConnectionInfo.Tags.Concat(new string[1] { "TrapLink" }).ToArray());
            }

            this.AddItemsRcvCallback($"Celeste_Open_Rcv_{_session.Players.GetPlayerName(this.Slot)}", ItemsRcvUpdated);
            this.ServerItemsRcv = -1;

            // TODO: Wrap this and only do if active
            //AddPlayerListCallback($"Celeste_OtherPlayers_List", PlayerListUpdated);

            // Build dictionary of locations with item information for fast lookup.
            await BuildLocationDictionary();

            // Return null to signify no error.
            Ready = true;
            WasConnected = true;
            return null;
        }

        public async Task<LoginFailure> Disconnect(bool attemptReconnect = true)
        {
            this.Ready = false;
            this.SentLocations.Clear();
            Items.Traps.TrapManager.Instance.Reset();

            this.GoalSent = false;
            this.ServerItemsRcv = -1;
            this.StoredRoom = "";
            this.DeathsCounted = 0;
            this.ItemQueue.Clear();
            this.MessageLog.Clear();
            this._lastDeath = DateTime.MinValue;

            if (!attemptReconnect)
            {
                this.WasConnected = false;
            }

            // Clear DeathLink events.
            if (_deathLinkService != null)
            {
                _deathLinkService.OnDeathLinkReceived -= OnDeathLink;
                _deathLinkService = null;
            }

            // Clear events and session object.
            if (_session != null)
            {
                _session.Socket.ErrorReceived -= OnError;
                _session.Socket.SocketClosed -= OnSocketClosed;
                _session.Items.ItemReceived -= OnItemReceived;
                _session.Locations.CheckedLocationsUpdated -= OnLocationReceived;
                _session.Socket.PacketReceived -= OnPacketReceived;
                _session.Socket.DisconnectAsync(); // It'll disconnect on its own time.
                _session = null;
            }

            if (this.WasConnected && attemptReconnect)
            {
                return await this.TryConnect();
            }
            else
            {
                return null;
            }
        }

        private void OnDeathLink(DeathLink deathLink)
        {
            // If we receive a DeathLink that is after our last death, let's set it.
            if (!IsDeathLinkSafe && DateTime.Compare(deathLink.Timestamp, _lastDeath) > 0)
            {
                DeathLinkData = deathLink;
            }
        }

        public void ClearDeathLink()
        {
            DeathLinkData = null;
            DeathsCounted = 0;
        }

        public void SendDeathLinkIfEnabled(string cause)
        {
            // Do not send any DeathLink messages if it's not enabled.
            if (!DeathLink)
            {
                return;
            }

            DeathsCounted = DeathsCounted + 1;
            if (DeathsCounted < DeathLinkAmnesty)
            {
                return;
            }

            DeathsCounted = 0;

            try
            {
                // Log our current time so we can make sure we ignore our own DeathLink.
                _lastDeath = DateTime.UtcNow;
                cause = $"{_session.Players.GetPlayerAlias(Slot)} {cause}.";

                _deathLinkService.SendDeathLink(new(_session.Players.GetPlayerAlias(Slot), cause));
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }

            ClearDeathLink();
        }

        public void CheckLocations(long[] locations)
        {
            foreach (var locationID in locations)
            {
                SentLocations.Add(locationID);
            }

            try
            {
                _session.Locations.CompleteLocationChecksAsync(locations);
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }
        public void UpdateGameStatus(ArchipelagoClientState state)
        {
            try
            {
                if (state == ArchipelagoClientState.ClientGoal)
                {
                    if (GoalSent)
                    {
                        return;
                    }
                    GoalSent = true;
                }
                SendPacket(new StatusUpdatePacket { Status = state });
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }

        public string GetPlayerName(int slot)
        {
            if (slot == 0)
            {
                return "Archipelago";
            }

            var name = _session.Players.GetPlayerAlias(slot);
            return string.IsNullOrEmpty(name) ? $"Unknown Player {slot}" : name;
        }

        public string GetLocationName(long location)
        {
            var name = _session.Locations.GetLocationNameFromId(location);
            return string.IsNullOrEmpty(name) ? $"Unknown Location {location}" : name;
        }

        public string GetItemName(long item)
        {
            var name = _session.Items.GetItemName(item);
            return string.IsNullOrEmpty(name) ? $"Unknown Item {item}" : name;
        }

        public void EnableDeathLink()
        {
            _deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink()
        {
            _deathLinkService.DisableDeathLink();
        }

        public int LocationsCheckedCount()
        {
            try
            {
                return _session.Locations.AllLocationsChecked.Count();
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
                return 0;
            }
        }

        public int LocationsTotalCount()
        {
            try
            {
                return _session.Locations.AllLocations.Count();
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
                return 0;
            }
        }

        private void SendPacket(ArchipelagoPacketBase packet)
        {
            try
            {
                _session.Socket.SendPacket(packet);
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }

        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            var i = helper.Index;
            while (helper.Any())
            {
                ItemQueue.Add(new(i++, helper.DequeueItem()));
            }
        }

        private void OnLocationReceived(ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (var newLoc in newCheckedLocations)
            {
                CollectedLocations.Add(newLoc);
            }
        }

        private async Task BuildLocationDictionary()
        {
            var locations = await _session.Locations.ScoutLocationsAsync(false, _session.Locations.AllLocations.ToArray());

            foreach (var item in locations)
            {
                LocationDictionary[item.Key] = item.Value;
            }
        }


        #region AP Messaging
        private static void copyConsoleCommandToHolder(On.Monocle.Commands.orig_EnterCommand orig, Monocle.Commands self)
        {
            commandHolder = (string)currentText.GetValue(self);
            orig(self);
            commandHolder = null;
        }

        private static void customParseDisplayMessageCommandConsole(On.Monocle.Commands.orig_ExecuteCommand orig, Monocle.Commands self, string command, string[] args)
        {
            if (commandHolder != null && command == "!ap")
            {
                string[] split = commandHolder.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                args = new string[split.Length - 1];
                for (int i = 1; i < split.Length; i++)
                {
                    args[i - 1] = split[i];
                }
            }

            orig(self, command, args);
        }

        [Monocle.Command("!ap", "Send a message to the AP server.")]
        private static void CmdAP(string ap_command)
        {
            if (ArchipelagoManager.Instance.Ready)
            {
                string final_command = string.Join(" ", ap_command);
                ArchipelagoManager.Instance._session.Say(final_command);
            }
            else
            {
                Monocle.Engine.Commands.Log("Archipelago connection is not established.", M_Color.Red);
            }
        }

        private static string GetColorString(ItemFlags flags)
        {
            string itemColor = "";
            if ((flags & ItemFlags.Advancement) != 0)
            {
                itemColor = "#AF99EF";
            }
            else if ((flags & ItemFlags.NeverExclude) != 0)
            {
                itemColor = "#6D8BE8";
            }
            else if ((flags & ItemFlags.Trap) != 0)
            {
                itemColor = "#FA8072";
            }
            else
            {
                itemColor = "#00EEEE";
            }

            return itemColor;
        }

        private void OnMessageReceived(LogMessage message)
        {
            switch (message)
            {
                case HintItemSendLogMessage:
                    HintItemSendLogMessage hintItemSendMessage = (HintItemSendLogMessage)message;

                    if (hintItemSendMessage.IsRelatedToActivePlayer)
                    {
                        if (!hintItemSendMessage.IsFound)
                        {
                            var item = hintItemSendMessage.Item;

                            string itemColor = GetColorString(item.Flags);
                            string sendPlayerColor = (hintItemSendMessage.Sender == this.Slot) ? "#EE00EE" : "#FAFAD2";
                            string recvPlayerColor = (hintItemSendMessage.Receiver == this.Slot) ? "#EE00EE" : "#FAFAD2";
                            string prettyMessage = $"{{{recvPlayerColor}}}{hintItemSendMessage.Receiver.Name}{{#}}'s {{{itemColor}}}{item.ItemName}{{#}} is at {{#00FF7F}}{hintItemSendMessage.Item.LocationName}{{#}} in {{{sendPlayerColor}}}{hintItemSendMessage.Sender.Name}{{#}}'s World.";

                            MessageLog.Add(new ArchipelagoMessage(prettyMessage, ArchipelagoMessage.MessageType.ItemHint));
                        }
                        Logger.Log("AP", message.ToString());
                        Monocle.Engine.Commands.Log(message.ToString(), M_Color.Orange);
                    }
                    break;
                case ItemSendLogMessage:
                    ItemSendLogMessage itemSendMessage = (ItemSendLogMessage)message;

                    if (itemSendMessage.IsRelatedToActivePlayer && !itemSendMessage.IsReceiverTheActivePlayer)
                    {
                        string itemColor = GetColorString(itemSendMessage.Item.Flags);
                        string prettyMessage = $"Sent {{{itemColor}}}{itemSendMessage.Item.ItemName}{{#}} to {{#FAFAD2}}{itemSendMessage.Receiver.Name}{{#}}.";

                        MessageLog.Add(new ArchipelagoMessage(prettyMessage.ToString(), ArchipelagoMessage.MessageType.ItemSend, itemSendMessage.Item.Flags));
                        Logger.Log("AP", message.ToString());
                        Monocle.Engine.Commands.Log(message.ToString(), M_Color.Lime);
                    }
                    break;
                case CommandResultLogMessage:
                case ServerChatLogMessage:
                case CountdownLogMessage:
                    Monocle.Engine.Commands.Log(message.ToString());
                    MessageLog.Add(new ArchipelagoMessage(message.ToString(), ArchipelagoMessage.MessageType.Server));
                    break;
                case ChatLogMessage:
                    Monocle.Engine.Commands.Log(message.ToString());
                    MessageLog.Add(new ArchipelagoMessage(message.ToString(), ArchipelagoMessage.MessageType.Chat));
                    break;
                case GoalLogMessage:
                    Monocle.Engine.Commands.Log(message.ToString(), M_Color.Gold);
                    MessageLog.Add(new ArchipelagoMessage(message.ToString()));
                    break;
            }
        }

        private static void OnError(Exception exception, string message)
        {
            Logger.Error("AP", message);
            Monocle.Engine.Commands.Log(message, M_Color.Red);

            ArchipelagoManager.Instance.Disconnect();
        }

        private static void OnSocketClosed(string reason)
        {
            Logger.Error("AP", reason);
            Monocle.Engine.Commands.Log(reason, M_Color.Red);

            ArchipelagoManager.Instance.Disconnect();
        }
        #endregion


        public void CheckReceivedItemQueue()
        {
            if (this.Slot == -1 || SaveData.Instance == null || Celeste_MultiworldModule.SaveData == null)
            {
                return;
            }

            if (this.ServerItemsRcv < 0)
            {
                this.ServerItemsRcv = this.GetInt($"Celeste_Open_Rcv_{_session.Players.GetPlayerName(this.Slot)}");
                return;
            }

            SaveData.Instance.TotalStrawberries_Safe = Celeste_MultiworldModule.SaveData.Strawberries;

            int audioGuard = 0;
            for (int index = Celeste_MultiworldModule.SaveData.ItemRcv; index < ItemQueue.Count; index++)
            {
                var item = ItemQueue[index].Item2;

                string receivedMessage = $"Received {Items.APItemData.ItemIDToString[item.ItemId]} from {GetPlayerName(item.Player)}.";
                string itemColor = GetColorString(item.Flags);
                string prettyMessage = "";

                if (item.Player == this.Slot)
                {
                    prettyMessage = $"You found your {{{itemColor}}}{Items.APItemData.ItemIDToString[item.ItemId]}{{#}}.";
                }
                else
                {
                    prettyMessage = $"Received {{{itemColor}}}{Items.APItemData.ItemIDToString[item.ItemId]}{{#}} from {{#FAFAD2}}{GetPlayerName(item.Player)}{{#}}.";
                }

                if ((item.ItemId < 0xCA10020 || item.ItemId >= 0xCA10050) && index >= this.ServerItemsRcv)
                {
                    Logger.Info("AP", receivedMessage);
                    MessageLog.Add(new ArchipelagoMessage(prettyMessage, ArchipelagoMessage.MessageType.ItemReceive, item.Flags));
                    Monocle.Engine.Commands.Log(receivedMessage, M_Color.DeepPink);
                }

                switch (item.ItemId)
                {
                    case 0xCA10000:
                    {
                        Celeste_MultiworldModule.SaveData.Strawberries += 1;
                        break;
                    }
                    case 0xCA10001:
                    {
                        Celeste_MultiworldModule.SaveData.Raspberries += 1;
                        break;
                    }
                    case 0xCA10010:
                    {
                        Celeste_MultiworldModule.SaveData.GoalItem = true;
                        break;
                    }
                    case long id when id >= 0xCA10020 && id < 0xCA10050:
                    {
                        if (index >= this.ServerItemsRcv)
                        {
                            Items.Traps.TrapManager.Instance.AddTrapToQueue((Items.Traps.TrapType)(id - 0xCA10000), prettyMessage);
                        }
                        break;
                    }
                    case long id when id >= 0xCA11000 && id < 0xCA12000:
                    {
                        Celeste_MultiworldModule.SaveData.CassetteItems[id] = true;

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_gen_cassette_get);
                        }
                        break;
                    }
                    case long id when id >= 0xCA13000 && id < 0xCA14000:
                    {
                        string newPhrase = UI.modJournal.Poems[ChosenPoem][(int)(id - 0xCA13000)];
                        if (!Celeste_MultiworldModule.SaveData.Poem.Contains(newPhrase))
                        {
                            Celeste_MultiworldModule.SaveData.Poem.Add(newPhrase);

                            if (audioGuard < 3)
                            {
                                audioGuard++;
                                Audio.Play(SFX.ui_postgame_crystalheart);
                            }
                        }
                        break;
                    }
                    case long id when id >= 0xCA14000 && id < 0xCA15000:
                    {
                        Items.CheckpointItemData cp_data = Items.APItemData.CheckpointData[id];
                        SaveData.Instance.Areas_Safe[cp_data.Area].Modes[cp_data.Mode].Checkpoints.Add(cp_data.Room);

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_07_checkpointconfetti);
                        }
                        break;
                    }
                    case long id when id >= 0xCA16000 && id < 0xCA16A00:
                    {
                        Celeste_MultiworldModule.SaveData.KeyItems[id] = true;

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_gen_key_get);
                        }
                        break;
                    }
                    case long id when id >= 0xCA16A00 && id < 0xCA17000:
                    {
                        Celeste_MultiworldModule.SaveData.GemItems[id] = true;

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_07_gem_get);
                        }
                        break;
                    }
                    case long id when id >= 0xCA12000 && id < 0xCA12030:
                    {
                        Celeste_MultiworldModule.SaveData.Interactables[id] = true;

                        if (audioGuard < 3)
                        {
                            audioGuard++;
                            Audio.Play(SFX.game_gen_secret_revealed);
                        }
                        break;
                    }
                }

                Celeste_MultiworldModule.SaveData.ItemRcv = index + 1;
            }

            if (Celeste_MultiworldModule.SaveData.ItemRcv > this.ServerItemsRcv)
            {
                this.ServerItemsRcv = Celeste_MultiworldModule.SaveData.ItemRcv;
                this.Set($"Celeste_Open_Rcv_{_session.Players.GetPlayerName(this.Slot)}", Celeste_MultiworldModule.SaveData.ItemRcv);
            }
        }

        public void CheckLocationsToSend()
        {
            if (SaveData.Instance == null || Celeste_MultiworldModule.SaveData == null)
            {
                return;
            }

            List<long> locationsToCheck = new List<long>();
            foreach (KeyValuePair<string, long> checkpointIDPair in Locations.APLocationData.CheckpointStringToID)
            {
                if (Celeste_MultiworldModule.SaveData.CheckpointLocations.Contains(checkpointIDPair.Key))
                {
                    long locationID = checkpointIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> levelClearIDPair in Locations.APLocationData.LevelClearIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.LevelClearLocations.Contains(levelClearIDPair.Key))
                {
                    long locationID = levelClearIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> cassetteIDPair in Locations.APLocationData.CassetteIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.CassetteLocations.Contains(cassetteIDPair.Key))
                {
                    long locationID = cassetteIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> crystalHeartIDPair in Locations.APLocationData.CrystalHeartIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.CrystalHeartLocations.Contains(crystalHeartIDPair.Key))
                {
                    long locationID = crystalHeartIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> carIDPair in Locations.APLocationData.CarIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.CarLocations.Contains(carIDPair.Key))
                {
                    long locationID = carIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> keyIDPair in Locations.APLocationData.KeyIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.KeyLocations.Contains(keyIDPair.Key))
                {
                    long locationID = keyIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> gemIDPair in Locations.APLocationData.GemIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.GemLocations.Contains(gemIDPair.Key))
                {
                    long locationID = gemIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> strawberryIDPair in Locations.APLocationData.StrawberryIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.StrawberryLocations.Contains(strawberryIDPair.Key))
                {
                    long locationID = strawberryIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> binocularsIDPair in Locations.APLocationData.BinocularsIDToAP)
            {
                if (Celeste_MultiworldModule.SaveData.BinocularsLocations.Contains(binocularsIDPair.Key))
                {
                    long locationID = binocularsIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }
            foreach (KeyValuePair<string, long> roomIDPair in Locations.APLocationData.RoomNameToAP)
            {
                if (Celeste_MultiworldModule.SaveData.RoomLocations.Contains(roomIDPair.Key))
                {
                    long locationID = roomIDPair.Value;
                    if (!SentLocations.Contains(locationID))
                    {
                        locationsToCheck.Add(locationID);
                    }
                }
            }

            CheckLocations(locationsToCheck.ToArray());
        }

        public void HandleCollectedLocations()
        {
            if (SaveData.Instance == null || Celeste_MultiworldModule.SaveData == null)
            {
                return;
            }

            foreach (long newLoc in CollectedLocations)
            {
                if (Locations.APLocationData.CheckpointIDToString.ContainsKey(newLoc))
                {
                    string checkpointLocString = Locations.APLocationData.CheckpointIDToString[newLoc];

                    Celeste_MultiworldModule.SaveData.CheckpointLocations.Add(checkpointLocString);
                }

                if (Locations.APLocationData.LevelClearAPToID.ContainsKey(newLoc))
                {
                    string levelClearLocString = Locations.APLocationData.LevelClearAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.LevelClearLocations.Add(levelClearLocString);
                }

                if (Locations.APLocationData.CassetteAPToID.ContainsKey(newLoc))
                {
                    string cassetteLocString = Locations.APLocationData.CassetteAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.CassetteLocations.Add(cassetteLocString);

                    string[] area_mode = cassetteLocString.Split(new char[] { '_' }, 3);
                    int area = Int32.Parse(area_mode[0]);
                    int mode = Int32.Parse(area_mode[1]);

                    SaveData.Instance.RegisterCassette(new AreaKey(area, (AreaMode)mode));
                }

                if (Locations.APLocationData.CrystalHeartAPToID.ContainsKey(newLoc))
                {
                    string crystalHeartLocString = Locations.APLocationData.CrystalHeartAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.CrystalHeartLocations.Add(crystalHeartLocString);

                    string[] area_mode = crystalHeartLocString.Split(new char[] { '_' }, 3);
                    int area = Int32.Parse(area_mode[0]);
                    int mode = Int32.Parse(area_mode[1]);

                    SaveData.Instance.RegisterHeartGem(new AreaKey(area, (AreaMode)mode));
                }

                if (Locations.APLocationData.CarAPToID.ContainsKey(newLoc))
                {
                    string carLocString = Locations.APLocationData.CarAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.CarLocations.Add(carLocString);
                }

                if (Locations.APLocationData.KeyAPToID.ContainsKey(newLoc))
                {
                    string keyLocString = Locations.APLocationData.KeyAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.KeyLocations.Add(keyLocString);
                }

                if (Locations.APLocationData.GemAPToID.ContainsKey(newLoc))
                {
                    string gemLocString = Locations.APLocationData.GemAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.GemLocations.Add(gemLocString);
                }

                if (Locations.APLocationData.StrawberryAPToID.ContainsKey(newLoc))
                {
                    string strawberryLocString = Locations.APLocationData.StrawberryAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.StrawberryLocations.Add(strawberryLocString);

                    string[] area_mode_levelEntityID = strawberryLocString.Split(new char[]{'_'}, 3);
                    int area = Int32.Parse(area_mode_levelEntityID[0]);
                    int mode = Int32.Parse(area_mode_levelEntityID[1]);
                    string[] level_EntityID = area_mode_levelEntityID[2].Split(":");
                    int ID = Int32.Parse(level_EntityID[1]);

                    if (SaveData.Instance.Areas_Safe[area].Modes[mode].Strawberries.Add(new EntityID(level_EntityID[0], ID)))
                    {
                        SaveData.Instance.Areas_Safe[area].Modes[mode].TotalStrawberries += 1;
                    }
                }

                if (Locations.APLocationData.BinocularsAPToID.ContainsKey(newLoc))
                {
                    string binocularsLocString = Locations.APLocationData.BinocularsAPToID[newLoc];

                    Celeste_MultiworldModule.SaveData.BinocularsLocations.Add(binocularsLocString);
                }

                if (Locations.APLocationData.APToRoomName.ContainsKey(newLoc))
                {
                    string roomLocString = Locations.APLocationData.APToRoomName[newLoc];

                    Celeste_MultiworldModule.SaveData.RoomLocations.Add(roomLocString);
                }
            }

            CollectedLocations.Clear();
        }

        public void SendTrapLink(Items.Traps.TrapType trapType)
        {
            try
            {
                if (!this.Ready || !this.TrapLinkActive)
                {
                    return;
                }

                BouncePacket bouncePacket = new BouncePacket
                {
                    Tags = new List<string> { "TrapLink" },
                    Data = new Dictionary<string, JToken>
                    {
                        { "time", DateTime.UtcNow.ToUnixTimeStamp() },
                        { "source", GetPlayerName(this.Slot) },
                        { "trap_name", Items.APItemData.ItemIDToString[0xCA10000 + (int)trapType] }
                    }
                };

                _session.Socket.SendPacketAsync(bouncePacket);
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }

        public int GetInt(string key)
        {
            try
            {
                if (!_session.DataStorage[key])
                {
                    return 0;
                }

                return _session.DataStorage[key];
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }

            return 0;
        }

        public void Set(string key, int value)
        {
            try
            {
                var token = JToken.FromObject(value);
                _session.DataStorage[key] = token;
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }

        public void Set(string key, string value)
        {
            try
            {
                var token = JToken.FromObject(value);
                _session.DataStorage[key] = token;
            }
            catch (ArchipelagoSocketClosedException)
            {
                Disconnect();
            }
        }

        public void AddItemsRcvCallback(string key, Action<int> callback)
        {
            if (!ItemRcvCallbackSet)
            {
                ItemRcvCallbackSet = true;
                _session.DataStorage[key].OnValueChanged += (oldData, newData, _) => {
                    int newItemsRcv = JsonConvert.DeserializeObject<int>(newData.ToString());
                    callback(newItemsRcv);
                };
            }
        }

        public void ItemsRcvUpdated(int newItemsRcv)
        {
            this.ServerItemsRcv = newItemsRcv;
        }

        public void SetRoomStorage(string newRoom)
        {
            if (this.Slot != -1 && newRoom != this.StoredRoom)
            {
                this.StoredRoom = newRoom;
                this.Set($"Celeste_Open_Room_{_session.Players.GetPlayerName(this.Slot)}", newRoom);
                Logger.Verbose("AP", $"Set Celeste_Open_Room_{_session.Players.GetPlayerName(this.Slot)} to {newRoom}");
            }
        }

        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            if (this.Slot == -1)
            {
                return;
            }

            if (packet.PacketType == ArchipelagoPacketType.Bounced)
            {
                BouncedPacket bouncedPacket = packet as BouncedPacket;

                if (bouncedPacket.Tags.Contains("TrapLink") && this.TrapLinkActive && bouncedPacket.Data["source"].ToString() != GetPlayerName(this.Slot))
                {
                    string trap_name = bouncedPacket.Data["trap_name"].ToString();

                    if (Items.Traps.TrapManager.TrapLinkNames.ContainsKey(trap_name))
                    {
                        string message = $"Received Linked {{#FA8072}}{trap_name}{{#}} from {{#FAFAD2}}{bouncedPacket.Data["source"].ToString()}{{#}}.";

                        Items.Traps.TrapType type = Items.Traps.TrapManager.TrapLinkNames[trap_name];

                        if (Items.Traps.TrapManager.EnabledTraps[(int)(type)] == 0)
                        {
                            return;
                        }

                        Items.Traps.TrapManager.Instance.SetPriorityTrap(type, message);
                    }
                }
            }
            else if (packet.PacketType == ArchipelagoPacketType.Retrieved)
            {
                //if (_connectionInfo.SeeGhosts)
                {
                    RetrievedPacket retPacket = packet as RetrievedPacket;

                    foreach (KeyValuePair<string, JToken> entry in retPacket.Data)
                    {
                        if (entry.Key.StartsWith("Celeste_OtherPlayer_"))
                        {
                            //PlayerUpdated(entry.Key, entry.Value);
                        }
                    }
                }
            }
        }
    }
}
