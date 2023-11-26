using UnityEngine;
using System.Collections;

public class Merchant : MonoBehaviour
{
    [SerializeField] private Vector2 entryPosition;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 exitPosition;
    [SerializeField] private float speed = 2.0f;

    [SerializeField] private Animator anim;

    [SerializeField] private Vector2 itemOffset = new Vector2(1, 0);
    [SerializeField] private float itemDisappearTime = 8.0f;

    [SerializeField] private GameObject randomItemPrefab;

    [SerializeField] private bool shouldReturn = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.transform.position = startPosition;
        randomItemPrefab = MerchantManager.Instance.shopItemPrefabs[Random.Range(0, MerchantManager.Instance.shopItemPrefabs.Length)];
        if (randomItemPrefab == null)
        {
            Debug.LogError("Shop Item Prefab is not set!");
            return;
        }
        StartCoroutine(MerchantRoutine());
    }

    public void MoveToStop()
    {
        anim.SetTrigger("stop");
    }

    public void MoveToIdle()
    {
        anim.SetTrigger("shouldIdle");
    }

    public void MoveToGo()
    {
        anim.SetTrigger("go");
    }

    public void MoveToTurnLeft()
    {
        anim.SetTrigger("turnLeft");
        shouldReturn = true;
    }

    private IEnumerator MerchantRoutine()
    {
        yield return MoveToPosition(entryPosition);

        MoveToStop();

        GameObject item = Instantiate(randomItemPrefab, (Vector2)transform.position + itemOffset, Quaternion.identity);
        Destroy(item, itemDisappearTime);

        yield return new WaitForSeconds(10.0f);

        MoveToGo();

        yield return new WaitUntil(() => shouldReturn);

        yield return MoveToPosition(exitPosition);
        Destroy(gameObject);


    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
