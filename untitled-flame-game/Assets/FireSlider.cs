using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSlider : MonoBehaviour
{
    [SerializeField] private GameObject bonFireObj;
    private Slider sl;
    private BonfireManager bm;
    // Start is called before the first frame update
    void Start()
    {
        bm = bonFireObj.GetComponent<BonfireManager>();
        sl = GetComponent<Slider>();
        sl.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        sl.value = bm.bonfireValue / bm.maxBonfireValue;
    }
}
