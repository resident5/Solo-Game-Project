using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffects{

	void Start (NewCharacters character);
	void Effect();
	void Duration();
	void Stop();

}
