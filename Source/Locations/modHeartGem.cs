using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.Locations
{
    internal class modHeartGem : modLocationBase
    {
        private static Dictionary<string, int> poemIDtoIndex = new Dictionary<string, int>()
        {
            { "fc", 0 },
            { "fcr", 1 },
            { "os", 2 },
            { "osr", 3 },
            { "cr", 4 },
            { "crr", 5 },
            { "cs", 6 },
            { "csr", 7 },
            { "t", 8 },
            { "tr", 9 },
            { "tf", 10 },
            { "tfr", 11 },
            { "ts", 12 },
            { "tsr", 13 },
            { "mc", 14 },
            { "mcr", 15 },
        };

        public override void Load()
        {
            On.Celeste.HeartGem.Collect += modHeartGem_Collect;
            On.Celeste.HeartGem.CollectRoutine += modHeartGem_CollectRoutine;
            On.Celeste.HeartGem.SkipFakeHeartCutscene += modHeartGem_SkipFakeHeartCutscene;
            On.Celeste.Level.RegisterAreaComplete += modLevel_RegisterAreaComplete;
        }

        public override void Unload()
        {
            On.Celeste.HeartGem.Collect -= modHeartGem_Collect;
            On.Celeste.HeartGem.CollectRoutine -= modHeartGem_CollectRoutine;
            On.Celeste.HeartGem.SkipFakeHeartCutscene -= modHeartGem_SkipFakeHeartCutscene;
            On.Celeste.Level.RegisterAreaComplete -= modLevel_RegisterAreaComplete;
        }

        private static void modHeartGem_Collect(On.Celeste.HeartGem.orig_Collect orig, HeartGem self, Player player)
        {
            orig(self, player);

            if (SaveData.Instance.CurrentSession_Safe.Area.Mode == AreaMode.Normal)
            {
                string crystalHeartString = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_CrystalHeart";

                Celeste_MultiworldModule.SaveData.CrystalHeartLocations.Add(crystalHeartString);
            }
        }

        private static System.Collections.IEnumerator modHeartGem_CollectRoutine(On.Celeste.HeartGem.orig_CollectRoutine orig, HeartGem self, Player player)
        {
            MonoMod.Utils.DynamicData dynamicHeartGem = MonoMod.Utils.DynamicData.For(self);
            Level level = self.Scene as Level;
            bool flag = false;
            Meta.MapMetaModeProperties mapMetaModeProperties = ((level != null) ? ((MapData)level.Session.MapData).Meta : null);
            if (mapMetaModeProperties != null && mapMetaModeProperties.HeartIsEnd != null)
            {
                flag = mapMetaModeProperties.HeartIsEnd.Value;
            }
            flag &= !self.IsFake;
            if (flag)
            {
                List<IStrawberry> list = new List<IStrawberry>();
                System.Collections.ObjectModel.ReadOnlyCollection<Type> berryTypes = StrawberryRegistry.GetBerryTypes();
                foreach (Follower follower in player.Leader.Followers)
                {
                    if (berryTypes.Contains(follower.Entity.GetType()) && follower.Entity is IStrawberry)
                    {
                        list.Add(follower.Entity as IStrawberry);
                    }
                }
                foreach (IStrawberry strawberry in list)
                {
                    strawberry.OnCollect();
                }
            }

            AreaKey area = level.Session.Area;
            string poemID = AreaData.Get(level).Mode[(int)area.Mode].PoemID;
            //bool completeArea = self.IsCompleteArea(!self.IsFake && (area.Mode != AreaMode.Normal || area.ID == 9));
            bool completeArea = dynamicHeartGem.Invoke<bool>("IsCompleteArea", !self.IsFake && (area.Mode != AreaMode.Normal || area.ID == 9));
            if (self.IsFake)
            {
                level.StartCutscene(new Action<Level>(self.SkipFakeHeartCutscene), true, false, true);
            }
            else
            {
                level.CanRetry = false;
            }
            if (completeArea || self.IsFake)
            {
                Audio.SetMusic(null, true, true);
                Audio.SetAmbience(null, true);
            }
            if (completeArea)
            {
                List<Strawberry> list = new List<Strawberry>();
                foreach (Follower follower in player.Leader.Followers)
                {
                    if (follower.Entity is Strawberry)
                    {
                        list.Add(follower.Entity as Strawberry);
                    }
                }
                foreach (Strawberry strawberry in list)
                {
                    strawberry.OnCollect();
                }
            }
            string text = "event:/game/general/crystalheart_blue_get";
            if (self.IsFake)
            {
                text = "event:/new_content/game/10_farewell/fakeheart_get";
            }
            else if (area.Mode == AreaMode.BSide)
            {
                text = "event:/game/general/crystalheart_red_get";
            }
            else if (area.Mode == AreaMode.CSide)
            {
                text = "event:/game/general/crystalheart_gold_get";
            }
            self.sfx = SoundEmitter.Play(text, self, null);
            self.Add(new LevelEndingHook(delegate
            {
                self.sfx.Source.Stop(true);
            }));
            self.walls.Add(new InvisibleBarrier(new Vector2((float)level.Bounds.Right, (float)level.Bounds.Top), 8f, (float)level.Bounds.Height));
            self.walls.Add(new InvisibleBarrier(new Vector2((float)(level.Bounds.Left - 8), (float)level.Bounds.Top), 8f, (float)level.Bounds.Height));
            self.walls.Add(new InvisibleBarrier(new Vector2((float)level.Bounds.Left, (float)(level.Bounds.Top - 8)), (float)level.Bounds.Width, 8f));
            foreach (InvisibleBarrier invisibleBarrier in self.walls)
            {
                self.Scene.Add(invisibleBarrier);
            }
            self.Add(self.white = GFX.SpriteBank.Create("heartGemWhite"));
            self.Depth = -2000000;
            yield return null;
            Celeste.Freeze(0.2f);
            yield return null;
            Monocle.Engine.TimeRate = 0.5f;
            player.Depth = -2000000;
            for (int i = 0; i < 10; i++)
            {
                self.Scene.Add(new AbsorbOrb(self.Position, null, null));
            }
            level.Shake(0.3f);
            Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
            level.Flash(Color.White, false);
            level.FormationBackdrop.Display = true;
            level.FormationBackdrop.Alpha = 1f;
            self.light.Alpha = (self.bloom.Alpha = 0f);
            self.Visible = false;
            for (float t = 0f; t < 2f; t += Monocle.Engine.RawDeltaTime)
            {
                Monocle.Engine.TimeRate = Monocle.Calc.Approach(Monocle.Engine.TimeRate, 0f, Monocle.Engine.RawDeltaTime * 0.25f);
                yield return null;
            }
            yield return null;
            if (player.Dead)
            {
                yield return 100f;
            }
            Monocle.Engine.TimeRate = 1f;
            self.Tag = Tags.FrozenUpdate;
            level.Frozen = true;
            if (!self.IsFake)
            {
                self.RegisterAsCollected(level, poemID);
                if (completeArea)
                {
                    level.TimerStopped = true;
                    level.RegisterAreaComplete();
                }
            }
            string text2 = null;
            if (!string.IsNullOrEmpty(poemID))
            {
                if (modHeartGem.poemIDtoIndex.Keys.Contains(poemID.ToLower()))
                {
                    text2 = UI.modJournal.Poems[ArchipelagoManager.Instance.ChosenPoem][modHeartGem.poemIDtoIndex[poemID]];
                }
                else
                {
                    text2 = Dialog.Clean("poem_" + poemID, null);
                }
            }
            self.poem = new Poem(text2, (int)(self.IsFake ? ((AreaMode)3) : area.Mode), (area.Mode == AreaMode.CSide || self.IsFake) ? 1f : 0.6f);
            self.poem.Alpha = 0f;
            self.Scene.Add(self.poem);
            for (float t = 0f; t < 1f; t += Monocle.Engine.RawDeltaTime)
            {
                self.poem.Alpha = Monocle.Ease.CubeOut(t);
                yield return null;
            }
            if (self.IsFake)
            {
                yield return self.DoFakeRoutineWithBird(player);
            }
            else
            {
                while (!Input.MenuConfirm.Pressed && !Input.MenuCancel.Pressed)
                {
                    yield return null;
                }
                self.sfx.Source.Param("end", 1f);
                if (!completeArea)
                {
                    level.FormationBackdrop.Display = false;
                    for (float t = 0f; t < 1f; t += Monocle.Engine.RawDeltaTime * 2f)
                    {
                        self.poem.Alpha = Monocle.Ease.CubeIn(1f - t);
                        yield return null;
                    }
                    player.Depth = 0;
                    self.EndCutscene();
                }
                else
                {
                    yield return new FadeWipe(level, false, null)
                    {
                        Duration = 3.25f
                    }.Duration;
                    level.CompleteArea(false, true, false);
                }
            }
            yield break;
        }

        private static void modHeartGem_SkipFakeHeartCutscene(On.Celeste.HeartGem.orig_SkipFakeHeartCutscene orig, HeartGem self, Level level)
        {
            if (ArchipelagoManager.Instance.ActiveLevels.Contains("10b"))
            {
                orig(self, level);
            }
            else
            {
                Monocle.Engine.TimeRate = 1f;
                Glitch.Value = 0f;
                if (self.sfx != null)
                {
                    self.sfx.Source.Stop(true);
                }
                level.Session.SetFlag("fake_heart", true);
                level.Frozen = false;
                level.FormationBackdrop.Display = false;
                level.Session.Audio.Music.Event = "event:/new_content/music/lvl10/intermission_heartgroove";
                level.Session.Audio.Apply(false);
                Player entity = self.Scene.Tracker.GetEntity<Player>();
                if (entity != null)
                {
                    entity.Sprite.Play("idle", false, false);
                    entity.Active = true;
                    entity.StateMachine.State = 0;
                    entity.Dashes = 1;
                    entity.Speed = Vector2.Zero;
                    entity.MoveV(200f, null, null);
                    entity.Depth = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        entity.UpdateHair(true);
                    }
                }
                foreach (AbsorbOrb absorbOrb in self.Scene.Entities.FindAll<AbsorbOrb>())
                {
                    absorbOrb.RemoveSelf();
                }
                if (self.poem != null)
                {
                    self.poem.RemoveSelf();
                }
                if (self.bird != null)
                {
                    self.bird.RemoveSelf();
                }

                self.RemoveSelf();
            }
        }

        private static void modLevel_RegisterAreaComplete(On.Celeste.Level.orig_RegisterAreaComplete orig, Level self)
        {
            orig(self);

            if (SaveData.Instance.CurrentSession_Safe.Area.Mode != AreaMode.Normal)
            {
                string AP_ID = $"{SaveData.Instance.CurrentSession_Safe.Area.ID}_{(int)SaveData.Instance.CurrentSession_Safe.Area.Mode}_Clear";
                Celeste_MultiworldModule.SaveData.LevelClearLocations.Add(AP_ID);
            }
        }
    }
}
