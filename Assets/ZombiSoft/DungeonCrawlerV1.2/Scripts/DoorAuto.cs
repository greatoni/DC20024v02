//==============================================================
// DoorAuto (C) 2024 Zombisoft
// Attach to door
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class DoorAuto : MonoBehaviour
	{
		//==============================================================
		// Variables DOORS
		//==============================================================

		public Transform doorTransform; // The Door transform
		public Collider DoorCollider; // Door Collider

		public float delayBeforeOpen = 0.5f;
		public float distanceToMove = 3.45f; // How far will it go
		public float timeDuringOpening = 0.5f;
		public float timeDuringClosing = 0.3f;
		public float delayBeforeClose = 3.0f;
		public bool stayOpen = false;

		// Door closed position
		private Vector3 closedPosition;
		private Vector3 openedPosition;

		// LERP Variables
		private float timeStartLerp;
		private float timeSinceStarted;
		private float doorOpeningComplete;
		private float doorClosingComplete;

		// Logic
		private bool doorOpening = false;
		private bool doorOpen = false;

		//==============================================================
		// Start
		//==============================================================
		void Start()
		{
			// Define closed & opened positions
			closedPosition = doorTransform.transform.localPosition;
			openedPosition = doorTransform.transform.localPosition + doorTransform.TransformDirection(Vector3.up) * distanceToMove;

			if (doorOpen)
				this.doorTransform.transform.localPosition = openedPosition;
		}

		//==============================================================
		// On Trigger Enter
		//==============================================================
		protected void OnTriggerEnter(Collider t)
		{
			if (!doorOpen && !doorOpening) // Door is not opened and opening
			{
				gameObject.GetComponent<BoxCollider>().enabled = false;
				StartCoroutine("OpenDoor");
			}
		}

		//==============================================================
		// Open the door
		//==============================================================
		IEnumerator OpenDoor()
		{
			yield return new WaitForSeconds(delayBeforeOpen);

			timeStartLerp = Time.time;
			doorOpeningComplete = 0f;

			doorOpening = true;

			while (doorOpeningComplete < 1.0f)
			{
				timeSinceStarted = Time.time - timeStartLerp;
				doorOpeningComplete = timeSinceStarted / timeDuringOpening;

				doorTransform.transform.localPosition = Vector3.Lerp(closedPosition, openedPosition, doorOpeningComplete);

				yield return null;
			}

			DoorCollider.gameObject.SetActive(false);

			doorOpening = false;
			doorOpen = true;

			if (stayOpen)
			{
				yield return null;
			}
			else
			{
				yield return new WaitForSeconds(delayBeforeClose);
				yield return StartCoroutine("CloseDoor");
			}
		}

		//==============================================================
		// Close the door
		//==============================================================
		IEnumerator CloseDoor()
		{
			DoorCollider.gameObject.SetActive(true);

			timeStartLerp = Time.time;
			doorClosingComplete = 0f;

			while (doorClosingComplete < 1.0f)
			{
				timeSinceStarted = Time.time - timeStartLerp;
				doorClosingComplete = timeSinceStarted / timeDuringClosing;

				doorTransform.transform.localPosition = Vector3.Lerp(openedPosition, closedPosition, doorClosingComplete);

				yield return null;
			}

			gameObject.GetComponent<BoxCollider>().enabled = true;

			doorOpen = false;
		}
	}
}