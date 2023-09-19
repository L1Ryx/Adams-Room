using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Item
{
    public override void OnPickup()
    {
        if (ItemManager.Instance.TryIncrementLogCount())
        {
            //bm.bonfireValue += 5f;
            DestroyAndMarkPositionFree();
        }
    }

    public override bool CanPickUp()
    {
        return ItemManager.Instance.GetLogCount() < ItemManager.Instance.maxLogs;
    }

    // Added this new method
    private void DestroyAndMarkPositionFree()
    {
        ItemSpawner.Instance.MarkPositionAsFree((int)gridPosition.x, (int)gridPosition.y);
        Destroy(gameObject);
    }
}
