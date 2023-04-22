using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
	//Inspector
	public bool showController = false;
	public InputDeviceCharacteristics characteristics;
	public List<GameObject> controllerPrefabs = new List<GameObject>();
	public GameObject handModelPrefab;

	//Objects
	private InputDevice targetDevice;
	private GameObject spawnedController;
	private GameObject spawnedHandModel;
	private Animator HandAnimator;
	private Lightsaber lightsaber;

	//Inputs
	private float triggerValue;
	private float gripValue;
	private Vector2 primary2DAxisValue;
	private bool primaryButtonValue;
	private bool secondaryButtonValue;

	//State
	private bool initialized = false;

	//debugging
	//private TMPro.TMP_Text handText;


	void Start()
	{
		//handText = ((characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left) ? GameObject.Find("Left Hand Text").GetComponent<TMP_Text>() : GameObject.Find("Right Hand Text").GetComponent<TMP_Text>();

		TryInitialize();
	}

	void Update()
	{
		if (!initialized) TryInitialize(); // In case the device was not available when start was run.

		GetControllerInputs();
		if (!showController) UpdateHandAnimation();
		spawnedHandModel?.SetActive(!showController);
		spawnedController?.SetActive(showController);

		if (gripValue > 0.1f) lightsaber.Enable();
		else lightsaber.Disable();

		HandDebugText();
	}

	private void TryInitialize()
	{
		List<InputDevice> devices = new List<InputDevice>();
		InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

		if (devices.Count > 0)
		{
			initialized = true;
			targetDevice = devices[0];
			GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
			if (prefab != null) spawnedController = Instantiate(prefab, transform);
			else spawnedController = Instantiate(controllerPrefabs[0], transform); // The default controller model
		}

		spawnedHandModel = Instantiate(handModelPrefab, transform);
		HandAnimator = spawnedHandModel.GetComponent<Animator>();

		lightsaber = GetComponentInChildren<Lightsaber>();
		//Console.Instance.Log($"Found lightsaber: {lightsaber != null}");
	}

	private void UpdateHandAnimation()
	{
		HandAnimator.SetFloat("Trigger", triggerValue);
		HandAnimator.SetFloat("Grip", gripValue);
	}

	private void GetControllerInputs()
	{
		targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue);
		targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonValue);
		targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisValue);
		targetDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
		targetDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
	}

	private void HandDebugText()
	{
		//handText.text = $"{targetDevice.name}\n{targetDevice.characteristics}";

		//if (triggerValue > 0) handText.text += "\nTrigger Pressed"; ;
		//if (gripValue > 0) handText.text += "\nGripp Pressed";
		//if (primary2DAxisValue != Vector2.zero) handText.text += $"\nPrimary 2D Axis: {primary2DAxisValue}";
		//if (primaryButtonValue) handText.text += "\nPrimary button pressed";
		//if (secondaryButtonValue) handText.text += "\nSecondary button pressed";
	}
}
