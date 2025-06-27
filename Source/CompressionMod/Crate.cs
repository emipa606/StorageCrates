using UnityEngine;
using Verse;

namespace KompressionMod;

[StaticConstructorOnStartup]
internal class Crate : ThingWithComps
{
    private static Texture2D UnLockIco;

    private int StoredAmount;

    private string StoredThingName;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        ReadFormXML();
        LongEventHandler.ExecuteWhenFinished(ss2);
    }

    private static void ss2()
    {
        UnLockIco = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_unPack");
    }

    private void ReadFormXML()
    {
        var kompressionModThingDefs = (KompressionModThingDefs)def;
        if (kompressionModThingDefs == null)
        {
            return;
        }

        StoredThingName = kompressionModThingDefs.StoredStuff;
        StoredAmount = kompressionModThingDefs.StoredStuffAmmount;
    }
}