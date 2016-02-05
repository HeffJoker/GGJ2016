using System;
using UnityEngine;
using System.Collections;

public class AnimationClipOverride : MonoBehaviour {

	#region Editor Properties

	public bool InitOnWake = false;

	#endregion

	#region Private Members

	[Serializable]
	private class AnimationOverride
	{
		public string StateName;
		public AnimationClip OverrideState;
	}

	[SerializeField]
	private AnimationOverride[] Overrides;

	#endregion

	#region Public Interface

	public void Init(Animator animator)
	{
		AnimatorOverrideController overrideController = new AnimatorOverrideController();
		overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;

		foreach(AnimationOverride anim in Overrides)
		{
			overrideController[anim.StateName] = anim.OverrideState;
		}

		animator.runtimeAnimatorController = overrideController;
	}

	#endregion

	#region Private Routines

	private void Awake()
	{
		if(InitOnWake)
		{
			Animator animator = GetComponent<Animator>();
			if(animator != null)
				Init(animator);
			else
				Debug.LogError(string.Format("[ANIMATION_CLIP_OVERRIDE]: Could not find an animator attached the object: {0}", this.gameObject.name));
		}
	}

	#endregion
}
