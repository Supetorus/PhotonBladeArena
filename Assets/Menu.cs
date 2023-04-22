using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	private void Update()
	{
		transform.position = new Vector3(0, Game.Instance.Player.transform.position.y, Game.Instance.Player.transform.position.z + 0.5f);
	}
}
