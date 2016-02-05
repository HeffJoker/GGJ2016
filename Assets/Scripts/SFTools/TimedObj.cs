using UnityEngine;
using System.Collections;

public class TimedObj : MonoBehaviour {

	#region Editor Properties

	public float LifeSpan = 0.5f;
	public float FadeStart = 0.25f;
	public bool FadeAway = false;
	public bool DestroyObj = false;

	#endregion

	#region Private Members

	private SpriteRenderer sprite = null;
	private Color transparent = new Color(1f, 1f, 1f, 0f);

	#endregion

	#region Private Routines

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();

		if(sprite == null)
			sprite = GetComponentInChildren<SpriteRenderer>();
	}

	IEnumerator StartTime()
	{
        if (FadeAway && FadeStart < LifeSpan)
        {
            yield return new WaitForSeconds(FadeStart);

            float timeToFade = LifeSpan - FadeStart;
            float currTime = timeToFade;
            while (currTime > 0)
            {
                sprite.color = Color.Lerp(transparent, Color.white, currTime / timeToFade);
                currTime -= Time.deltaTime;
                yield return null;
            }

            Die();
        }
        else
        {
            yield return new WaitForSeconds(LifeSpan);
            Die();
        }
	}

	void Die()
	{
        gameObject.SetActive(false); 

        if (DestroyObj)
            GameObject.Destroy(gameObject);           
	}

    void OnEnable()
    {
        StartCoroutine(StartTime());
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(transform.position, 0.1f);
	}

	#endregion
}
