using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Archipelago.MultiClient.Net;
using System.Runtime.InteropServices;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    public class OuiConnection : Oui
    {
        public static OuiConnection Instance;
        private TextMenu connectMenu;

        private bool waitingOnConnect = false;
        private Task<LoginFailure> connectTask;

        public OuiConnection()
        {
            Instance = this;
        }

        public override void Update()
        {
            if (connectTask != null && waitingOnConnect)
            {
                if (connectTask.IsCompleted)
                {
                    if (connectTask.Result == null)
                    {
                        BeginGame();
                        waitingOnConnect = false;
                        return;
                    }
                    else
                    {
                        (this.connectMenu.items[6] as TextMenu.Header).Title = "Connection Failed";
                        Logger.Error("AP", "Connection Failed.");
                        connectTask = null;
                        waitingOnConnect = false;
                        return;
                    }
                }
                else if (this.connectMenu != null)
                {
                    (this.connectMenu.items[6] as TextMenu.Header).Title = "Connecting...";
                    return;
                }
                else
                {
                    return;
                }
            }

            if (base.Selected &&
                !waitingOnConnect &&
                connectMenu != null &&
                connectMenu.Focused &&
                connectMenu.Selection == 7)
            {
                if (Input.MenuConfirm.Pressed)
                {
                    waitingOnConnect = true;
                    connectTask = ArchipelagoManager.Instance.TryConnect();
                }

                if (Input.MenuCancel.Pressed)
                {
                    Audio.Play("event:/ui/main/button_back");
                    base.Overworld.Goto<OuiMainMenu>();
                }
            }
        }

        public override IEnumerator Enter(Oui from)
        {
            TextInput.OnInput += modTextInput_OnInput;

            RefreshConnectionMenu();
            this.Visible = true;
            this.connectMenu.Visible = true;
            this.connectMenu.Focused = true;

            yield return null;
        }

        public override IEnumerator Leave(Oui next)
        {
            TextInput.OnInput -= modTextInput_OnInput;

            yield return Everest.SaveSettings();

            RefreshConnectionMenu();
            this.Visible = false;
            this.connectMenu.Visible = false;
            this.connectMenu.RemoveSelf();
            this.connectMenu = null;
        }

        private void modTextInput_OnInput(char obj)
        {
            if (this.connectMenu.Selection != 7)
            {
                string currentButtonText = (this.connectMenu.items[this.connectMenu.Selection] as TextMenu.Button).Label;
                if (obj == 8 && currentButtonText.Length > 0)
                {
                    (this.connectMenu.items[this.connectMenu.Selection] as TextMenu.Button).Label = currentButtonText.Remove(currentButtonText.Length - 1);
                }
                else if (obj >= 32 && obj <= 126)
                {
                    (this.connectMenu.items[this.connectMenu.Selection] as TextMenu.Button).Label += obj;
                }
                else if (obj == 13)
                {
                    this.connectMenu.Selection += 2;
                }

                Core.CoreModule.Settings.DebugConsole.Keys.Remove(Microsoft.Xna.Framework.Input.Keys.OemPeriod);
            }

            Celeste_MultiworldModule.Settings.Address = (this.connectMenu.items[1] as TextMenu.Button).Label;
            Celeste_MultiworldModule.Settings.SlotName = (this.connectMenu.items[3] as TextMenu.Button).Label;
            Celeste_MultiworldModule.Settings.Password = (this.connectMenu.items[5] as TextMenu.Button).Label;
        }

        public TextMenu CreateConnectMenu()
        {
            Type settings = typeof(Celeste_MultiworldModuleSettings);

            TextMenu retMenu = new TextMenu();
            retMenu.BatchMode = true;
            retMenu.CompactWidthMode = true;

            retMenu.Add(new TextMenu.Header("Address"));
            retMenu.Add(new TextMenu.Button(Celeste_MultiworldModule.Settings.Address));
            retMenu.Add(new TextMenu.Header("Slot Name"));
            retMenu.Add(new TextMenu.Button(Celeste_MultiworldModule.Settings.SlotName));
            retMenu.Add(new TextMenu.Header("Password"));
            retMenu.Add(new TextMenu.Button(Celeste_MultiworldModule.Settings.Password));
            retMenu.Add(new TextMenu.Header(""));

            TextMenu.Button connectButton = new TextMenu.Button("Connect");
            retMenu.Add(connectButton);

            return retMenu;
        }

        public void RefreshConnectionMenu()
        {
            int menuSelection = -1;
            Vector2 menuPos = Vector2.Zero;

            if (this.connectMenu != null)
            {
                menuSelection = this.connectMenu.Selection;
                menuPos = this.connectMenu.Position;
                Scene.Remove(connectMenu);
            }

            this.connectMenu = CreateConnectMenu();

            if (menuSelection != -1)
            {
                this.connectMenu.Position = menuPos;
                this.connectMenu.Selection = menuSelection;
            }

            Scene.Add(connectMenu);
        }

        public void BeginGame()
        {
            SaveData.TryDelete(144);
            SaveData.TryDeleteModSaveData(144);
            SaveData.Start(new SaveData
            {
                Name = Celeste_MultiworldModule.Settings.SlotName,
                AssistMode = false,
                VariantMode = false
            }, 144);


            if (SaveData.Instance != null)
            {
                foreach(AreaStats area in SaveData.Instance.Areas_Safe)
                {
                    AreaData areaData = AreaData.Areas[area.ID];
                    foreach(AreaModeStats areaMode in area.Modes)
                    {
                        areaMode.Completed = true;
                    }
                }

                SaveData.Instance.UnlockedAreas = 10;

                SaveData.Instance.AssistMode = true;

                if (ArchipelagoManager.Instance.DashShuffle == -1)
                {
                    Celeste_MultiworldModule.SaveData.Interactables[0xCA12024] = true;

                    Celeste_MultiworldModule.SaveData.UpDash = true;
                    Celeste_MultiworldModule.SaveData.UpRightDash = true;
                    Celeste_MultiworldModule.SaveData.RightDash = true;
                    Celeste_MultiworldModule.SaveData.DownRightDash = true;
                    Celeste_MultiworldModule.SaveData.DownDash = true;
                    Celeste_MultiworldModule.SaveData.DownLeftDash = true;
                    Celeste_MultiworldModule.SaveData.LeftDash = true;
                    Celeste_MultiworldModule.SaveData.UpLeftDash = true;
                }

                if (ArchipelagoManager.Instance.ClimbShuffle == -1)
                {
                    Celeste_MultiworldModule.SaveData.RightClimb = true;
                    Celeste_MultiworldModule.SaveData.LeftClimb = true;
                }

                if (ArchipelagoManager.Instance.CrouchShuffle == false)
                {
                    Celeste_MultiworldModule.SaveData.Crouch = true;
                }
            }

            (Scene as Overworld).Goto<OuiChapterSelect>();
        }
    }
}
