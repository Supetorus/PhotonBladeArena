using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
	// inspector
	public float maxAccelerationForce = 1.5f;
	public float damage = 5;
	public AnimationCurve speedCurve;
	public Material attackMaterial;
	public AudioClip attackSound;
	public AudioClip playerDestroySound;
	public AudioClip nonPlayerDestroySound;
	public AudioClip traceSound;
	public PathNode targetNode;

	// components
	private Rigidbody rb;
	private AudioSource audioSource;

	// members
	private enum state
	{
		tracing,
		seekingPlayer,
	}
	private int chanceToQuitTrace = 2; // 1 / value
	private int traceCount = 0; // Increments each trace between points.
	private Player player;
	private state navState = state.tracing;
	private Vector3 traceStartPosition;
	private Vector3 traceEndPosition;
	private float elapsedTime = 0;
	private readonly float duration = 2;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();

		player = Game.Instance.Player;

		traceStartPosition = transform.position;
		traceEndPosition = targetNode.transform.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Sword") || collision.gameObject.CompareTag("Enemy")) return;
		else DieFromAllCauses();
	}

	private bool initialized = false;
	public void Initialize(PathNode startNode)
	{
		if (initialized) return;
		initialized = true;
		targetNode = startNode;
	}

	private void FixedUpdate()
	{
		transform.LookAt(player.transform);
		switch (navState)
		{
			case state.tracing:
				Trace();
				break;
			case state.seekingPlayer:
				Vector3 direction = player.transform.position - transform.position;
				float acceleration = (maxAccelerationForce * 0.65f) + (maxAccelerationForce * 0.35f * Game.secondsToMaxDifficultyReciprocal);
				rb.AddForce(acceleration * Time.deltaTime * direction);
				break;
			default:
				Console.Instance.Log("Broken enemy state");
				break;
		}
	}

	private void Trace()
	{
		if ((transform.position - traceEndPosition).magnitude > 0.01f)
		{
			//print("state is tracing");
			elapsedTime += Time.fixedDeltaTime;
			float percent = Mathf.Clamp(elapsedTime / duration, 0, 1);
			transform.position = Vector3.Lerp(traceStartPosition, traceEndPosition, percent);
		}
		else
		{
			traceCount++;
			int chance;
			if (traceCount > 1) chance = Random.Range(0, chanceToQuitTrace);
			else chance = 1;
			if (chance != 0)
			{
				audioSource.clip = traceSound;
				audioSource.Play();
				// if didn't switch then reset for next lerp
				traceStartPosition = targetNode.transform.position;
				targetNode = targetNode.next[Random.Range(0, targetNode.next.Length)];
				traceEndPosition = targetNode.transform.position;
				elapsedTime = 0;
			}
			else
			{
				audioSource.clip = attackSound;
				audioSource.Play();
				navState = state.seekingPlayer;
				GetComponent<Collider>().enabled = true;
				GetComponent<Renderer>().material = attackMaterial;
				Vector3 direction = player.transform.position - transform.position;
				rb.AddForce(.5f * direction, ForceMode.VelocityChange);
			}
		}
	}

	public void DieFromAllCauses()
	{
		AudioSource.PlayClipAtPoint(nonPlayerDestroySound, transform.position);
		//Game.Instance.PlayerLose();
		Game.Instance.Points -= 2;
		Destroy(gameObject);
	}

	public void DieFromPlayer()
	{
		AudioSource.PlayClipAtPoint(playerDestroySound, transform.position);
		Game.Instance.Points++;
		GetComponent<Collider>().enabled = false;
		Destroy(gameObject, 0.5f);
	}
}
