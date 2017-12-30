﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	private float fillAmount;

	[SerializeField]
	private float lerpSpeed;

	[SerializeField]
	private Image content;

	[SerializeField]
	private Text valueText;

	public float MaxValue { get; set; }

	public float Value
	{
		set
		{
			string[] tmp = valueText.text.Split(':');
			valueText.text = tmp [0] + ":" + value;
			fillAmount = Map (value, 0, MaxValue, 0, 1);
		}
				

	}

	void Update () {
		handleBar ();
	}

	private void handleBar()
	{
		if (fillAmount != content.fillAmount) {
			content.fillAmount = Mathf.Lerp (content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}
	}

	private float Map (float val, float inMin, float inMax, float outMin,float outMax)
	{
		return ((val - inMin) * (outMax - outMin)) / ((inMax - inMin) + outMin);
	}
}
