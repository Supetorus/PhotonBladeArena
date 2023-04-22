using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
	public PathNode[] next;

	public LineRenderer lineRenderer;

	private void Start()
	{
		foreach (var node in next)
		{
			//var line = Instantiate(lineRenderer);
			var line = Instantiate(lineRenderer, transform);
			line.SetPosition(0, transform.position);
			line.SetPosition(1, node.transform.position);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.5078125f, 1f, 0.63671875f, 0.5f);
		Gizmos.DrawSphere(transform.position, 0.1f);
		foreach (var node in next)
		{
			//print("node is " + node.name);
			Gizmos.DrawLine(transform.position, node.transform.position);
		}
	}
}
