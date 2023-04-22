using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{

	public float speed = 1;
	public XRNode inputSource;
	public float gravity = -9.81f;
	public LayerMask ground;
	public Character character;

	//debug
	public TMP_Text debugText;
	public Transform debugTransform;
	private string[] debugLines = new string[10];

	private float fallingSpeed;
	private XROrigin origin;
	private Vector2 inputAxis;

	void Start()
	{
		origin = GetComponent<XROrigin>();
	}

	void Update()
	{
		InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
		device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
debugLines[0] = $"Device: {device.name}";
debugLines[1] = $"2D Input: {inputAxis}";

		debugText.text = "";
		foreach (var line in debugLines)
		{
			debugText.text += line + "\n";
		}
	}

	private void FixedUpdate()
	{
		CapsuleFollowHeadset();

		Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
		//debugLines[2] = $"Head Yaw: {headYaw}";
		Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
		//debugLines[3] = $"Direction: {direction}";

		bool isGrounded = CheckGrounded();
debugLines[2] = $"isGrounded: {isGrounded}";
		if (!isGrounded) fallingSpeed += gravity * Time.fixedDeltaTime;
		else fallingSpeed = 0;
		//debugLines[5] = $"Falling Speed: {fallingSpeed}";

		direction += Vector3.up * fallingSpeed;
		Vector3 finalDirection = direction * Time.fixedDeltaTime * speed;
debugLines[3] = $"Final Direction: {finalDirection}";
		character.Move(finalDirection);
		debugLines[8] = $"Debug transform: {debugTransform.position}";
	}

	private bool CheckGrounded()
	{
		Vector3 start = transform.TransformPoint(character.Position);
		float length = character.Position.y + 0.1f;
		bool hasHit = Physics.SphereCast(start, character.Radius, Vector3.down, out RaycastHit hit, length, ground);
		return hasHit;
	}

	private void CapsuleFollowHeadset()
	{
		character.Height = origin.CameraInOriginSpaceHeight;
		character.LateralCenter = transform.InverseTransformPoint(origin.Camera.transform.position);
debugLines[5] = $"Headset height: {origin.CameraInOriginSpaceHeight}";
debugLines[4] = $"Character height: {character.Height}";
debugLines[7] = $"Character Center: {character.LateralCenter}";
	}
}
