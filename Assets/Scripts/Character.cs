using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	//inspector
	public GameObject head;
	public GameObject feet;

	private CharacterController characterController;
	private float additionalHeight = 0.2f;

	private float height;
	public float Height
	{
		get => height;
		set { height = value + additionalHeight; characterController.height = value + additionalHeight; }
	}

	public Vector3 Position
	{
		get => transform.position;
		set
		{
			value.y += 5;
			transform.position = value;
			characterController.center = value;
		}
	}

	public Vector3 LateralCenter
	{
		get { return transform.position; }
		set { transform.position = new Vector3(value.x, transform.position.y, value.z); }
	}

	public float Radius { get => characterController.radius; set => characterController.radius = value; }

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		head.transform.position = new Vector3(Position.x, Position.y + Height * 0.5f, Position.z);
		feet.transform.position = new Vector3(Position.x, Position.y - Height * 0.5f, Position.z);
	}

	public void Move(Vector3 v)
	{
		characterController.Move(v);
	}
}
