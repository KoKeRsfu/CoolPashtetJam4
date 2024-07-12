﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dimension", order = 1)]
public class DimensionScriptable : ScriptableObject
{
	public float gravity;
	public float light;
	public float air;
	public bool multipleJumps;
	public bool invertedControls;	
}
