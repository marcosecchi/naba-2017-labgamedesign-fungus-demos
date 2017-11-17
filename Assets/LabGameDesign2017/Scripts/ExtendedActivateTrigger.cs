using UnityEngine;
using Object = UnityEngine.Object;

namespace TheBitCave.Utility
{
	[RequireComponent(typeof(Collider))]
	public class ExtendedActivateTrigger : MonoBehaviour
	{
		// A multi-purpose script which causes an action to occur when
		// a trigger collider is entered or exited.
		// This is an extension on the regular ActivateTrigger in the Standard Assets.
		public enum TriggerType {
			Enter = 0,		// The trigger is entered
			Exit = 1		// The trigger is exited
		}

		// A multi-purpose script which causes an action to occur when
		// a trigger collider is entered.
		public enum Mode {
			Trigger = 0,		// Just broadcast the action on to the target
			Replace = 1,		// replace target with source
			Activate = 2,		// Activate the target GameObject
			Enable = 3,			// Enable a component
			Animate = 4,		// Start animation on target
			Deactivate = 5		// Decativate target GameObject
		}

		public TriggerType triggerType = TriggerType.Enter;	// The trigger that should activate the actions (enter or exit)
		public string otherTag = "";						// The tag of the element entering/exiting the trigger
		public Mode action = Mode.Activate;					// The action to accomplish
		public Object target;								// The game object to affect. If none, the trigger work on this game object
		public GameObject source;							// Used in the 'Replace' case
		public int triggerCount = 1;
		public bool repeatTrigger = false;

		private void DoActivateTrigger () {
			triggerCount--;

			if (triggerCount == 0 || repeatTrigger) {
				Object currentTarget = target ?? gameObject;
				Behaviour targetBehaviour = currentTarget as Behaviour;
				GameObject targetGameObject = currentTarget as GameObject;
				if (targetBehaviour != null) {
					targetGameObject = targetBehaviour.gameObject;
				}

				switch (action) {
				case Mode.Trigger:
					if (targetGameObject != null) {
						targetGameObject.BroadcastMessage ("DoActivateTrigger");
					}
					break;
				case Mode.Replace:
					if (source != null) {
						if (targetGameObject != null) {
							Instantiate (source, targetGameObject.transform.position,
								targetGameObject.transform.rotation);
							DestroyObject (targetGameObject);
						}
					}
					break;
				case Mode.Activate:
					if (targetGameObject != null) {
						targetGameObject.SetActive (true);
					}
					break;
				case Mode.Enable:
					if (targetBehaviour != null) {
						targetBehaviour.enabled = true;
					}
					break;
				case Mode.Animate:
					if (targetGameObject != null) {
						targetGameObject.GetComponent<Animation> ().Play ();
					}
					break;
				case Mode.Deactivate:
					if (targetGameObject != null) {
						targetGameObject.SetActive (false);
					}
					break;
				}
			}
		}


		private void OnTriggerExit (Collider other) {
			if (triggerType == TriggerType.Exit && (string.IsNullOrEmpty(otherTag) || otherTag == other.tag))
				DoActivateTrigger ();
		}


		private void OnTriggerEnter (Collider other) {
			if (triggerType == TriggerType.Enter && (string.IsNullOrEmpty(otherTag) || otherTag == other.tag))
				DoActivateTrigger ();
		}
	}
}
