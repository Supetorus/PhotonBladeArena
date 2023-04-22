using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Lightsaber : MonoBehaviour
{
	public new GameObject collider;
	public GameObject saber;

	private XRController controller;

	private void Start()
	{
		controller = GetComponentInParent<XRController>();
		//Console.Instance.Log($"Found controller: {controller != null}");
	}

	private void OnCollisionEnter(Collision collision)
	{
		controller.SendHapticImpulse(0.2f, 0.1f);
		if (collision.gameObject.tag.Equals("Enemy"))
		{
			if (collision.gameObject.TryGetComponent(out Enemy e)) e.DieFromPlayer();
		}
	}

	public void Disable()
	{
		collider.SetActive(false);
		saber.SetActive(false);
	}

	public void Enable()
	{
		collider.SetActive(true);
		saber.SetActive(true);
	}
}
