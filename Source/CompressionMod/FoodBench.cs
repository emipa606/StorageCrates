using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace KompressionMod
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal class FoodBench : Building_WorkTable
    {
        // Token: 0x0400000A RID: 10
        private static Texture2D Ui_Pmode1;

        // Token: 0x0400000B RID: 11
        private static Texture2D Ui_Pmode2;

        // Token: 0x0400000D RID: 13
        private static Graphic TexOpen;

        // Token: 0x0400000E RID: 14
        private static Graphic TexClosed;

        // Token: 0x0400000F RID: 15
        private static Graphic[] TexResFrames;

        // Token: 0x04000006 RID: 6
        private bool Change;

        // Token: 0x04000011 RID: 17
        private Faction factionthing;

        // Token: 0x04000010 RID: 16
        private Graphic OutputGraphic;

        // Token: 0x04000009 RID: 9
        private CompPowerTrader powerComp;

        // Token: 0x04000008 RID: 8
        public bool stateOpen = true;

        // Token: 0x0400000C RID: 12
        private Graphic TexMain;

        // Token: 0x04000007 RID: 7
        private int timer;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
        public override Graphic Graphic => OutputGraphic ?? base.Graphic;

        // Token: 0x06000007 RID: 7 RVA: 0x00002118 File Offset: 0x00000318
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            powerComp = GetComp<CompPowerTrader>();
            LongEventHandler.ExecuteWhenFinished(SS2);
            factionthing = factionInt;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002150 File Offset: 0x00000350
        public void SS2()
        {
            Ui_Pmode1 = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_Pack");
            Ui_Pmode2 = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_unPack");
            TexOpen = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/FoodBench_Packing");
            TexClosed = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/FoodBench_UnPacking");
            var texResFrames = new Graphic[12];
            TexResFrames = texResFrames;
            for (var i = 0; i < 12; i++)
            {
                TexResFrames[i] =
                    GraphicDatabase.Get<Graphic_Single>("Things/Building/Frames/FoodBench_Frame" + (i + 1));
                TexResFrames[i].drawSize = Graphic.drawSize;
                TexResFrames[i].color = Graphic.color;
                TexResFrames[i].colorTwo = Graphic.colorTwo;
                TexResFrames[i].MatSingle.color = Graphic.MatSingle.color;
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002248 File Offset: 0x00000448
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Change, "ChangeActivator");
            Scribe_Values.Look(ref timer, "Timer");
            Scribe_Values.Look(ref stateOpen, "BenchState");
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002298 File Offset: 0x00000498
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

        // Token: 0x0600000B RID: 11 RVA: 0x00002373 File Offset: 0x00000573
        private void Open()
        {
            Change = !Change;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002388 File Offset: 0x00000588
        public override void Tick()
        {
            base.Tick();
            if (!Change || !powerComp.PowerOn)
            {
                return;
            }

            HandleAnimation(!stateOpen);

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
                thing = ThingMaker.MakeThing(ThingDef.Named("FoodUnPackingBench"), Stuff);
            }
            else
            {
                TexMain = TexOpen;
                thing = ThingMaker.MakeThing(ThingDef.Named("FoodPackingBench"), Stuff);
            }

            thing.HitPoints = HitPoints;
            ((FoodBench) thing).stateOpen = !stateOpen;
            thing.SetFactionDirect(Faction);
            GenSpawn.Spawn(thing, Position, Map, Rotation);
            timer = 0;
            Change = false;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000024C4 File Offset: 0x000006C4
        private void HandleAnimation(bool open)
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

        // Token: 0x0600000E RID: 14 RVA: 0x00002510 File Offset: 0x00000710
        private void UpdateOutputGraphic()
        {
            OutputGraphic = TexMain.GetColoredVersion(def.graphicData.Graphic.Shader, Stuff.stuffProps.color,
                Stuff.stuffProps.color);
        }
    }
}