//==============================================================
// DungeonCrawlerController (C) 2024 Zombisoft
// Attach to the Player Object
//==============================================================

using System.Collections;
using UnityEngine;

namespace zombisoft
{
	public class DungeonCrawlerController : MonoBehaviour
	{
		//==============================================================
		// Controllertype
		//==============================================================
		[Header("Controllertype")]
		public bool Keys = true;
		public bool Swipe;
		public bool Arrows;
		public GameObject ArrowButtons;

		//==============================================================
		// Physics RayCast
		//==============================================================
		[Header("Physics RayCast")]
		public float castDistance = 4f; // Physics RayCast distance to check obsticle

		//==============================================================
		// Key bindings
		//==============================================================

		[Header("Keybindings")]
		public string Forward = "s";
		public string Backward = "x";
		public string Strafeleft = "z";
		public string Straferight = "c";
		public string Turnleft = "a";
		public string Turnright = "d";

		//==============================================================
		// Move States
		//==============================================================
		public static int moveState; // Default None (0)
		public const int moveStateIdle = 0;
		public const int moveStateForward = 1;
		public const int moveStateBackward = 2;
		public const int moveStateLeft = 3;
		public const int moveStateRight = 4;
		public const int moveStateRotateLeft = 5;
		public const int moveStateRotateRight = 6;
		public const int moveStateFreelook = 7;
		public const int moveStateResetFreelook = 8;

		//==============================================================
		// Distance to walk. Default 4 units
		//==============================================================
		[Header("Distance Unity Units")]
		public float walkDistance = 4.0f; // Unity units to move

		//==============================================================
		//The start and finish positions for the walk, bounce interpolation
		//==============================================================
		private Vector3 curPos;
		private Vector3 newPos;

		// Temporay collider to prevent player & enemy move to the same position
		public GameObject tempCollider;

		//==============================================================
		// Timers for walk, rotate, bounce and reset rotation
		//==============================================================
		[Header("Timers")]
		public float walkDuration = 0.4f; // Walk duration in seconds
		public float rotateDuration = 0.2f; // Rotate duration in seconds
		public float bounceDuration = 0.1f; // Bounce duration in seconds
		public float bounceRange = 0.1f; // Bounce range
		public float resetDuration = 0.2f;

		//==============================================================
		// Head Bobbing
		//==============================================================
		[Header("Bobbing")]
		public bool bobEnabled;
		public Camera playerCamera;
		public AnimationCurve bobCurve;
		[Range(-0.2f, 0.0f)]
		public float bobAmount;

		//==============================================================
		// FreeLook
		//==============================================================
		[Header("FreeLook")]
		public GameObject freelook;
		public int xMinLimit = -80;
		public int xMaxLimit = 80;
		public int yMinLimit = -70;
		public int yMaxLimit = 70;
		public float xSpeed = 5.0f;
		public float ySpeed = 5.0f;
		public float zoomDampening = 12.0f;

		public static bool freezePlayerInput = false;// Disable Player Movement Input
		
		private float xAngle;
		private float yAngle;
		private Quaternion curRot;
		private Quaternion desiredRot;

		//==============================================================
		// Start
		//==============================================================
		void Start()
		{
			//==============================================================
			// Create animation curve for bobbing
			//==============================================================
			bobCurve = new AnimationCurve(new Keyframe(0, 0));
			bobCurve.AddKey(0.7f, bobAmount);
			bobCurve.AddKey(0.9f, bobAmount / 2);
			bobCurve.AddKey(1.0f, 0.0f);

			CardinalPoints.Instance.NewDirection(transform); // Set Compass

			SwipeManager.OnSwipeDetected += OnSwipeDetected;

			ArrowButtons.SetActive(Arrows);
		}

		//==============================================================
		// Mobile Swipe
		//==============================================================
		void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity)
		{
			if (Swipe && !freezePlayerInput && moveState == moveStateIdle)
			{
				if (SwipeManager.IsSwipingUp())
					playerMoveForward();
				if (SwipeManager.IsSwipingDown())
					playerMoveBackward();
				if (SwipeManager.IsSwipingLeft())
					playerStrafeLeft();
				if (SwipeManager.IsSwipingRight())
					playerStrafeRight();
				if (SwipeManager.IsSwipingUpLeft())
					playerRotate(-90, rotateDuration);
				if (SwipeManager.IsSwipingDownRight())
					playerRotate(90, rotateDuration);
				if (SwipeManager.IsSwipingDownLeft())
					playerRotate(-90, rotateDuration);
				if (SwipeManager.IsSwipingUpRight())
					playerRotate(90, rotateDuration);
			}
		}

		//==============================================================
		// Update
		//==============================================================
		void Update()
		{
			if (Keys && !freezePlayerInput)
			{
				//==============================================================
				// MoveStates
				//==============================================================
				switch (moveState)
				{
					//==============================================================
					// MoveState Idle
					//==============================================================
					case moveStateIdle:
						if (Input.GetKey(Forward))
							playerMoveForward();
						if (Input.GetKey(Backward))
							playerMoveBackward();
						if (Input.GetKey(Strafeleft))
							playerStrafeLeft();
						if (Input.GetKey(Straferight))
							playerStrafeRight();
						if (Input.GetKey(Turnleft))
							playerRotate(-90, rotateDuration);
						if (Input.GetKey(Turnright))
							playerRotate(90, rotateDuration);

						break;
					//==============================================================
					// Default
					//==============================================================
					default:
						break;
				}
			}
			if (Arrows)
				ArrowButtons.SetActive(Arrows);
			else
				ArrowButtons.SetActive(Arrows);

		}

		//==============================================================
		// Late Update
		//==============================================================
		void LateUpdate()
		{
			if (!freezePlayerInput)
				{
				//==============================================================
				// Move States
				//==============================================================
				switch (moveState)
				{
					//==============================================================
					// MoveState Idle
					//==============================================================
					case moveStateIdle:

						// Right Mouse Button is pressed
						if (Input.GetMouseButtonDown(1))
						{
							xAngle = 0;
							yAngle = 0;
							moveState = moveStateFreelook;
						}
						break;
      	
					//==============================================================
					// MoveState Freelook
					//==============================================================
					case moveStateFreelook:

						// Right Mouse Button is hold
						if (Input.GetMouseButton(1))
						{
							FreeLook();
						}

						// If Right Mouse Button is released
						if (Input.GetMouseButtonUp(1))
						{
							// Check angle of rotation. Will we rotate?
							if (xAngle >= 45)
							{
								playerRotate(90, resetDuration);
								StartCoroutine("ResetFreeLook", resetDuration);
							}
							else if (xAngle <= -45)
							{
								playerRotate(-90, resetDuration);
								StartCoroutine("ResetFreeLook", resetDuration);
							}
							// Just resetfreelook without rotation
							else
							{
								StartCoroutine("ResetFreeLook", resetDuration);
								moveState = moveStateResetFreelook;
							}
						}
      	
						break;
      	
					//==============================================================
					// MoveState ResetFreeLook
					//==============================================================
					case moveStateResetFreelook:
						break;
      	
					//==============================================================
					// Default
					//==============================================================
					default:
						break;
				}
			}
		}

		//==============================================================
		// RayCast Check. Bump into stuff =)
		//==============================================================
		bool CheckObsticle(float Distance, Vector3 endPos)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, endPos, out hit, Distance)) // Does the ray intersect any objects
			{
				if ((hit.transform.tag == "Obsticle" || hit.transform.tag == "Enemy" || hit.transform.tag == "TmpCollider")) // Is it an obsticle..
				{
					Debug.DrawRay(transform.position, endPos * hit.distance, Color.yellow); // Debug
					return true;
				}
				else
					return false;
			}
			else
			{
				Debug.DrawRay(transform.position, endPos * 1000, Color.white); // Debug
				return false;
			}
		}

		//==============================================================
		// Player Moving
		//==============================================================
		public void playerMoveForward()
		{
			if (moveState == moveStateIdle)
			{
				moveState = moveStateForward;
				if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.forward)))
				{
					StartCoroutine("BouncePlayer");
				}
				else
				{
					curPos = transform.position;
					newPos = curPos + transform.TransformDirection(Vector3.forward) * walkDistance;
					StartCoroutine("MovePlayer", newPos);
				}
			}
		}

		public void playerMoveBackward()
		{
			if (moveState == moveStateIdle)
			{
				moveState = moveStateBackward;
				if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.back)))
				{
					StartCoroutine("BouncePlayer");
				}
				else
				{
					curPos = transform.position;
					newPos = curPos + transform.TransformDirection(Vector3.back) * walkDistance;
					StartCoroutine("MovePlayer", newPos);
				}
			}
		}

		public void playerStrafeLeft()
		{
			if (moveState == moveStateIdle)
			{
				moveState = moveStateLeft;
				if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.left)))
				{
					StartCoroutine("BouncePlayer");
				}
				else
				{
					curPos = transform.position;
					newPos = curPos + transform.TransformDirection(Vector3.left) * walkDistance;
					StartCoroutine("MovePlayer", newPos);
				}
			}
		}

		public void playerStrafeRight()
		{
			if (moveState == moveStateIdle)
			{
				moveState = moveStateRight;
				if (CheckObsticle(castDistance, transform.TransformDirection(Vector3.right)))
				{
					StartCoroutine("BouncePlayer");
				}
				else
				{
					curPos = transform.position;
					newPos = curPos + transform.TransformDirection(Vector3.right) * walkDistance;
					StartCoroutine("MovePlayer", newPos);
				}
			}
		}

		public void playerRotateLeft(float newAngle)
		{
			if (moveState == moveStateIdle)
			{
				newAngle = -90;
				moveState = moveStateRotateLeft;

				StartCoroutine(RotatePlayer(new Vector3(0, newAngle, 0), rotateDuration));
			}
		}

		public void playerRotateRight(float newAngle)
		{
			if (moveState == moveStateIdle)
			{
				newAngle = 90;
				moveState = moveStateRotateRight;

				StartCoroutine(RotatePlayer(new Vector3(0, newAngle, 0), rotateDuration));
			}
		}

		public void playerRotate(float newAngle, float duration)
		{
			if (newAngle == -90)
				moveState = moveStateRotateLeft;
			if (newAngle == 90)
				moveState = moveStateRotateRight;

			StartCoroutine(RotatePlayer(new Vector3(0, newAngle, 0), duration));
		}

		//==============================================================
		// Coroutine: MovePlayer 4 directions
		//==============================================================
		IEnumerator MovePlayer(Vector3 endPosition)
		{
			// Prevent Enemies and player to move to the same position
			GameObject tmpCollider = Instantiate(tempCollider, endPosition, transform.rotation); // Create protecting collider
			Destroy(tmpCollider, walkDuration); // Destroy protecting collider

			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			if (bobEnabled)
				StartCoroutine(Bob(bobCurve, walkDuration));

			while (lerpComplete < 1.0f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / walkDuration;

				transform.position = Vector3.Lerp(curPos, endPosition, lerpComplete);

				yield return null;
			}

			moveState = moveStateIdle;
		}

		//==============================================================
		// Coroutine: Rotate Player Left or Right
		//==============================================================
		IEnumerator RotatePlayer(Vector3 eulerAngles, float duration)
		{
			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			Vector3 currentRot = transform.rotation.eulerAngles;
			Vector3 newRot = transform.rotation.eulerAngles + eulerAngles;

			while (lerpComplete < 1.0f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / duration;

				transform.eulerAngles = Vector3.Lerp(currentRot, newRot, lerpComplete);

				yield return null;
			}

			CardinalPoints.Instance.NewDirection(transform);

			moveState = moveStateIdle;
		}

		//==============================================================
		// FreeLook stuff!!
		//==============================================================
		private void FreeLook()
		{
			xAngle += Input.GetAxis("Mouse X") * (xSpeed);
			yAngle -= Input.GetAxis("Mouse Y") * (ySpeed);

			xAngle = ClampAngle(xAngle, xMinLimit, xMaxLimit);
			yAngle = ClampAngle(yAngle, yMinLimit, yMaxLimit);

			desiredRot = Quaternion.Euler(yAngle, xAngle, 0);
			curRot = freelook.transform.localRotation;

			freelook.transform.localRotation = Quaternion.Lerp(curRot, desiredRot, Time.deltaTime * zoomDampening);
		}

		//==============================================================
		// Coroutine: Reset FreeLook
		//==============================================================
		IEnumerator ResetFreeLook(float duration)
		{
			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			Quaternion currentRot = freelook.transform.localRotation;

			while (lerpComplete < 1.0f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / duration;

				freelook.transform.localRotation = Quaternion.Lerp(currentRot, Quaternion.Euler(0, 0, 0), lerpElapsedTime / duration);

				yield return null;
			}

			moveState = moveStateIdle;
		}

		//==============================================================
		// Coroutine: Bounce Player
		//==============================================================
		IEnumerator BouncePlayer()
		{
			float lerpStartTime;
			float lerpElapsedTime;
			float lerpComplete;

			lerpStartTime = Time.time;
			lerpComplete = 0f;

			curPos = transform.position;
			if (moveState == moveStateForward)
				newPos = curPos + transform.TransformDirection(Vector3.forward) * bounceRange;
			if (moveState == moveStateBackward)
				newPos = curPos + transform.TransformDirection(Vector3.back) * bounceRange;
			if (moveState == moveStateLeft)
				newPos = curPos + transform.TransformDirection(Vector3.left) * bounceRange;
			if (moveState == moveStateRight)
				newPos = curPos + transform.TransformDirection(Vector3.right) * bounceRange;

			while (lerpComplete < 1.0f)
			{
				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / bounceDuration;
				transform.position = Vector3.Lerp(curPos, newPos, lerpComplete);

				lerpElapsedTime = Time.time - lerpStartTime;
				lerpComplete = lerpElapsedTime / bounceDuration;
				transform.position = Vector3.Lerp(newPos, curPos, lerpComplete);

				yield return null;
			}

			moveState = moveStateIdle;
		}

		//==============================================================
		// Coroutine: Head Bobbing
		//==============================================================
		IEnumerator Bob(AnimationCurve bobCurve, float time)
		{
			float timer = 0.0f;
			Vector3 cameraCurPos = playerCamera.transform.localPosition;

			while (timer <= time)
			{
				playerCamera.transform.localPosition = new Vector3(cameraCurPos.x, bobCurve.Evaluate(timer / time), cameraCurPos.z);
				timer += Time.deltaTime;
				yield return null;
			}
		}

		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f) angle += 360f;
			if (angle > 360f) angle -= 360f;
			return Mathf.Clamp(angle, min, max);
		}
	}
}
