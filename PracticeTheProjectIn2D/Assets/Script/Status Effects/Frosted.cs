using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Frosted : IStatusEffects
{
	//Dot damage
	private NewPlayer afflicted;

	public Sprite icon;
	public bool activated;

	public int stack = 1;

	public float duration = 8f;
	public float timer = 0;
	public float tickRate = 1;
	public float tickDamage = 0.5f;

	public float tickCounter;

	protected GameObject debuff;
	protected GameObject tooltip;

	Text debuffTimer;
	Text toolTipText;
	Image debuffIcon;

	public override void Begin (NewPlayer character)
	{
		afflicted = character;

		afflicted.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = Color.blue;

		icon = Resources.Load<Sprite> ("Frost Icon");

		activated = true;

		DisplayDebuff ();

		tooltip.SetActive (false);


	}

	public override void Effect ()
	{
		afflicted.healthStat.CurrentVal -= tickDamage * stack;

		tickCounter = 0;
	}

	public override void Duration ()
	{
		timer += Time.deltaTime;
		tickCounter += Time.deltaTime;

		//If the timer is pass the duration
		if (timer >= duration * stack)
		{
			//Stop the effect

			Stop ();

			return;
		}

		//If the timer is below the duration
		if (timer < duration * stack)
		{
			if (tickCounter >= tickRate / stack)
			{
				Effect ();
			}
		}

		UpdateDebuff ();
		ToolTip ();

	}

	public override void Stop ()
	{
		Debug.Log ("Ending Frosted");

		//afflicted.status.Find(this) = null;
		afflicted.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = Color.white;
		activated = false;

		Destroy (debuff);
		Destroy (this);

	}

	public void DisplayDebuff ()
	{

		debuff = Instantiate (afflicted.placeholder, new Vector3 (0, 0, 0), Quaternion.identity, afflicted.statusParent);
		debuff.name = "Frosted";

		debuffTimer = debuff.GetComponentInChildren<Text> ();
		debuffIcon = debuff.GetComponent<Image> ();
		tooltip = debuff.transform.Find ("Tooltip").gameObject;
		toolTipText = tooltip.GetComponentInChildren<Text> ();

		debuffIcon.sprite = icon;

	}

	public void UpdateDebuff ()
	{
		debuffTimer.text = "" + Mathf.Round (timer);

	}

	public void ToolTip ()
	{
		
		PointerEventData pointer = new PointerEventData (EventSystem.current);

		pointer.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult> ();

		if (EventSystem.current.IsPointerOverGameObject ())
		{
			results.Clear ();
			EventSystem.current.RaycastAll (pointer, results);

			foreach (RaycastResult obj in results)
			{

				if (obj.gameObject.name == debuff.gameObject.name)
				{
					Debug.Log (obj.gameObject.name);

					this.toolTipText.text = "Frosted Touch: Taking Damage overtime";
					this.tooltip.SetActive (true);
				}
			}
		}


		if (!EventSystem.current.IsPointerOverGameObject ())
		{
			this.tooltip.SetActive (false);

		}
			
	}

}
