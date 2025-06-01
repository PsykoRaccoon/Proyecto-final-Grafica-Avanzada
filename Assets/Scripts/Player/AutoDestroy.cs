using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeSpan;

    void Start()
    {
        Destroy(gameObject, lifeSpan); ;
    }
}
