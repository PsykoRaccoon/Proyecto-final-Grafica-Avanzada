using System.Collections;
using UnityEngine;

public class DestroyAfterTimeParticle : MonoBehaviour
{
	public float timeToDestroy;
	void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}

}
