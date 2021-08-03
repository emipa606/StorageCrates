using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace KompressionMod
{
    // Token: 0x02000005 RID: 5
    [StaticConstructorOnStartup]
    internal class PackingBenchTierOne : Building_WorkTable
    {
        // Token: 0x04000016 RID: 22
        private static Texture2D Ui_Pmode1;

        // Token: 0x04000017 RID: 23
        private static Texture2D Ui_Pmode2;

        // Token: 0x04000019 RID: 25
        private static Graphic TexOpen;

        // Token: 0x0400001A RID: 26
        private static Graphic TexClosed;

        // Token: 0x0400001B RID: 27
        private static Graphic[] TexResFrames;

        // Token: 0x04000012 RID: 18
        private bool Change;

        // Token: 0x0400001D RID: 29
        private Faction factionthing;

        // Token: 0x0400001C RID: 28
        private Graphic OutputGraphic;

        // Token: 0x04000015 RID: 21
        private CompPowerTrader powerComp;

        // Token: 0x04000014 RID: 20
        public bool stateOpen = true;

        // Token: 0x04000018 RID: 24
        private Graphic TexMain;

        // Token: 0x04000013 RID: 19
        private int timer;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000010 RID: 16 RVA: 0x00002584 File Offset: 0x00000784
        public override Graphic Graphic => OutputGraphic ?? base.Graphic;

        // Token: 0x06000011 RID: 17 RVA: 0x000025AC File Offset: 0x000007AC
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            powerComp = GetComp<CompPowerTrader>();
            LongEventHandler.ExecuteWhenFinished(SS2);
            factionthing = factionInt;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000025E4 File Offset: 0x000007E4
        public void SS2()
        {
            Ui_Pmode1 = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_Pack");
            Ui_Pmode2 = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_unPack");
            TexOpen = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/PackingBenchTier1_Open");
            TexClosed = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/PackingBenchTier1_Closed");
            var texResFrames = new Graphic[12];
            TexResFrames = texResFrames;
            for (var i = 0; i < 12; i++)
            {
                TexResFrames[i] =
                    GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/PackingBenchTier1_Frame" + (i + 1));
                TexResFrames[i].drawSize = Graphic.drawSize;
                TexResFrames[i].color = Graphic.color;
                TexResFrames[i].colorTwo = Graphic.colorTwo;
                TexResFrames[i].MatSingle.color = Graphic.MatSingle.color;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000026DC File Offset: 0x000008DC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Change, "ChangeActivator");
            Scribe_Values.Look(ref timer, "Timer");
            Scribe_Values.Look(ref stateOpen, "BenchState");
        }

        // Token: 0x06000014 RID: 20 RVA: 0x0000272C File Offset: 0x0000092C
        public override IEnumerable<Gizmo> GetGizmos()
        {
            IList<Gizmo> list = new List<Gizmo>();
            var command_Action = new Command_Action();
            if (!Change && powerComp.PowerOn)
            {
                if (stateOpen)
                {
                    command_Action.icon = Ui_Pmode2;
                    command_Action.defaultDesc = "Change to Un-Packing Mode";
                }
                else
                {
                    command_Action.icon = Ui_Pmode1;
                    command_Action.defaultDesc = "Change to Packing Mode";
                }

                command_Action.activateSound = SoundDef.Named("Click");
                command_Action.action = Open;
                command_Action.groupKey = 78142894;
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                list.Add(command_Action);
            }

            var gizmos = base.GetGizmos();
            return gizmos == null ? list.AsEnumerable() : list.AsEnumerable().Concat(gizmos);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002807 File Offset: 0x00000A07
        private void Open()
        {
            Change = !Change;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x0000281C File Offset: 0x00000A1C
        public override void Tick()
        {
            base.Tick();
            if (!Change || !powerComp.PowerOn)
            {
                return;
            }

            handleAnimation(!stateOpen);

            timer++;
            Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things, false, false);
            if (timer < 60)
            {
                return;
            }

            Thing thing;
            if (stateOpen)
            {
                TexMain = TexClosed;
                thing = ThingMaker.MakeThing(ThingDef.Named("UnpackingBenchTierOne"), Stuff);
            }
            else
            {
                TexMain = TexOpen;
                thing = ThingMaker.MakeThing(ThingDef.Named("PackingBenchTierOne"), Stuff);
            }

            thing.HitPoints = HitPoints;
            ((PackingBenchTierOne) thing).stateOpen = !stateOpen;
            thing.SetFactionDirect(Faction);
            GenSpawn.Spawn(thing, Position, Map, Rotation);
            timer = 0;
            Change = false;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002958 File Offset: 0x00000B58
        private void handleAnimation(bool open)
        {
            if (timer >= 60)
            {
                return;
            }

            var num = timer / 5;
            if (!open)
            {
                num = 11 - num;
            }

            TexMain = TexResFrames[num];
            UpdateOutputGraphic();
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000029A4 File Offset: 0x00000BA4
        private void UpdateOutputGraphic()
        {
            OutputGraphic = TexMain.GetColoredVersion(def.graphicData.Graphic.Shader, Stuff.stuffProps.color,
                Stuff.stuffProps.color);
        }
    }
}