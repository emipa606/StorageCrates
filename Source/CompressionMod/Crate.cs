using UnityEngine;
using Verse;

namespace KompressionMod
{
    // Token: 0x02000002 RID: 2
    [StaticConstructorOnStartup]
    internal class Crate : ThingWithComps
    {
        // Token: 0x04000003 RID: 3
        private static Texture2D UnLockIco;

        // Token: 0x04000001 RID: 1
        private int StoredAmount;

        // Token: 0x04000002 RID: 2
        private string StoredThingName;

        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            ReadFormXML();
            LongEventHandler.ExecuteWhenFinished(SS2);
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002075 File Offset: 0x00000275
        public void SS2()
        {
            UnLockIco = ContentFinder<Texture2D>.Get("Things/Building/Ui/Ui_unPack");
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002088 File Offset: 0x00000288
        private void ReadFormXML()
        {
            var kompressionModThingDefs = (KompressionModThingDefs) def;
            if (kompressionModThingDefs == null)
            {
                return;
            }

            StoredThingName = kompressionModThingDefs.StoredStuff;
            StoredAmount = kompressionModThingDefs.StoredStuffAmmount;
        }
    }
}