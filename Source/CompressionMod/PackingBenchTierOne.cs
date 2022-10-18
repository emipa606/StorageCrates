using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace KompressionMod;

[StaticConstructorOnStartup]
internal class PackingBenchTierOne : Building_WorkTable
{
    private static Texture2D Ui_Pmode1;

    private static Texture2D Ui_Pmode2;

    private static Graphic TexOpen;

    private static Graphic TexClosed;

    private static Graphic[] TexResFrames;

    private bool Change;

    private Faction factionthing;

    private Graphic OutputGraphic;

    private CompPowerTrader powerComp;

    public bool stateOpen = true;

    private Graphic TexMain;

    private int timer;

    public override Graphic Graphic => OutputGraphic ?? base.Graphic;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        powerComp = GetComp<CompPowerTrader>();
        LongEventHandler.ExecuteWhenFinished(SS2);
        factionthing = factionInt;
    }

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

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Change, "ChangeActivator");
        Scribe_Values.Look(ref timer, "Timer");
        Scribe_Values.Look(ref stateOpen, "BenchState");
    }

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

    private void Open()
    {
        Change = !Change;
    }

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
        ((PackingBenchTierOne)thing).stateOpen = !stateOpen;
        thing.SetFactionDirect(Faction);
        GenSpawn.Spawn(thing, Position, Map, Rotation);
        timer = 0;
        Change = false;
    }

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

    private void UpdateOutputGraphic()
    {
        OutputGraphic = TexMain.GetColoredVersion(def.graphicData.Graphic.Shader, Stuff.stuffProps.color,
            Stuff.stuffProps.color);
    }
}