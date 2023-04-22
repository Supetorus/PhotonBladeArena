using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Dummy : MonoBehaviour
{
	public Material angryMaterial;
	public AudioClip hitSound;
	public UnityEvent startEvent;

	private Vector3 localStartPosition;
	private Material startMaterial;

	private Rigidbody rb;
	private new Renderer renderer;

	private void OnCollisionEnter(Collision collision)
	{
		//print($"Collided with tagged object: {collision.gameObject.tag}");
		if (!collision.gameObject.CompareTag("Sword")) return;
		rb.AddForce(collision.contacts[0].normal.normalized * 100);
		StartCoroutine(StartGame());
	}

	private void Start()
	{
		renderer = GetComponent<Renderer>();
		rb = GetComponent<Rigidbody>();
		localStartPosition = new Vector3(transform.localPosition.x * transform.lossyScale.x, transform.localPosition.y * transform.lossyScale.y, transform.localPosition.z * transform.lossyScale.z);
		//print($"localstartposition: {localStartPosition}");
		startMaterial = renderer.material;
	}

	private void Update()
	{
		transform.LookAt(Game.Instance.Player.transform.position);
		if (Game.Instance.IsPaused)
		{
			Physics.SyncTransforms();
			float radius = transform.localScale.x * 0.5f;
			float multiplier = 1f;
			//print($"radius: {radius}");
			//print($"radius multiplied: {radius * multiplier}");
			//print($"position: {transform.position}");
			bool collided = Physics.CheckSphere(transform.position, radius * multiplier);
			Game.Instance.IsPaused = !collided;
			//Console.Instance.Log($"collided: {collided}");
			/* o = origin
			 *  o-->
			 *  |   ^
			 *  o-->|
			 *  v   |
			 *   <--o
			 */
			//Vector3 topLeft = new Vector3(transform.position.x - radius, transform.position.y + radius, transform.position.z - radius);
			//Vector3 bottomRight = new Vector3(transform.position.x + radius, transform.position.y - radius, transform.position.z - radius);
			//if (Physics.Raycast(topLeft , Vector3.right, transform.localScale.x)
			//	|| Physics.Raycast(topLeft, Vector3.down, transform.localScale.x)
			//	|| Physics.Raycast(bottomRight, Vector3.left, transform.localScale.x)
			//	|| Physics.Raycast(bottomRight, Vector3.up, transform.localScale.x)
			//	|| Physics.Raycast(topLeft, bottomRight - topLeft, 1.4142f/*sqrt of 2*/))
			//{
			//	Game.Instance.IsPaused = false;
			//}
			//else Game.Instance.IsPaused = true;
		}
	}

	private IEnumerator StartGame()
	{
		AudioSource.PlayClipAtPoint(hitSound, transform.position);
		renderer.material = angryMaterial;
		yield return new WaitForSecondsRealtime(0.5f);
		renderer.material = startMaterial;
		transform.localPosition = Vector3.zero;

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		startEvent.Invoke();
	}

	private void OnDrawGizmos()
	{
		float diameter = transform.localScale.x;
		float radius = diameter * 0.5f;
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radius * 2);
		////top left corner
		//Vector3 start = new Vector3(transform.position.x - radius, transform.position.y + radius, transform.position.z - radius);
		//Vector3 end = new Vector3(start.x + diameter, start.y, start.z);
		//Gizmos.DrawLine(start, end);
		//end = new Vector3(start.x, start.y - diameter, start.z);
		//Gizmos.DrawLine(start, end);

		////diagonal
		//end = new Vector3(transform.position.x + radius, transform.position.y - radius, transform.position.z - radius);
		//Gizmos.DrawLine(start, end);

		////bottom right corner
		//start = end;
		//end = new Vector3(start.x - diameter, start.y, start.z);
		//Gizmos.DrawLine(start, end);
		//end = new Vector3(start.x, start.y + diameter, start.z);
		//Gizmos.DrawLine(start, end);
	}
}
