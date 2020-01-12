using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy = null;

    public void DestroyTarget()
    {
        Destroy(targetToDestroy);
    }
}
