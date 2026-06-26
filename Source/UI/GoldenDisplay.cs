using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Celeste_Multiworld.UI
{
    public class GoldenCounter : StrawberriesCounter
    {
        public GoldenCounter(bool centeredX, int amount, int outOf = 0, bool showOutOf = false) : base(centeredX, amount, outOf, showOutOf)
        {

        }

        new public float FullHeight
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return Math.Max(ActiveFont.LineHeight, (float)GFX.Gui["collectables/goldberry"].Height);
            }
        }

        public override void Render()
        {
            Vector2 vector = this.RenderPosition;
            Vector2 vector2 = Monocle.Calc.AngleToVector(this.Rotation, 1f);
            Vector2 vector3 = new Vector2(-vector2.Y, vector2.X);
            string text = (this.showOutOf ? this.sOutOf : "");
            float num = ActiveFont.Measure(this.sAmount).X;
            float num2 = ActiveFont.Measure(text).X;
            float num3 = 62f + (float)this.x.Width + 2f + num + num2;
            Color color = this.Color;
            if (this.flashTimer > 0f && base.Scene != null && base.Scene.BetweenRawInterval(0.05f))
            {
                color = StrawberriesCounter.FlashColor;
            }
            if (this.CenteredX)
            {
                vector -= vector2 * (num3 / 2f) * this.Scale;
            }
            string text2 = "collectables/goldberry";
            GFX.Gui[text2].DrawCentered(vector + vector2 * 60f * 0.5f * this.Scale, Color.White, this.Scale);
            this.x.DrawCentered(vector + vector2 * (62f + (float)this.x.Width * 0.5f) * this.Scale + vector3 * 2f * this.Scale, color, this.Scale);
            ActiveFont.DrawOutline(this.sAmount, vector + vector2 * (num3 - num2 - num * 0.5f) * this.Scale + vector3 * (this.wiggler.Value * 18f) * this.Scale, new Vector2(0.5f, 0.5f), Vector2.One * this.Scale, color, this.Stroke, Color.Black);
            if (text != "")
            {
                ActiveFont.DrawOutline(text, vector + vector2 * (num3 - num2 / 2f) * this.Scale, new Vector2(0.5f, 0.5f), Vector2.One * this.Scale, this.OutOfColor, this.Stroke, Color.Black);
            }
        }
    }


    public class GoldenDisplay : Monocle.Entity
    {
        public GoldenDisplay()
        {
            base.Y = 296f;
            base.Depth = -101;
            base.Tag = Tags.HUD | Tags.Global | Tags.PauseUpdate | Tags.TransitionUpdate;
            this.bg = GFX.Gui["strawberryCountBG"];
            base.Add(this.deaths = new GoldenCounter(false, ArchipelagoManager.Instance.GoldenDeathsCounted, ArchipelagoManager.Instance.GoldenAmnesty, true));
        }

        public override void Update()
        {
            base.Update();

            if (!Celeste_MultiworldModule.Session.GoldenStrawberryFollowing.HasValue)
            {
                this.deaths.Amount = 0;
                ArchipelagoManager.Instance.GoldenDeathsCounted = 0;
                return;
            }

            Level level = base.Scene as Level;
            if (ArchipelagoManager.Instance.GoldenDeathsCounted > this.deaths.Amount && this.deathsUpdateTimer <= 0f)
            {
                this.deathsUpdateTimer = 0.4f;
            }
            else if (ArchipelagoManager.Instance.GoldenDeathsCounted == 0 && this.deaths.amount > 0 && this.deathsUpdateTimer <= 0f)
            {
                this.deathsUpdateTimer = 0.4f;
            }
            if (ArchipelagoManager.Instance.GoldenDeathsCounted > this.deaths.Amount || this.deathsUpdateTimer > 0f || this.deathsWaitTimer > 0f ||
                (level.Paused && level.PauseMainMenuOpen && Celeste_MultiworldModule.Session.GoldenStrawberryFollowing.HasValue && ArchipelagoManager.Instance.GoldenAmnesty > 1))
            {
                this.DrawLerp = Monocle.Calc.Approach(this.DrawLerp, 1f, 1.2f * Monocle.Engine.RawDeltaTime);
            }
            else
            {
                this.DrawLerp = Monocle.Calc.Approach(this.DrawLerp, 0f, 2f * Monocle.Engine.RawDeltaTime);
            }
            if (this.deathsWaitTimer > 0f)
            {
                this.deathsWaitTimer -= Monocle.Engine.RawDeltaTime;
            }
            if (this.deathsUpdateTimer > 0f && this.DrawLerp == 1f)
            {
                this.deathsUpdateTimer -= Monocle.Engine.RawDeltaTime;
                if (this.deathsUpdateTimer <= 0f)
                {
                    if (this.deaths.Amount < ArchipelagoManager.Instance.GoldenDeathsCounted)
                    {
                        this.deaths.Amount += 1;
                    }
                    else if (ArchipelagoManager.Instance.GoldenDeathsCounted == 0 && this.deaths.amount > 0)
                    {
                        this.deaths.Amount = 0;
                    }
                    this.deathsWaitTimer = 2f;
                    if (this.deaths.Amount < ArchipelagoManager.Instance.GoldenDeathsCounted && ArchipelagoManager.Instance.GoldenDeathsCounted != 0)
                    {
                        this.deathsUpdateTimer = 0.3f;
                    }
                }
            }
            if (this.Visible)
            {
                float num = 196f;
                if (!level.TimerHidden)
                {
                    if (Settings.Instance.SpeedrunClock == SpeedrunType.Chapter)
                    {
                        num += 58f;
                    }
                    else if (Settings.Instance.SpeedrunClock == SpeedrunType.File)
                    {
                        num += 78f;
                    }
                }
                if (ArchipelagoManager.Instance.DeathLinkActive)
                {
                    num += 100f;
                }
                base.Y = Monocle.Calc.Approach(base.Y, num, Monocle.Engine.DeltaTime * 800f);
            }
            this.Visible = this.DrawLerp > 0f;
        }

        public override void Render()
        {
            Vector2 vector = Vector2.Lerp(new Vector2((float)(-(float)this.bg.Width), base.Y), new Vector2(32f, base.Y), Monocle.Ease.CubeOut(this.DrawLerp));
            vector = Monocle.Calc.Round(vector);
            this.bg.DrawJustified(vector + new Vector2(-96f, 12f), new Vector2(0f, 0.5f));
            this.deaths.Position = vector + new Vector2(0f, -base.Y);
            this.deaths.Render();
        }

        static GoldenDisplay()
        {
            GoldenDisplay.FlashColor = Monocle.Calc.HexToColor("FF5E76");
        }

        // Token: 0x04002174 RID: 8564
        private const float NumberUpdateDelay = 0.4f;

        // Token: 0x04002175 RID: 8565
        private const float ComboUpdateDelay = 0.3f;

        // Token: 0x04002176 RID: 8566
        private const float AfterUpdateDelay = 2f;

        // Token: 0x04002177 RID: 8567
        private const float LerpInSpeed = 1.2f;

        // Token: 0x04002178 RID: 8568
        private const float LerpOutSpeed = 2f;

        // Token: 0x04002179 RID: 8569
        public static Microsoft.Xna.Framework.Color FlashColor;

        // Token: 0x0400217A RID: 8570
        private Monocle.MTexture bg;

        // Token: 0x0400217B RID: 8571
        public float DrawLerp;

        // Token: 0x0400217C RID: 8572
        private float deathsUpdateTimer;

        // Token: 0x0400217D RID: 8573
        private float deathsWaitTimer;

        // Token: 0x0400217E RID: 8574
        private GoldenCounter deaths;
    }
}
