using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] private CollectableObjectSO _collectableObjectSO;

    public CollectableObjectSO GetCollectableObjectSO()
    {
        return _collectableObjectSO;
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public static CollectableObject SpawnCollectableObject(CollectableObjectSO collectableObjectSO, Transform parent)
    {
        Debug.Log("Spawning object with name: " + collectableObjectSO.objectName);

        Transform collectableObjectTransform = Instantiate(collectableObjectSO.prefab, parent.position + new Vector3(0.5f,0.5f,0.5f), parent.rotation);
        CollectableObject collectableObject = collectableObjectTransform.GetComponent<CollectableObject>();

        Vector3 position = new Vector3(Random.Range(0f, 3f), Random.Range(5f, 10f), Random.Range(0f, 3f));
        collectableObject.GetComponent<Rigidbody>().AddForce(position, ForceMode.Impulse);

        return collectableObject;
    }


}
