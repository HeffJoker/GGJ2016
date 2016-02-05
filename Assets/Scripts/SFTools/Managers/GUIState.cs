using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SF_Tools.Managers;

namespace SF_Tools.GUI
{
	public class GUIState : MonoBehaviour
	{
		#region Editor Properties

		public string Name = string.Empty;
		public float Speed = 1.0f;
		public bool Stackable = false;
        public bool ControlChildAnimators = true;

		#endregion

		#region Private Members
		
		private List<Animator> animators = new List<Animator>();
        private Text[] textObjs = null;
		private bool enterDone = false;
		private bool exitDone = false;
		private Canvas canvas;

		#endregion

		#region Public Properties

		public bool EnterDone
		{
			get { return enterDone; }
		}

		public bool ExitDone
		{
			get { return exitDone; }
		}

		public bool IsHidden
		{
			get { return !canvas.enabled; }
		}

		#endregion

		#region Public Interface

        public void Start()
        {
            GUIStateManager.Instance.AddState(this);
        }

		public void Initialize()
		{
			Animator animator = GetComponent<Animator>();
			
			if(animator != null)
				animators.Add(animator);
			
            if(ControlChildAnimators)
			    animators.AddRange(GetComponentsInChildren<Animator>());

			animators.ForEach(x => x.speed = Speed);

			canvas = GetComponent<Canvas>();
            //canvas.gameObject.SetActive(false);
            //canvas.enabled = false;


            textObjs = GetComponentsInChildren<Text>();
            ToggleTextObjs(false);
		}
		
		public void Enter()
		{
			enterDone = false;
            canvas.gameObject.SetActive(true);
			canvas.enabled = true;
            ToggleTextObjs(true);

            foreach (Animator animator in animators)
				animator.SetTrigger("IsEntering");
        }
		
		public void Exit()
		{
			exitDone = false;
            ToggleTextObjs(false);

            foreach (Animator animator in animators)
				animator.SetTrigger("IsLeaving");
        }
		
		public void Hide()
		{
            canvas.gameObject.SetActive(false);
			canvas.enabled = false;
            ToggleTextObjs(false);

            foreach (Animator animator in animators)
			{
				animator.ResetTrigger("IsEntering");
				animator.ResetTrigger("IsLeaving");
				animator.SetTrigger("IsHiding");
			}
		}

        private void ToggleTextObjs(bool isActive)
        {
            for (int i = 0; i < textObjs.Length; ++i)
                textObjs[i].gameObject.SetActive(isActive);
        }

		[SerializeField]
		private void NotifyEnterDone()
		{
			enterDone = true;	
		}

		[SerializeField]
		private void NotifyExitDone()
		{
			exitDone = true;
            canvas.enabled = false;
		}

		#endregion
	}
}
