using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	public Material[] materials;
	public MaterialDesignation[] materialDesignations;

	private new Renderer renderer;

	private void Start()
	{
		renderer = GetComponent<Renderer>();
	}

	public void ChangeMaterial(Material material)
	{
		renderer.material = material;
	}

	public void ChangeMaterial(string materialName)
	{
		foreach (Material m in materials)
		{
			if (m.name == materialName)
			{
				renderer.material = m;
				return;
			}
		}

		foreach (MaterialDesignation m in materialDesignations)
		{
			if (m.name == materialName)
			{
				renderer.material = m.material;
				return;
			}
		}
	}

	[System.Serializable]
	public struct MaterialDesignation
	{
		public string name;
		public Material material;
	}
}
