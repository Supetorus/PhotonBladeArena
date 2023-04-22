using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public string matchTag;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag(matchTag)) Destroy(collision.gameObject);
	}
}
