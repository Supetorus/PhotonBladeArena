using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private float health;

	void Start()
	{

	}

	void Update()
	{

	}

	internal void TakeDamage(float damage)
	{
		health = MathF.Min(health, health - damage);
	}
}
