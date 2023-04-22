using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
	public XRController rightTeleportRay;
	public XRController leftTeleportRay;
	public InputHelpers.Button teleportActivationButton;
	public float activationThreshhold = 0.1f;

	void Update()
	{
		if (leftTeleportRay != null)
		{
			leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));
		}
		if (rightTeleportRay != null)
		{
			rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay));
		}
	}

	bool CheckIfActivated(XRController controller)
	{
		InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated);
		return isActivated;
	}
}
