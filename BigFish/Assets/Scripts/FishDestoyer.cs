
using UnityEngine;

public class FishDestoyer : MonoBehaviour
{
    public void DestroyParentPrefab()
    {
        GameObject parentObject = transform.parent.gameObject;

        if (parentObject != null)
        {
            Destroy(parentObject);
        }
    }
}
