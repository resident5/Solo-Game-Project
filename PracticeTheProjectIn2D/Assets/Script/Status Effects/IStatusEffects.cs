using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStatusEffects : MonoBehaviour
{
	

	public abstract void Begin (NewPlayer character);
	public abstract void Effect ();
	public abstract void Duration ();
	public abstract void Stop ();

}
