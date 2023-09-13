using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] private GameObject bonfireObj;
    private BonfireManager bm;
    // Start is called before the first frame update
    void Start()
    {
        bm = bonfireObj.GetComponent<BonfireManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bm.bonfireValue += 5f;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
