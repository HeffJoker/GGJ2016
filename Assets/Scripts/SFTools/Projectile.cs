using UnityEngine;
using System.Collections;

namespace SF_Tools.Weapons
{
	public abstract class Projectile : MonoBehaviour 
	{
		public float MaxLifeTime = 2.0f;
		public float ShotSpeed = 4.0f;
		public string[] CollisionTags;

		protected  float currLifeTime = 0f;
		internal bool isAlive = false;
		protected  SpriteRenderer sprite;

		// Use this for initialization
		void Start () 
		{
			sprite = gameObject.GetComponent<SpriteRenderer>();
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(isAlive)
			{
				currLifeTime -=	Time.deltaTime;
				if(currLifeTime <= 0)
				{
					Die();
				}
			}
		}

		public void Move(Vector3 position, Vector2 direction)
		{
			if(!isAlive)
			{
                gameObject.SetActive(true);
				transform.position = position;
				gameObject.GetComponent<Rigidbody2D>().AddForce(direction * ShotSpeed);
				currLifeTime = MaxLifeTime;
				isAlive = true;
				sprite.color = Color.white;
			}
		}

		protected void Die()
		{            
			isAlive = false;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			sprite.color = Color.clear;
			OnDeath();
            gameObject.SetActive(false);
		}

		protected abstract void OnTriggerEnter2D(Collider2D collider);

		protected virtual void OnDeath()
		{}
	}
}