using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static Celeste.Tentacles;
using static MonoMod.InlineRT.MonoModRule;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    internal class OuiJournalMoves : OuiJournalPage
    {
        public OuiJournalMoves(OuiJournal journal) : base(journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(this);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            this.Journal = journal;

            string header = "MOVEMENT";

            this.PageTexture = "page";
            this.table = new OuiJournalPage.Table()
                .AddColumn(new OuiJournalPage.TextCell(header, new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f));

            bool haveUDash = Celeste_MultiworldModule.SaveData.UpDash;
            bool haveURDash = Celeste_MultiworldModule.SaveData.UpRightDash;
            bool haveRDash = Celeste_MultiworldModule.SaveData.RightDash;
            bool haveDRDash = Celeste_MultiworldModule.SaveData.DownRightDash;
            bool haveDDash = Celeste_MultiworldModule.SaveData.DownDash;
            bool haveDLDash = Celeste_MultiworldModule.SaveData.DownLeftDash;
            bool haveLDash = Celeste_MultiworldModule.SaveData.LeftDash;
            bool haveULDash = Celeste_MultiworldModule.SaveData.UpLeftDash;
            bool haveLClimb = Celeste_MultiworldModule.SaveData.LeftClimb;
            bool haveRClimb = Celeste_MultiworldModule.SaveData.RightClimb;
            bool haveCrouch = Celeste_MultiworldModule.SaveData.Crouch;

            this.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row1 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveULDash)
            {
                row1.Add(new OuiJournalPage.IconsCell("dasharrow05"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveUDash)
            {
                row1.Add(new OuiJournalPage.IconsCell("dasharrow06"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveURDash)
            {
                row1.Add(new OuiJournalPage.IconsCell("dasharrow07"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
            }

            this.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row2 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveLDash)
            {
                row2.Add(new OuiJournalPage.IconsCell("dasharrow04"));
            }
            else
            {
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            row2.Add(new OuiJournalPage.IconsCell("dash_maddy"));

            if (haveRDash)
            {
                row2.Add(new OuiJournalPage.IconsCell("dasharrow00"));
            }
            else
            {
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            this.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row3 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveDLDash)
            {
                row3.Add(new OuiJournalPage.IconsCell("dasharrow03"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveDDash)
            {
                row3.Add(new OuiJournalPage.IconsCell("dasharrow02"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveDRDash)
            {
                row3.Add(new OuiJournalPage.IconsCell("dasharrow01"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
            }

            this.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row4 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            row4.Add(new OuiJournalPage.IconsCell(haveLClimb ? "climb_l" : "climb_l_outline"));
            row4.Add(new OuiJournalPage.IconsCell(haveRClimb ? "climb_r" : "climb_r_outline"));
            row4.Add(new OuiJournalPage.IconsCell(haveCrouch ? "crouch" : "crouch_outline"));
        }

        public override void Redraw(Monocle.VirtualRenderTarget buffer)
        {
            base.Redraw(buffer);
            Monocle.Draw.SpriteBatch.Begin();
            this.table.Render(new Vector2(60f, 20f));
            Monocle.Draw.SpriteBatch.End();
        }

        private OuiJournalPage.Table table;
    }

    internal class OuiJournalInteractables : OuiJournalPage
    {
        public OuiJournalInteractables(OuiJournal journal, int level, int side) : base(journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(this);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            this.Journal = journal;
            this.level = level;
            this.side = side;

            bool showIndicators = Celeste_MultiworldModule.Settings.JournalItemIndicators;

            string header = "ITEMS";

            if (ArchipelagoManager.Instance.SplitInteractables == 1)
            {
                // Per Level
                header = $"{modJournal.level_to_str[level].ToUpper()} ITEMS";
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 2)
            {
                // Per Side
                header = $"{modJournal.mode_to_str[side].ToUpper()}-SIDE ITEMS";
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 3)
            {
                // Per Level and Side
                if (this.level == 10)
                {
                    header = $"{modJournal.level_to_str[level].ToUpper()} ITEMS";
                }
                else
                {
                    header = $"{modJournal.level_to_str[level].ToUpper()} {modJournal.mode_to_str[side].ToUpper()} ITEMS";
                }
            }

            this.PageTexture = "page";
            this.table = new OuiJournalPage.Table()
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.TextCell(header, new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false));

            int haveSpring              = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12000, this.level, this.side);
            int haveTrafficBlock        = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12001, this.level, this.side);
            int haveDashRefill          = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12018, this.level, this.side);
            int haveDoubleDashRefill    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12019, this.level, this.side);
            int havePinkCassetteBlock   = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12002, this.level, this.side);
            int haveBlueCassetteBlock   = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12003, this.level, this.side);
            int haveYellowCassetteBlock = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1201A, this.level, this.side);
            int haveGreenCassetteBlock  = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1201B, this.level, this.side);

            int haveDream       = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12004, this.level, this.side);
            int haveCoins       = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12005, this.level, this.side);
            int haveSeeds       = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1201F, this.level, this.side);
            int haveSinking     = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12020, this.level, this.side);
            int haveMoving      = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12006, this.level, this.side);
            int haveBlueBooster = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12007, this.level, this.side);
            int haveRedBooster  = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200B, this.level, this.side);
            int haveMoveBlock   = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12009, this.level, this.side);

            int haveBlueCloud     = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12008, this.level, this.side);
            int havePinkCloud     = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12010, this.level, this.side);
            int haveWhiteBlock    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12021, this.level, this.side);
            int haveSwapBlock     = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200A, this.level, this.side);
            int haveDashSwitch    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1201C, this.level, this.side);
            int haveSeekers       = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1201D, this.level, this.side);
            int haveTheo          = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200C, this.level, this.side);
            int haveTorches       = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12022, this.level, this.side);
            int haveYellowTorches = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12024, this.level, this.side);

            int haveFeather  = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200D, this.level, this.side);
            int haveBumper   = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200E, this.level, this.side);
            int haveKevin    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA1200F, this.level, this.side);
            int haveBadeline = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12011, this.level, this.side);

            int haveCoreBlock  = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12014, this.level, this.side);
            int haveCoreToggle = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12013, this.level, this.side);
            int haveFireIce    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12012, this.level, this.side);

            int havePuffer  = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12015, this.level, this.side);
            int haveJelly   = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12016, this.level, this.side);
            int haveBreaker = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12017, this.level, this.side);
            int haveBird    = Celeste_MultiworldModule.SaveData.HaveInteractableJournal(0xCA12023, this.level, this.side);

            OuiJournalPage.Row row1 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row2 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveSpring != 0)
            {
                string name = showIndicators ? (haveSpring == 2 ? "Springs [o]" : "Springs [x]") : "Springs";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveSpring == 2 ? "spring" : "spring_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveTrafficBlock != 0)
            {
                string name = showIndicators ? (haveTrafficBlock == 2 ? "Traffic\nBlocks [o]" : "Traffic\nBlocks [x]") : "Traffic\nBlocks";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveTrafficBlock == 2 ? "traffic" : "traffic_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveDashRefill != 0)
            {
                string name = showIndicators ? (haveDashRefill == 2 ? "Dash\nRefills [o]" : "Dash\nRefills [x]") : "Dash\nRefills";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveDashRefill == 2 ? "dash" : "dash_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (havePinkCassetteBlock != 0)
            {
                string name = showIndicators ? (havePinkCassetteBlock == 2 ? "Pink\nCassette\nBlocks [o]" : "Pink\nCassette\nBlocks [x]") : "Pink\nCassette\nBlocks";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(havePinkCassetteBlock == 2 ? "pink_cassette" : "pink_cassette_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBlueCassetteBlock != 0)
            {
                string name = showIndicators ? (haveBlueCassetteBlock == 2 ? "Blue\nCassette\nBlocks [o]" : "Blue\nCassette\nBlocks [x]") : "Blue\nCassette\nBlocks";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveBlueCassetteBlock == 2 ? "blue_cassette" : "blue_cassette_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveDream != 0)
            {
                string name = showIndicators ? (haveDream == 2 ? "Dream\nBlocks [o]" : "Dream\nBlocks [x]") : "Dream\nBlocks";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveDream == 2 ? "dream" : "dream_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveCoins != 0)
            {
                string name = showIndicators ? (haveCoins == 2 ? "Coins [o]" : "Coins [x]") : "Coins";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveCoins == 2 ? "coin" : "coin_outline"));
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveSeeds != 0)
            {
                string name = showIndicators ? (haveSeeds == 2 ? "Strawberry\nSeeds [o]" : "Strawberry\nSeeds [x]") : "Strawberry\nSeeds";
                row1.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row2.Add(new OuiJournalPage.IconsCell(haveSeeds == 2 ? "seed" : "seed_outline"));;
            }
            else
            {
                row1.Add(new OuiJournalPage.EmptyCell(64f));
                row2.Add(new OuiJournalPage.EmptyCell(64f));
            }

            OuiJournalPage.Row row3 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row4 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveSinking != 0)
            {
                string name = showIndicators ? (haveSinking == 2 ? "Sinking\nPlatforms\n[o]" : "Sinking\nPlatforms\n[x]") : "Sinking\nPlatforms";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveSinking == 2 ? "sinking" : "sinking_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveMoving != 0)
            {
                string name = showIndicators ? (haveMoving == 2 ? "Moving\nPlatforms\n[o]" : "Moving\nPlatforms\n[x]") : "Moving\nPlatforms";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveMoving == 2 ? "moving" : "moving_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBlueCloud != 0)
            {
                string name = showIndicators ? (haveBlueCloud == 2 ? "Blue\nClouds [o]" : "Blue\nClouds [x]") : "Blue\nClouds";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveBlueCloud == 2 ? "cloud_blue" : "cloud_blue_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (havePinkCloud != 0)
            {
                string name = showIndicators ? (havePinkCloud == 2 ? "Pink\nClouds [o]" : "Pink\nClouds [x]") : "Pink\nClouds";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(havePinkCloud == 2 ? "cloud_pink" : "cloud_pink_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBlueBooster != 0)
            {
                string name = showIndicators ? (haveBlueBooster == 2 ? "Blue\nBoosters\n[o]" : "Blue\nBoosters\n[x]") : "Blue\nBoosters";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveBlueBooster == 2 ? "booster_blue" : "booster_blue_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveMoveBlock != 0)
            {
                string name = showIndicators ? (haveMoveBlock == 2 ? "Move\nBlocks [o]" : "Move\nBlocks [x]") : "Move\nBlocks";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveMoveBlock == 2 ? "move_block" : "move_block_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveWhiteBlock != 0)
            {
                string name = showIndicators ? (haveWhiteBlock == 2 ? "White\nBlock [o]" : "White\nBlock [x]") : "White\nBlock";
                row3.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row4.Add(new OuiJournalPage.IconsCell(haveWhiteBlock == 2 ? "white_block" : "white_block_outline"));
            }
            else
            {
                row3.Add(new OuiJournalPage.EmptyCell(64f));
                row4.Add(new OuiJournalPage.EmptyCell(64f));
            }

            OuiJournalPage.Row row5 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row6 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveSwapBlock != 0)
            {
                string name = showIndicators ? (haveSwapBlock == 2 ? "Swap\nBlocks [o]" : "Swap\nBlocks [x]") : "Swap\nBlocks";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveSwapBlock == 2 ? "swap_block" : "swap_block_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveRedBooster != 0)
            {
                string name = showIndicators ? (haveRedBooster == 2 ? "Red\nBoosters\n[o]" : "Red\nBoosters\n[x]") : "Red\nBoosters";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveRedBooster == 2 ? "booster_red" : "booster_red_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveDashSwitch != 0)
            {
                string name = showIndicators ? (haveDashSwitch == 2 ? "Dash\nSwitches\n[o]" : "Dash\nSwitches\n[x]") : "Dash\nSwitches";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveDashSwitch == 2 ? "dash_switch" : "dash_switch_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveSeekers != 0)
            {
                string name = showIndicators ? (haveSeekers == 2 ? "Seekers\n[o]" : "Seekers\n[x]") : "Seekers";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveSeekers == 2 ? "seeker" : "seeker_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveTheo != 0)
            {
                string name = showIndicators ? (haveTheo == 2 ? "Theo\nCrystal [o]" : "Theo\nCrystal [x]") : "Theo\nCrystal";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveTheo == 2 ? "theo" : "theo_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveTorches != 0)
            {
                string name = showIndicators ? (haveTorches == 2 ? "Blue\nTorches\n[o]" : "Blue\nTorches\n[x]") : "Blue\nTorches";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveTorches == 2 ? "torch" : "torch_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveYellowTorches != 0)
            {
                string name = showIndicators ? (haveYellowTorches == 2 ? "Yellow\nTorches\n[o]" : "Yellow\nTorches\n[x]") : "Yellow\nTorches";
                row5.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row6.Add(new OuiJournalPage.IconsCell(haveYellowTorches == 2 ? "yellow_torch" : "yellow_torch_outline"));
            }
            else
            {
                row5.Add(new OuiJournalPage.EmptyCell(64f));
                row6.Add(new OuiJournalPage.EmptyCell(64f));
            }

            OuiJournalPage.Row row7 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row8 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveFeather != 0)
            {
                string name = showIndicators ? (haveFeather == 2 ? "Feathers\n[o]" : "Feathers\n[x]") : "Feathers";
                row7.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row8.Add(new OuiJournalPage.IconsCell(haveFeather == 2 ? "feather" : "feather_outline"));
            }
            else
            {
                row7.Add(new OuiJournalPage.EmptyCell(64f));
                row8.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBumper != 0)
            {
                string name = showIndicators ? (haveBumper == 2 ? "Bumpers\n[o]" : "Bumpers\n[x]") : "Bumpers";
                row7.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row8.Add(new OuiJournalPage.IconsCell(haveBumper == 2 ? "bumper" : "bumper_outline"));
            }
            else
            {
                row7.Add(new OuiJournalPage.EmptyCell(64f));
                row8.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveKevin != 0)
            {
                string name = showIndicators ? (haveKevin == 2 ? "Kevins [o]" : "Kevins [x]") : "Kevins";
                row7.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row8.Add(new OuiJournalPage.IconsCell(haveKevin == 2 ? "kevin" : "kevin_outline"));
            }
            else
            {
                row7.Add(new OuiJournalPage.EmptyCell(64f));
                row8.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBadeline != 0)
            {
                string name = showIndicators ? (haveBadeline == 2 ? "Badeline\nBoosters\n[o]" : "Badeline\nBoosters\n[x]") : "Badeline\nBoosters";
                row7.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row8.Add(new OuiJournalPage.IconsCell(haveBadeline == 2 ? "badeline" : "badeline_outline"));
            }
            else
            {
                row7.Add(new OuiJournalPage.EmptyCell(64f));
                row8.Add(new OuiJournalPage.EmptyCell(64f));
            }

            OuiJournalPage.Row row9 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row10 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveCoreBlock != 0)
            {
                string name = showIndicators ? (haveCoreBlock == 2 ? "Core\nBlocks [o]" : "Core\nBlocks [x]") : "Core\nBlocks";
                row9.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row10.Add(new OuiJournalPage.IconsCell(haveCoreBlock == 2 ? "core_block" : "core_block_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveCoreToggle != 0)
            {
                string name = showIndicators ? (haveCoreToggle == 2 ? "Core\nToggles [o]" : "Core\nToggles [x]") : "Core\nToggles";
                row9.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row10.Add(new OuiJournalPage.IconsCell(haveCoreToggle == 2 ? "core_toggle" : "core_toggle_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveFireIce != 0)
            {
                string name = showIndicators ? (haveFireIce == 2 ? "Fire\nand Ice\nBalls [o]" : "Fire\nand Ice\nBalls [x]") : "Fire\nand Ice\nBalls";
                row9.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row10.Add(new OuiJournalPage.IconsCell(haveFireIce == 2 ? "ice_ball" : "ice_ball_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            OuiJournalPage.Row row11 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));
            OuiJournalPage.Row row12 = this.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (haveDoubleDashRefill != 0)
            {
                string name = showIndicators ? (haveDoubleDashRefill == 2 ? "Double\nDash\nRefills [o]" : "Double\nDash\nRefills [x]") : "Double\nDash\nRefills";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveDoubleDashRefill == 2 ? "double_dash" : "double_dash_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (havePuffer != 0)
            {
                string name = showIndicators ? (havePuffer == 2 ? "Pufferfish\n[o]" : "Pufferfish\n[x]") : "Pufferfish";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(havePuffer == 2 ? "puffer" : "puffer_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveJelly != 0)
            {
                string name = showIndicators ? (haveJelly == 2 ? "Jellyfish\n[o]" : "Jellyfish\n[x]") : "Jellyfish";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveJelly == 2 ? "jelly" : "jelly_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBreaker != 0)
            {
                string name = showIndicators ? (haveBreaker == 2 ? "Breaker\nBoxes [o]" : "Breaker\nBoxes [x]") : "Breaker\nBoxes";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveBreaker == 2 ? "breaker" : "breaker_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveBird != 0)
            {
                string name = showIndicators ? (haveBird == 2 ? "Bird [o]" : "Bird [x]") : "Bird";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveBird == 2 ? "bird" : "bird_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveYellowCassetteBlock != 0)
            {
                string name = showIndicators ? (haveYellowCassetteBlock == 2 ? "Yellow\nCassette\nBlocks [o]" : "Yellow\nCassette\nBlocks [x]") : "Yellow\nCassette\nBlocks";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveYellowCassetteBlock == 2 ? "yellow_cassette" : "yellow_cassette_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }

            if (haveGreenCassetteBlock != 0)
            {
                string name = showIndicators ? (haveGreenCassetteBlock == 2 ? "Green\nCassette\nBlocks [o]" : "Green\nCassette\nBlocks [x]") : "Green\nCassette\nBlocks";
                row11.Add(new OuiJournalPage.TextCell(name, new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                row12.Add(new OuiJournalPage.IconsCell(haveGreenCassetteBlock == 2 ? "green_cassette" : "green_cassette_outline"));
            }
            else
            {
                row9.Add(new OuiJournalPage.EmptyCell(64f));
                row10.Add(new OuiJournalPage.EmptyCell(64f));
            }
        }

        public override void Redraw(Monocle.VirtualRenderTarget buffer)
        {
            base.Redraw(buffer);
            Monocle.Draw.SpriteBatch.Begin();
            this.table.Render(new Vector2(60f, 20f));
            Monocle.Draw.SpriteBatch.End();
        }

        private OuiJournalPage.Table table;

        private int side = 0;
        private int level = 0;
    }

    internal class modJournal
    {
        public static Dictionary<int, string> mode_to_str { get; set; } = new Dictionary<int, string>
        {
            { 0, "a" },
            { 1, "b" },
            { 2, "c" },
        };
        public static Dictionary<int, string> level_to_str { get; set; } = new Dictionary<int, string>
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

        public static List<List<string>> Poems { get; set; } = new List<List<string>>
        {
            new List<string> {
              "Know him well", "Has no style", "Finally back", "Fly real high",
              "Back again", "Listen up dudes", "Shrink in size", "Kick some tail",
              "Got style", "Pistols out", "About time too", "Leader of the bunch",
              "Has no grace", "Funny face", "Digs this tune", "Suit her mood",
            },
            new List<string> {
              "Tangle you up", "Hardly swallow", "Save your sorrow", "Paid in trade",
              "Facedown on the floor", "Find your way", "Can't help but follow", "Feel life",
              "Edge of tomorrow", "Back where you came", "Moving through your mind", "Slipping down your spine",
              "Feel time", "Works of yesterday", "Beg or borrow", "Fears and pain",
            },
            new List<string> {
              "Let you down", "Tell a lie", "Run around", "To shy to say it",
              "Strangers to love", "What's been going on", "Gonna play it", "Desert you",
              "Make you cry", "Say goodbye", "Give you up", "Hurt you",
              "Any other guy", "For so long", "Know the rules", "What I'm thinking of",
            },
            new List<string> {
              "History quickly crashing", "The school book", "Seldom mentioned", "Forcast to be falling",
              "Build a tent",  "Using you to fall", "Made me cross", "Only in the past",
              "Some stay dry", "A baby born", "The prisons", "It's the fear",
              "Makes us happy", "Raised your neighborhood", "Made you turn", "Zoom the camera out",
            },
            new List<string> {
              "She only knew", "Give up your children", "Act too soon", "Seal their fate",
              "Prophesy come true", "Separate", "Bide your time", "Lie in wait",
              "The children grow", "The throne awaits", "Learn what's right", "Seek their mother",
              "Triplets born", "Freedom fight", "A seer warns", "A deadly fate",
            },
            new List<string> {
              "Scaramouche", "Easy go", "Little high", "Need no sympathy",
              "Little silhouetto", "Easy come", "Galileo Figaro", "Escape from reality",
              "Open your eyes", "Real life", "Look up to the skies", "Just a poor boy",
              "Thunderbolt and lightning", "Fantasy", "Caught in a landslide", "Little low",
            },
            new List<string> {
              "Some new weyr", "Gone ahead", "Dead", "Dragons gone",
              "Gone away", "Open", "Leaving weyrs", "Worlds away",
              "Weyrfolk fled", "The empty weyr", "Cruel thread", "Herdbeasts free",
              "Dusty", "Echoes roll", "Empty", "Unanswered",
            },
            new List<string> {
              "The road has gone", "With eager feet", "I must follow", "I cannot say",
              "Pursuing it", "Now far ahead", "Down from the door", "Some larger way",
              "Where many paths", "Whither then", "If I can", "The road goes",
              "Errands meet", "Until it joins", "Ever on and on", "Where it began",
            },
            new List<string> {
              "Moonlight melts", "Ticking clock", "Lukewarm gloom", "Devouring moonlight",
              "Dreamless dorm", "Clench my fists", "Windless night", "Far in mist",
              "Voiceless town", "Tapping feet", "Pockets tight", "From the soundless room",
              "Ghostly shadow", "Tower waits", "I walk away", "Merciless tomb",
            },
            new List<string> {
              "The sun arising", "Greatness thrusts", "Into our lives", "Hear you breathing",
              "Do you think", "Heart beats", "Blowing me around", "Let destiny choose",
              "Fire in the sky", "Makes me feel alive", "See you coming", "Only if I lose",
              "Going fast", "You can win", "Hyperdrive", "Feel the wind",
            },
            new List<string> {
              "Donkey Kong 3", "Harvest Moon", "ActRaiser, Blazing Lazers, Bases Loaded, Mega Turrican", "Bubble Bobble, Double Dribble, Double Dragon, F-Zero",
              "Cruisin' USA", "Sonic the Hedgehog", "Toejam & Earl in Panic on Funkotron", "Cybernator, Roling Thunder, Dynastic Hero",
              "King's Knight, Dig Dug, Chew Man Fu", "Adventures of Lolo 1 and 2", "League Puzzle Pokemon", "Castlevania, Fatal Fury, Ninja JajaMaru-kun",
              "Super Mario RPG", "F-Zero X", "Donkey Kong Jr. Math", "Ninja Gaiden 1, Ninja Gaiden 2, Ninja Gaiden 3",
            },
            new List<string> {
              "The true Lord of the Dance", "A gallon of Strawberry Quik", "Unexpected trip", "Tomorrow you'll wake up",
              "Friendly and intelligent (except for you)", "Lock my doors and windows", "Meaning that exclusively applies to only you", "Ernest Borgnine",
              "Explosive Flatulence", "Inconceivable, or at the very least a bit unlikely", "Relative position of the planets", "Face down in the mud",
              "Forty-pound Watermelon", "Every single one of them is absolutely true", "Travel in your future", "Laughter is the very best medicine",
            },
            new List<string> {
              "Its earted jurtles, grumbling", "On a lurgid bee", "Or else I shall rend thee in the gobberwarts", "That mordiously hath blurted out",
              "And hooptiously drangle me", "Now the jurpling slayjid agrocrustles", "As plurdled gabbleblotchits", "See if I don't!",
              "Groop, I implore thee", "Like jowling meated liverslime", "And living glupules frart and stipulate", "my foonting turlingdromes",
              "Are slurping hagrilly up the axlegrurts", "Thy micturations are to me", "Oh freddled gruntbuggly", "With crinkly bindlewurdles",
            },
            new List<string> {
              "Won the Golden Honeycomb", "Yellow oranges", "A pretty butterfly", "The rivers seventeen",
              "Battled with the Dumbledors", "A weather-driven mariner", "Winds of argosies", "Studied wizardry",
              "Merry passenger", "Perfumed her with marjoram", "Gems and necklaces", "Shield and morion",
              "Gilded gondola", "All loneliness", "Passed the archipelagoes", "Tarried for a little while",
            },
            new List<string> {
              "And in the dark, and on a train", "I do! I like them, Sam-I-Am!", "And I will eat them with a fox", "And I would eat them with a goat",
              "Thank you! Thank you, Sam-I-Am!", "So I will eat them in a box", "I do so like green eggs and ham!", "They are so good, so good, you see!",
              "And I would eat them in a boat", "And I will eat them here and there", "Say! I like green eggs and ham!", "And in a car, and in a tree",
              "And I will eat them with a mouse", "And I will eat them in the rain", "And I will eat them in a house", "Say! I will eat them anywhere!",
            },
            new List<string> {
              "Grab your pick", "You think you're safe", "Blows up", "Shame it's gotta end",
              "A grueling one", "Diamonds tonight", "Hear a sound, turn", "Creeper's tryna steal",
              "Don't die, die, die", "Side to side", "That's a nice life", "Pickaxe swinging",
              "Right behind", "Back in the mine", "Never forget those eyes", "Overhear some hissing",
            },
            new List<string> {
              "Flapping big ears", "Huge, old, and tall", "Trees crack as I pass", "Never lie on the ground",
              "I make the earth shake", "Biggest of all", "Not even to die", "Nose like a snake",
              "I walk in the south", "Grey as a mouse", "As I tramp through the grass", "Big as a house",
              "I stump round and round", "Beyond count of years", "With horns in my mouth", "Oliphaunt am I",
            },
            new List<string> {
              "Bio-Dome with Pauly Shore", "Big bowl of sauerkraut", "Dr Pepper and salted peanuts", "Nathaniel and Superfly",
              "Real airplane", "Zelda", "Tray table up", "Tenor saxophone",
              "First class one-way ticket", "Still a little place", "Box under the stairs", "Warm root beer",
              "One dozen starving, crazed weasels", "Little chocolate mint", "Bear claws", "My lucky snorkel",
            },
            new List<string> {
              "Tonic and gin", "Manager gives me a smile", "Drinks for free", "Smile ran away",
              "John at the bar", "Younger man's clothes", "What are you doin' here?", "Nine o'clock on a Saturday",
              "Pretty good crowd", "Still in the navy", "Get out of this place", "Better than drinkin' alone",
              "Play me a memory", "Sounds like a carnival", "Drink they call loneliness", "Real estate novelist",
            },
            new List<string> {
              "Spire of flame", "Cold of the night", "Bear this torch", "Scattering ashes",
              "Search your soul", "Silence grows", "Past still unnamed", "Cling on to life",
              "Colors weave", "Distant sparks", "Glimmering shadows", "Caught in the struggle",
              "Reawaken the undying light", "Sky fell away", "Storms of change", "Chorus of souls",
            },
        };

        public static List<List<int>> PoemOrders { get; set; } = new List<List<int>>
        {
            new List<int> {
              11, 0, 2, 7,
              8, 5, 6, 15,
              1, 12, 13, 14,
              4, 10, 3, 9,
            },
            new List<int> {
              7, 10, 12, 11,
              1, 15, 6, 9,
              8, 13, 14, 5,
              0, 4, 2, 3,
            },
            new List<int> {
              4, 14, 15, 12,
              10, 0, 2, 7,
              8, 9, 1, 11,
              13, 3, 5, 6,
            },
            new List<int> {
              8, 9, 1, 10,
              4, 15, 3, 7,
              13, 12, 6, 14,
              0, 5, 2, 11,
            },
            new List<int> {
              12, 9, 14, 15,
              1, 5, 6, 7,
              8, 10, 13, 11,
              0, 4, 2, 3,
            },
            new List<int> {
              9, 13, 14, 7,
              8, 10, 11, 3,
              5, 1, 2, 15,
              4, 0, 12, 6
            },
            new List<int> {
              4, 1, 13, 15,
              14, 5, 12, 2,
              8, 3, 6, 11,
              0, 10, 7, 9,
            },
            new List<int> {
              11, 14, 6, 15,
              5, 0, 2, 10,
              4, 1, 13, 7,
              8, 12, 9, 3,
            },
            new List<int> {
              4, 1, 14, 11,
              6, 0, 12, 2,
              8, 9, 5, 10,
              7, 13, 15, 3,
            },
            new List<int> {
              12, 9, 5, 14,
              4, 13, 11, 7,
              3, 10, 15, 6,
              0, 8, 1, 2,
            },
            new List<int> {
              12, 5, 0, 9, 8, 1, 10, 6, 11, 2, 7, 3, 13, 14, 15, 4
            },
            new List<int> {
              14, 0, 12, 3, 8, 11, 1, 4, 9, 10, 6, 13, 15, 2, 7, 5
            },
            new List<int> {
              14, 13, 6, 1, 3, 0, 5, 12, 10, 9, 8, 11, 4, 15, 2, 7
            },
            new List<int> {
              8, 12, 1, 9, 6, 3, 13, 2, 7, 10, 14, 11, 4, 0, 15, 5
            },
            new List<int> {
              10, 1, 8, 3, 13, 0, 11, 7, 5, 2, 14, 12, 9, 15, 6, 4
            },
            new List<int> {
              13, 11, 9, 4, 5, 6, 14, 7, 0, 1, 15, 12, 10, 3, 2, 8
            },
            new List<int> {
              9, 11, 7, 4, 10, 2, 14, 8, 0, 13, 12, 3, 6, 15, 5, 1
            },
            new List<int> {
              10, 1, 11, 8, 4, 2, 0, 6, 7, 13, 15, 14, 12, 5, 3, 9
            },
            new List<int> {
              7, 0, 12, 5, 4, 2, 3, 10, 15, 9, 14, 11, 8, 1, 13, 6
            },
            new List<int> {
              8, 0, 9, 6, 2, 1, 4, 12, 13, 10, 5, 15, 7, 11, 14, 3
            },
        };

        public void Load()
        {
            On.Celeste.OuiJournal.Enter += modOuiJournal_Enter;
            On.Celeste.OuiJournalProgress.ctor += modOuiJournalProgress_ctor;
            On.Celeste.OuiJournalSpeedrun.ctor += modOuiJournalSpeedrun_ctor;
            On.Celeste.OuiJournalDeaths.ctor += modOuiJournalDeaths_ctor;
            On.Celeste.OuiJournalPoem.ctor += modOuiJournalPoem_ctor;
            On.Celeste.OuiJournalPoem.Swap += modOuiJournalPoem_Swap;
            On.Celeste.OuiJournalGlobal.ctor += modOuiJournalGlobal_ctor;
            On.Celeste.OuiJournalGlobal.Redraw += modOuiJournalGlobal_Redraw;
        }

        public void Unload()
        {
            On.Celeste.OuiJournal.Enter -= modOuiJournal_Enter;
            On.Celeste.OuiJournalProgress.ctor -= modOuiJournalProgress_ctor;
            On.Celeste.OuiJournalSpeedrun.ctor -= modOuiJournalSpeedrun_ctor;
            On.Celeste.OuiJournalDeaths.ctor -= modOuiJournalDeaths_ctor;
            On.Celeste.OuiJournalPoem.ctor -= modOuiJournalPoem_ctor;
            On.Celeste.OuiJournalPoem.Swap -= modOuiJournalPoem_Swap;
            On.Celeste.OuiJournalGlobal.ctor -= modOuiJournalGlobal_ctor;
            On.Celeste.OuiJournalGlobal.Redraw -= modOuiJournalGlobal_Redraw;
        }

        private static System.Collections.IEnumerator modOuiJournal_Enter(On.Celeste.OuiJournal.orig_Enter orig, OuiJournal self, Oui from)
        {
            yield return orig(self, from);

            self.Pages.Clear();
            self.Pages.Add(new OuiJournalCover(self));
            self.Pages.Add(new OuiJournalProgress(self));
            self.Pages.Add(new OuiJournalDeaths(self));
            self.Pages.Add(new OuiJournalPoem(self));
            self.Pages.Add(new OuiJournalGlobal(self));
            self.Pages.Add(new OuiJournalMoves(self));

            if (ArchipelagoManager.Instance.ExistentInteractables.Count() == 0)
            {
                // Left in for backwards compatibility with v1.0.x seeds
                self.Pages.Add(new OuiJournalSpeedrun(self));
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 0)
            {
                self.Pages.Add(new OuiJournalInteractables(self, -1, -1));
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 1)
            {
                // Per Level
                for (int level = 0; level <= 10; level++)
                {
                    if (level == 0 || level == 8)
                    {
                        continue;
                    }
                    else if (!ArchipelagoManager.Instance.ActiveLevels.Contains($"{level}a") && !ArchipelagoManager.Instance.ActiveLevels.Contains($"{level}b") && !ArchipelagoManager.Instance.ActiveLevels.Contains($"{level}c"))
                    {
                        continue;
                    }

                    self.Pages.Add(new OuiJournalInteractables(self, level, -1));
                }
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 2)
            {
                // Per Side
                self.Pages.Add(new OuiJournalInteractables(self, -1, 0));

                if (ArchipelagoManager.Instance.IncludeBSides || ArchipelagoManager.Instance.GoalLevel == "7b" ||ArchipelagoManager.Instance.GoalLevel == "9b")
                {
                    self.Pages.Add(new OuiJournalInteractables(self, -1, 1));
                }
                if (ArchipelagoManager.Instance.IncludeCSides || ArchipelagoManager.Instance.GoalLevel == "7c" || ArchipelagoManager.Instance.GoalLevel == "9c")
                {
                    self.Pages.Add(new OuiJournalInteractables(self, -1, 2));
                }
            }
            else if (ArchipelagoManager.Instance.SplitInteractables == 3)
            {
                // Per Level and Side
                for (int level = 0; level <= 10; level++)
                {
                    if (level == 0 || level == 8)
                    {
                        continue;
                    }
                    else if (level == 10)
                    {
                        self.Pages.Add(new OuiJournalInteractables(self, level, 0));
                        break;
                    }

                    for (int side = 0; side <= 2; side++)
                    {
                        if (ArchipelagoManager.Instance.ActiveLevels.Contains($"{level}{mode_to_str[side]}"))
                        {
                            self.Pages.Add(new OuiJournalInteractables(self, level, side));
                        }
                    }
                }
            }

            int num = 0;
            foreach (OuiJournalPage ouiJournalPage in self.Pages)
            {
                ouiJournalPage.PageIndex = num++;
            }
            self.Pages[0].Redraw(self.CurrentPageBuffer);

            yield break;
        }

        private static void modOuiJournalProgress_ctor(On.Celeste.OuiJournalProgress.orig_ctor orig, OuiJournalProgress self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("LOCATIONS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false)).AddColumn(new OuiJournalPage.EmptyCell(20f)).AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(100f))
                .AddColumn(new OuiJournalPage.IconCell("strawberry", 150f))
                .AddColumn(new OuiJournalPage.IconCell("checkpoint", 100f))
                .AddColumn(new OuiJournalPage.IconCell("key", 100f))
                .AddColumn(new OuiJournalPage.IconCell("binoculars", 100f))
                .AddColumn(new OuiJournalPage.IconCell("house", 100f));

            int AllLocationsDone = ArchipelagoManager.Instance.LocationsCheckedCount();
            int AllLocationsTotal = ArchipelagoManager.Instance.LocationsTotalCount();

            foreach (AreaStats areaStats in SaveData.Instance.Areas_Safe)
            {
                AreaData areaData = AreaData.Get(areaStats.ID_Safe);
                if (!areaData.Interlude_Safe)
                {
                    if (!ArchipelagoManager.Instance.ActiveLevels.Contains($"{areaStats.ID}a"))
                    {
                        continue;
                    }

                    int CheckpointDone = 0;
                    int KeyDone = 0;
                    int BinocularsDone = 0;
                    int RoomDone = 0;
                    int CheckpointTotal = 0;
                    int KeyTotal = 0;
                    int BinocularsTotal = 0;
                    int RoomTotal = 0;

                    foreach (string level in ArchipelagoManager.Instance.ActiveLevels)
                    {
                        string level_area = level.TrimEnd(level[level.Length - 1]);
                        if (level_area == $"{areaStats.ID}")
                        {
                            int mode = 0;

                            if (level.EndsWith("b"))
                            {
                                mode = 1;
                            }
                            else if (level.EndsWith("c"))
                            {
                                mode = 2;
                            }

                            CheckpointDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Checkpoint, areaStats.ID, mode);
                            KeyDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Key, areaStats.ID, mode);
                            BinocularsDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Binoculars, areaStats.ID, mode);
                            RoomDone += Locations.APLocationData.GetLocationCount(Locations.LocationType.Room, areaStats.ID, mode);

                            CheckpointTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Checkpoint, areaStats.ID, mode);
                            KeyTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Key, areaStats.ID, mode);
                            BinocularsTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Binoculars, areaStats.ID, mode);
                            RoomTotal += Locations.APLocationData.GetLocationTotal(Locations.LocationType.Room, areaStats.ID, mode);
                        }
                    }

                    // Strawberries
                    string text;
                    if (areaData.Mode[0].TotalStrawberries > 0 || areaStats.TotalStrawberries > 0)
                    {
                        text = areaStats.TotalStrawberries.ToString();
                        if (areaStats.Modes[0].Completed)
                        {
                            text = text + "/" + areaData.Mode[0].TotalStrawberries;
                        }
                    }
                    else
                    {
                        text = "-";
                    }

                    // Level Clear
                    string completionIcon = "dot";
                    if (Celeste_MultiworldModule.SaveData.LevelClearLocations.Contains($"{areaStats.ID}_{0}_Clear"))
                    {
                        if (!AreaData.Get(areaStats.ID_Safe).CanFullClear)
                        {
                            // Farewell
                            completionIcon = "beat";
                        }
                        else
                        {
                            completionIcon = "clear";
                        }
                    }
                    OuiJournalPage.IconsCell iconsCell;
                    OuiJournalPage.Row row = self.table.AddRow().Add(new OuiJournalPage.TextCell(Dialog.Clean(areaData.Name, null), new Vector2(1f, 0.5f), 0.6f, self.TextColor, 0f, false)).Add(null)
                        .Add(iconsCell = new OuiJournalPage.IconsCell(completionIcon));

                    // Crystal Hearts
                    List<string> list = new List<string>();
                    if (areaStats.Modes[0].HeartGem)
                    {
                        list.Add("heartgem" + 0);
                    }
                    for (int i = 1; i < areaStats.Modes.Length; i++)
                    {
                        if (Celeste_MultiworldModule.SaveData.LevelClearLocations.Contains($"{areaStats.ID}_{i}_Clear"))
                        {
                            list.Add("heartgem" + i);
                        }
                    }
                    if (list.Count <= 0)
                    {
                        list.Add("dot");
                    }

                    // Cassette
                    if (areaData.CanFullClear)
                    {
                        row.Add(new OuiJournalPage.IconsCell(new string[] { areaStats.Cassette ? "cassette" : "dot" }));
                        row.Add(new OuiJournalPage.IconsCell(-32f, list.ToArray()));
                    }
                    else
                    {
                        iconsCell.SpreadOverColumns = 3;
                        row.Add(null).Add(null);
                    }
                    row.Add(new OuiJournalPage.TextCell(text, self.TextJustify, 0.5f, self.TextColor, 0f, false));

                    if (CheckpointTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{CheckpointDone} / {CheckpointTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (KeyTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{KeyDone} / {KeyTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (ArchipelagoManager.Instance.Binosanity && BinocularsTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{BinocularsDone} / {BinocularsTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }

                    if (ArchipelagoManager.Instance.Roomsanity && RoomTotal != 0)
                    {
                        row.Add(new OuiJournalPage.TextCell($"{RoomDone} / {RoomTotal}", self.TextJustify, 0.5f, self.TextColor, 0f, false));
                    }
                    else
                    {
                        row.Add(new OuiJournalPage.IconCell("dot", 0f));
                    }
                }
            }

            // Totals Row
            if (self.table.Rows > 1)
            {
                self.table.AddRow();
                OuiJournalPage.Row row2 = self.table.AddRow().Add(new OuiJournalPage.TextCell(Dialog.Clean("journal_totals", null), new Vector2(1f, 0.5f), 0.7f, self.TextColor, 0f, false)).Add(null)
                    .Add(new OuiJournalPage.TextCell($"{AllLocationsDone} / {AllLocationsTotal}", self.TextJustify, 0.6f, self.TextColor, 100f, true)
                    {
                        SpreadOverColumns = 3
                    });

                for (int l = 0; l < 4; l++)
                {
                    row2.Add(null);
                }

                row2.Add(new OuiJournalPage.TextCell("DEATHS", self.TextJustify, 0.6f, self.TextColor, 70f, true)
                {
                    SpreadOverColumns = 2
                });

                row2.Add(null);

                row2.Add(new OuiJournalPage.TextCell(Dialog.Deaths(SaveData.Instance.TotalDeaths), self.TextJustify, 0.6f, self.TextColor, 0f, false));

                self.table.AddRow();
            }
        }

        private static void modOuiJournalSpeedrun_ctor(On.Celeste.OuiJournalSpeedrun.orig_ctor orig, OuiJournalSpeedrun self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("ITEMS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f));

            bool haveSpring = false;
            bool haveTrafficBlock = false;
            bool haveDashRefill = false;
            bool haveDoubleDashRefill = false;
            bool havePinkCassetteBlock = false;
            bool haveBlueCassetteBlock = false;
            bool haveYellowCassetteBlock = false;
            bool haveGreenCassetteBlock = false;

            bool haveDream = false;
            bool haveCoins = false;
            bool haveSeeds = false;
            bool haveSinking = false;
            bool haveMoving = false;
            bool haveBlueBooster = false;
            bool haveRedBooster = false;
            bool haveMoveBlock = false;

            bool haveBlueCloud = false;
            bool havePinkCloud = false;
            bool haveWhiteBlock = false;
            bool haveSwapBlock = false;
            bool haveDashSwitch = false;
            bool haveSeekers = false;
            bool haveTheo = false;
            bool haveTorches = false;

            bool haveFeather = false;
            bool haveBumper = false;
            bool haveKevin = false;
            bool haveBadeline = false;

            bool haveCoreBlock = false;
            bool haveCoreToggle = false;
            bool haveFireIce = false;

            bool havePuffer = false;
            bool haveJelly = false;
            bool haveBreaker = false;
            bool haveBird = false;

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12000, out haveSpring);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12001, out haveTrafficBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12018, out haveDashRefill);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12019, out haveDoubleDashRefill);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12002, out havePinkCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12003, out haveBlueCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201A, out haveYellowCassetteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201B, out haveGreenCassetteBlock);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12004, out haveDream);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12005, out haveCoins);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201F, out haveSeeds);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12020, out haveSinking);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12006, out haveMoving);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12007, out haveBlueBooster);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200B, out haveRedBooster);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12009, out haveMoveBlock);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12008, out haveBlueCloud);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12010, out havePinkCloud);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12021, out haveWhiteBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200A, out haveSwapBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201C, out haveDashSwitch);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1201D, out haveSeekers);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200C, out haveTheo);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12022, out haveTorches);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200D, out haveFeather);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200E, out haveBumper);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA1200F, out haveKevin);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12011, out haveBadeline);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12014, out haveCoreBlock);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12013, out haveCoreToggle);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12012, out haveFireIce);

            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12015, out havePuffer);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12016, out haveJelly);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12017, out haveBreaker);
            Celeste_MultiworldModule.SaveData.Interactables.TryGetValue(0xCA12023, out haveBird);

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Springs",                new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Traffic\nBlocks",        new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Dash\nRefills",          new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Pink\nCassette\nBlocks", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Blue\nCassette\nBlocks", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Dream\nBlocks",          new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Coins",                  new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Strawberry\nSeeds",      new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            OuiJournalPage.Row row1 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSpring            ? "spring" : "spring_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTrafficBlock      ? "traffic" : "traffic_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDashRefill        ? "dash" : "dash_outline"))
                .Add(new OuiJournalPage.IconsCell(havePinkCassetteBlock ? "pink_cassette" : "pink_cassette_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueCassetteBlock ? "blue_cassette" : "blue_cassette_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDream             ? "dream" : "dream_outline"))
                .Add(new OuiJournalPage.IconsCell(haveCoins             ? "coin" : "coin_outline"))
                .Add(new OuiJournalPage.IconsCell(haveSeeds             ? "seed" : "seed_outline"));

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Sinking\nPlatforms", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Moving\nPlatforms",  new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Blue\nClouds",       new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Pink\nClouds",       new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Blue\nBoosters",     new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Move\nBlocks",       new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("White\nBlock",       new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            OuiJournalPage.Row row2 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSinking     ? "sinking" : "sinking_outline"))
                .Add(new OuiJournalPage.IconsCell(haveMoving      ? "moving" : "moving_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueCloud   ? "cloud_blue" : "cloud_blue_outline"))
                .Add(new OuiJournalPage.IconsCell(havePinkCloud   ? "cloud_pink" : "cloud_pink_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBlueBooster ? "booster_blue" : "booster_blue_outline"))
                .Add(new OuiJournalPage.IconsCell(haveMoveBlock   ? "move_block" : "move_block_outline"))
                .Add(new OuiJournalPage.IconsCell(haveWhiteBlock  ? "white_block" : "white_block_outline"));

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Swap\nBlocks",   new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Red\nBoosters",  new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Dash\nSwitches", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Seekers",        new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Theo\nCrystal",  new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Torches",        new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            OuiJournalPage.Row row3 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveSwapBlock  ? "swap_block" : "swap_block_outline"))
                .Add(new OuiJournalPage.IconsCell(haveRedBooster ? "booster_red" : "booster_red_outline"))
                .Add(new OuiJournalPage.IconsCell(haveDashSwitch ? "dash_switch" : "dash_switch_outline"))
                .Add(new OuiJournalPage.IconsCell(haveSeekers    ? "seeker" : "seeker_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTheo       ? "theo" : "theo_outline"))
                .Add(new OuiJournalPage.IconsCell(haveTorches    ? "torch" : "torch_outline"));

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Feathers",           new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Bumpers",            new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Kevins",             new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Badeline\nBoosters", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            OuiJournalPage.Row row6 = self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.IconsCell(haveFeather  ? "feather" : "feather_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBumper   ? "bumper" : "bumper_outline"))
                .Add(new OuiJournalPage.IconsCell(haveKevin    ? "kevin" : "kevin_outline"))
                .Add(new OuiJournalPage.IconsCell(haveBadeline ? "badeline" : "badeline_outline"));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("9a"))
            {
                self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.TextCell("Core\nBlocks",         new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Core\nToggles",        new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Fire\nand Ice\nBalls", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

                OuiJournalPage.Row row8 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.IconsCell(haveCoreBlock  ? "core_block" : "core_block_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveCoreToggle ? "core_toggle" : "core_toggle_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveFireIce    ? "ice_ball" : "ice_ball_outline"));
            }
            else if (ArchipelagoManager.Instance.ActiveLevels.Contains("10b"))
            {
                self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.TextCell("Core\nBlocks",         new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Fire\nand Ice\nBalls", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

                OuiJournalPage.Row row8 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.IconsCell(haveCoreBlock  ? "core_block" : "core_block_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveFireIce    ? "ice_ball" : "ice_ball_outline"));
            }

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("10a"))
            {
                OuiJournalPage.Row row8_5 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.TextCell("Double\nDash\nRefills", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Pufferfish",            new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Jellyfish",             new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Breaker\nBoxes",        new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

                if (ArchipelagoManager.Instance.ActiveLevels.Contains("10b"))
                {
                    row8_5.Add(new OuiJournalPage.TextCell("Bird", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                        .Add(new OuiJournalPage.TextCell("Yellow\nCassette\nBlocks", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                        .Add(new OuiJournalPage.TextCell("Green\nCassette\nBlocks",  new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));
                }

                OuiJournalPage.Row row9 = self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.IconsCell(haveDoubleDashRefill    ? "double_dash" : "double_dash_outline"))
                    .Add(new OuiJournalPage.IconsCell(havePuffer              ? "puffer" : "puffer_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveJelly               ? "jelly" : "jelly_outline"))
                    .Add(new OuiJournalPage.IconsCell(haveBreaker             ? "breaker" : "breaker_outline"));

                if (ArchipelagoManager.Instance.ActiveLevels.Contains("10b"))
                {
                    row9.Add(new OuiJournalPage.IconsCell(haveBird                ? "bird" : "bird_outline"))
                        .Add(new OuiJournalPage.IconsCell(haveYellowCassetteBlock ? "yellow_cassette" : "yellow_cassette_outline"))
                        .Add(new OuiJournalPage.IconsCell(haveGreenCassetteBlock  ? "green_cassette" : "green_cassette_outline"));
                }
            }
        }

        private static void modOuiJournalDeaths_ctor(On.Celeste.OuiJournalDeaths.orig_ctor orig, OuiJournalDeaths self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("KEYS", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f))
                .AddColumn(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Front\nDoor Key",         new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Hallway\nKey 1",          new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Hallway\nKey 2",          new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Huge\nMess Key",          new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Presidential\nSuite Key", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            bool have_3_1 = false;
            bool have_3_2 = false;
            bool have_3_3 = false;
            bool have_3_4 = false;
            bool have_3_5 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16000, out have_3_1);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16001, out have_3_2);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16002, out have_3_3);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16003, out have_3_4);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16004, out have_3_5);

            OuiJournalPage.Row row3 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Celestial Resort A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_3_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_2 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_3 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_4 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_3_5 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("Entrance\nKey", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Depths\nKey",   new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Search\nKey 1", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Search\nKey 2", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                .Add(new OuiJournalPage.TextCell("Search\nKey 3", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            bool have_5_1 = false;
            bool have_5_2 = false;
            bool have_5_3 = false;
            bool have_5_4 = false;
            bool have_5_5 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16005, out have_5_1);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16006, out have_5_2);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16007, out have_5_3);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16008, out have_5_4);
            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16009, out have_5_5);

            OuiJournalPage.Row row5 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Mirror Temple A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_5_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_2 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_3 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_4 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.IconsCell(have_5_5 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("5b"))
            {
                self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.TextCell("Central\nChamber\nKey 1", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Central\nChamber\nKey 2", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

                bool have_5b_1 = false;
                bool have_5b_2 = false;

                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600A, out have_5b_1);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600B, out have_5b_2);

                OuiJournalPage.Row row5b = self.table.AddRow()
                    .Add(new OuiJournalPage.TextCell($"Mirror Temple B", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                    .Add(new OuiJournalPage.IconsCell(have_5b_1 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_5b_2 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.EmptyCell(64f));
            }

            self.table.AddRow()
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.TextCell("2500M\nKey", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

            bool have_7_1 = false;

            Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600C, out have_7_1);

            OuiJournalPage.Row row7 = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"The Summit A", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_7_1 ? "key" : "key_outline"))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f))
                .Add(new OuiJournalPage.EmptyCell(64f));

            self.table.AddRow().Add(new OuiJournalPage.EmptyCell(64f));

            bool have_gem_1 = false;
            bool have_gem_2 = false;
            bool have_gem_3 = false;
            bool have_gem_4 = false;
            bool have_gem_5 = false;
            bool have_gem_6 = false;

            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A00, out have_gem_1);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A01, out have_gem_2);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A02, out have_gem_3);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A03, out have_gem_4);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A04, out have_gem_5);
            Celeste_MultiworldModule.SaveData.GemItems.TryGetValue(0xCA16A05, out have_gem_6);

            OuiJournalPage.Row rowGem = self.table.AddRow()
                .Add(new OuiJournalPage.TextCell($"Gems", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                .Add(new OuiJournalPage.IconsCell(have_gem_1 ? "gem1" : "gem1_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_2 ? "gem2" : "gem2_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_3 ? "gem3" : "gem3_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_4 ? "gem4" : "gem4_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_5 ? "gem5" : "gem5_outline"))
                .Add(new OuiJournalPage.IconsCell(have_gem_6 ? "gem6" : "gem6_outline"));

            if (ArchipelagoManager.Instance.ActiveLevels.Contains("10a"))
            {
                self.table.AddRow()
                    .Add(new OuiJournalPage.EmptyCell(64f))
                    .Add(new OuiJournalPage.TextCell("Power\nSource\nKey 1", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Power\nSource\nKey 2", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Power\nSource\nKey 3", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Power\nSource\nKey 4", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true))
                    .Add(new OuiJournalPage.TextCell("Power\nSource\nKey 5", new Vector2(0.5f, 0.5f), 0.32f, Color.Black * 0.9f, 64f, true));

                bool have_10_1 = false;
                bool have_10_2 = false;
                bool have_10_3 = false;
                bool have_10_4 = false;
                bool have_10_5 = false;

                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600D, out have_10_1);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600E, out have_10_2);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA1600F, out have_10_3);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16010, out have_10_4);
                Celeste_MultiworldModule.SaveData.KeyItems.TryGetValue(0xCA16011, out have_10_5);

                OuiJournalPage.Row row10 = self.table.AddRow()
                    .Add(new OuiJournalPage.TextCell($"Farewell", self.TextJustify, 0.5f, self.TextColor, 0f, false))
                    .Add(new OuiJournalPage.IconsCell(have_10_1 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_2 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_3 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_4 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.IconsCell(have_10_5 ? "key" : "key_outline"))
                    .Add(new OuiJournalPage.EmptyCell(64f));
            }
        }

        private static void modOuiJournalPoem_ctor(On.Celeste.OuiJournalPoem.orig_ctor orig, OuiJournalPoem self, OuiJournal journal)
        {
            MonoMod.Utils.DynamicData dynamicUI = MonoMod.Utils.DynamicData.For(self);
            dynamicUI.Set("TextJustify", new Vector2(0.5f, 0.5f));
            dynamicUI.Set("TextColor", Color.Black * 0.6f);
            self.Journal = journal;

            self.lines = new List<OuiJournalPoem.PoemLine>();
            self.swapRoutine = new Monocle.Coroutine(true);
            self.wiggler = Monocle.Wiggler.Create(0.4f, 4f, null, false, false);

            self.PageTexture = "page";
            self.swapRoutine.RemoveOnComplete = false;
            float num = 0f;
            foreach (string phrase in Celeste_MultiworldModule.SaveData.Poem)
            {
                int phraseIndex = Poems[ArchipelagoManager.Instance.ChosenPoem].IndexOf(phrase);
                self.lines.Add(new OuiJournalPoem.PoemLine
                {
                    Text = phrase,
                    Index = num,
                    Remix = (phraseIndex % 2) == 1
                });
                num += 1f;
            }
        }

        private static System.Collections.IEnumerator modOuiJournalPoem_Swap(On.Celeste.OuiJournalPoem.orig_Swap orig, OuiJournalPoem self, int a, int b)
        {
            string text = Celeste_MultiworldModule.SaveData.Poem[a];
            Celeste_MultiworldModule.SaveData.Poem[a] = Celeste_MultiworldModule.SaveData.Poem[b];
            Celeste_MultiworldModule.SaveData.Poem[b] = text;
            OuiJournalPoem.PoemLine poemA = self.lines[a];
            OuiJournalPoem.PoemLine poemB = self.lines[b];
            OuiJournalPoem.PoemLine poemLine = self.lines[a];
            self.lines[a] = self.lines[b];
            self.lines[b] = poemLine;
            self.tween = Monocle.Tween.Create(Monocle.Tween.TweenMode.Oneshot, Monocle.Ease.CubeInOut, 0.125f, true);
            self.tween.OnUpdate = delegate (Monocle.Tween t)
            {
                poemA.Index = MathHelper.Lerp((float)a, (float)b, t.Eased);
                poemB.Index = MathHelper.Lerp((float)b, (float)a, t.Eased);
            };
            self.tween.OnComplete = delegate (Monocle.Tween t)
            {
                self.tween = null;
            };

            if (ArchipelagoManager.Instance.GoalLevel == "poetry")
            {
                bool allCorrect = true;
                for (int i = 0; i < 16; i++)
                {
                    string activeText = Celeste_MultiworldModule.SaveData.Poem[i];
                    int activeTextIndex = Poems[ArchipelagoManager.Instance.ChosenPoem].IndexOf(activeText);
                    if (activeTextIndex != PoemOrders[ArchipelagoManager.Instance.ChosenPoem][i])
                    {
                        allCorrect = false;
                        break;
                    }
                }

                if (allCorrect)
                {
                    Celeste_MultiworldModule.SaveData.MiscLocations.Add("Poetry Slam");
                }
            }

            yield return self.tween.Wait();
            self.swapping = false;
            yield break;
        }

        private static void modOuiJournalGlobal_ctor(On.Celeste.OuiJournalGlobal.orig_ctor orig, OuiJournalGlobal self, OuiJournal journal)
        {
            self.PageTexture = "page";
            self.table = new OuiJournalPage.Table().AddColumn(new OuiJournalPage.TextCell("BERRIES", new Vector2(0f, 0.5f), 1f, Color.Black * 0.7f, 0f, false));

            Monocle.Draw.SpriteBatch.Begin();
            MTN.Journal["strawberry"].DrawCentered(new Vector2(100, 150));
            MTN.Journal["strawberry"].DrawCentered(new Vector2(1500, 750));
            MTN.Journal["raspberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1600), Monocle.Calc.Random.Next(150, 750)));
            Monocle.Draw.SpriteBatch.End();
        }

        private static void modOuiJournalGlobal_Redraw(On.Celeste.OuiJournalGlobal.orig_Redraw orig, OuiJournalGlobal self, Monocle.VirtualRenderTarget buffer)
        {
            orig(self, buffer);

            Monocle.Draw.SpriteBatch.Begin();
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Strawberries; i++)
            {
                MTN.Journal["strawberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Raspberries; i++)
            {
                MTN.Journal["raspberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.BlueRaspberries; i++)
            {
                MTN.Journal["blue_raspberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Blueberries; i++)
            {
                MTN.Journal["blueberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Blackberries; i++)
            {
                MTN.Journal["blackberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Boysenberries; i++)
            {
                MTN.Journal["boysenberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Bananas; i++)
            {
                MTN.Journal["banana"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.Cranberries; i++)
            {
                MTN.Journal["cranberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            for (int i = 0; i < Celeste_MultiworldModule.SaveData.GoldenRaspberries; i++)
            {
                MTN.Journal["gold_raspberry"].DrawCentered(new Vector2(Monocle.Calc.Random.Next(100, 1500), Monocle.Calc.Random.Next(150, 800)));
            }
            Monocle.Draw.SpriteBatch.End();
        }
    }
}
