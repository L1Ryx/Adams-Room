using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected GameObject bonfireObj;
    protected BonfireManager bm;
    protected Vector2 gridPosition; // store the grid position for this item

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        bonfireObj = GameObject.FindWithTag("TheBonfireObj");
        bm = bonfireObj.GetComponent<BonfireManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CanPickUp())
            {
                OnPickup();
                Destroy(gameObject);
            }
        }
    }

    public virtual bool CanPickUp()
    {
        return true; // By default, all items can be picked up
    }

    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2(x, y);
    }

    // Derived classes must implement this method
    public abstract void OnPickup();

    private void OnDestroy()
    {
        // This will only be called if the object is actually being destroyed, not when exiting Play Mode in the Editor.
        if (Application.isPlaying)
        {
            ItemSpawner.Instance.MarkPositionAsFree((int)gridPosition.x, (int)gridPosition.y);
        }
    }
}
