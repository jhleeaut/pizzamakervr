using System;
using UnityEngine;
/*
[RequireComponent(typeof(CharacterController))]
public class OVRPlayerController : MonoBehaviour
{
    /// <summary>
    /// 이동 ​​중 속도 가속도.
    /// </summary>
    public float Acceleration = 0.1f;

    /// <summary>
    /// 운동 댐핑 속도.
    /// </summary>
    public float Damping = 0.3f;

    /// <summary>
    /// 옆이나 뒤로 움직일 때의 추가 감쇠 비율.
    /// </summary>
    public float BackAndSideDampen = 0.5f;

    /// <summary>
    /// 점프 할 때 캐릭터에 적용되는 힘.
    /// </summary>
    public float JumpForce = 0.3f;

    /// <summary>
    /// 게임 패드를 사용할 때의 회전 속도.
    /// </summary>
    public float RotationAmount = 1.5f;

    /// <summary>
    /// 키보드를 사용할 때의 회전 속도.
    /// </summary>
    public float RotationRatchet = 45.0f;

    /// <summary>
    /// 플레이어는 스냅 회전이 활성화되어 있으면 고정 된 단계로 회전합니다.
    /// </summary>
    [Tooltip("플레이어는 스냅 회전이 활성화되어 있으면 고정 된 단계로 회전합니다.")]
	public bool SnapRotation = true;

    /// <summary>
    /// 선형 이동과 함께 사용할 고정 속도는 몇 개입니까? 0 = 선형 제어
    /// </summary>
    [Tooltip("선형 이동과 함께 사용할 고정 속도는 몇 개입니까? 0 = 선형 제어")]
	public int FixedSpeedSteps;

    /// <summary>
    ///  true 인 경우, Hmd 포즈를 다시 올렸을 때 플레이어 컨트롤러의 초기 동작을 재설정합니다.
    /// </summary>
    public bool HmdResetsY = true;

    /// <summary>
    /// true 인 경우 OVRCameraRig 하위에서 데이터를 추적하면 이동 방향이 업데이트됩니다.
    /// </summary>
    public bool HmdRotatesY = true;

    /// <summary>
    /// 중력의 강도를 수정합니다.
    /// </summary>
    public float GravityModifier = 0.379f;

    /// <summary>
    /// true 인 경우 각 OVRPlayerController는 플레이어의 실제 높이를 사용합니다.
    /// </summary>
    public bool useProfileData = true;

    /// <summary>
    /// CameraHeight는 HMD의 실제 높이이며 캐릭터 컨트롤러의 높이를 조절하는 데 사용할 수 있습니다.
    /// 성격이 천장이 낮은 지역으로 이동하는 능력.
    /// </summary>
    [NonSerialized]
	public float CameraHeight;

    /// <summary>
    ///이 이벤트는 문자 컨트롤러가 이동 한 후에 발생합니다. 이것은 OVRAvatarLocomotion 스크립트가 아바타 변환을 동기화 된 상태로 유지하는 데 사용됩니다.
    /// OVRPlayerController를 사용하십시오.
    /// </summary>
    public event Action<Transform> TransformUpdated;

    /// <summary>
    ///이 bool은 플레이어 컨트롤러가 텔레포트 될 때마다 true로 설정됩니다. 매 프레임마다 재설정됩니다. 일부 시스템 (예 :
    /// CharacterCameraConstraint, 캐릭터 컨트롤러를 즉시 움직이는 로직을 비활성화하려면이 부울을 테스트하십시오.
    /// 텔레포트를 따른다.
    /// </summary>
    [NonSerialized]//이 속성은 속성에서 볼 필요가 없습니다.
	public bool Teleported;

    /// <summary>
    /// 이 이벤트는 카메라 변형이 업데이트 된 직후에 이동이 업데이트되기 전에 발생합니다.
    /// </summary>
    public event Action CameraUpdated;

    /// <summary>
    /// 이 이벤트는 다른 시스템에 기회를 제공하기 위해 캐릭터 컨트롤러가 실제로 이동하기 바로 전에 발생합니다.
    /// HMD의 움직임과 같은 사용자 입력 이외의 것에 응답하여 캐릭터 컨트롤러를 움직입니다. CharacterCameraConstraint.cs를 참조하십시오.
    /// 예를 들어.
    /// </summary>
    public event Action PreCharacterMove;

    /// <summary>
    /// true이면 직선 이동에 사용자 입력이 적용됩니다. 플레이어 컨트롤러가 입력을 무시할 필요가있을 때마다 이것을 false로 설정하십시오.
    /// 직선 운동.
    /// </summary>
    public bool EnableLinearMovement = true;

    /// <summary>
    /// true이면 사용자 입력이 회전에 적용됩니다. 플레이어 컨트롤러가 입력을 무시해야 할 때마다 false로 설정하십시오.
    /// </summary>
    public bool EnableRotation = true;

	protected CharacterController Controller = null;
	protected OVRCameraRig CameraRig = null;

	private float MoveScale = 1.0f;
	private Vector3 MoveThrottle = Vector3.zero;
	private float FallSpeed = 0.0f;
	private OVRPose? InitialPose;
	public float InitialYRotation { get; private set; }
	private float MoveScaleMultiplier = 1.0f;
	private float RotationScaleMultiplier = 1.0f;
	private bool  SkipMouseRotation = true; // VR에서 마우스 움직임을 사용하는 것은 드문 경우이므로 마우스를 기본적으로 무시하십시오.
    private bool  HaltUpdateMovement = false;
	private bool prevHatLeft = false;
	private bool prevHatRight = false;
	private float SimulationRate = 60f;
	private float buttonRotation = 0f;
	private bool ReadyToSnapTurn; // 스냅 회전이 발생하면 true로 설정되고, 코드는 다른 스냅 회전을 가능하게하기 위해 가운데 썸 드로의 한 프레임을 필요로합니다.

    void Start()
	{
        // 플레이어 컨트롤러에서 카메라 오프셋으로 눈 깊이를 추가합니다.
        var p = CameraRig.transform.localPosition;
		p.z = OVRManager.profile.eyeDepth;
		CameraRig.transform.localPosition = p;
	}

	void Awake()
	{
		Controller = gameObject.GetComponent<CharacterController>();

		if(Controller == null)
			Debug.LogWarning("OVRPlayerController : No CharacterController가 연결되었습니다.");

        // OVRCameraRig를 사용하여 회전을 카메라에 설정합니다.
        // 회전의 영향을 받는다.
        OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

		if(CameraRigs.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
		else if (CameraRigs.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
		else
			CameraRig = CameraRigs[0];

		InitialYRotation = transform.rotation.eulerAngles.y;
	}

	void OnEnable()
	{
		OVRManager.display.RecenteredPose += ResetOrientation;

		if (CameraRig != null)
		{
			CameraRig.UpdatedAnchors += UpdateTransform;
		}
	}

	void OnDisable()
	{
		OVRManager.display.RecenteredPose -= ResetOrientation;

		if (CameraRig != null)
		{
			CameraRig.UpdatedAnchors -= UpdateTransform;
		}
	}

	void Update()
	{
        // 회전을 래칫하는 데 키 사용
        if (Input.GetKeyDown(KeyCode.Q))
			buttonRotation -= RotationRatchet;

		if (Input.GetKeyDown(KeyCode.E))
			buttonRotation += RotationRatchet;
	}

	protected virtual void UpdateController()
	{
		if (useProfileData)
		{
			if (InitialPose == null)
			{
                // 초기 포즈를 저장하여 useProfileData 인 경우 복구 할 수 있도록합니다.
                //는 나중에 꺼집니다.
                InitialPose = new OVRPose()
				{
					position = CameraRig.transform.localPosition,
					orientation = CameraRig.transform.localRotation
				};
			}

			var p = CameraRig.transform.localPosition;
			if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
			{
				p.y = OVRManager.profile.eyeHeight - (0.5f * Controller.height) + Controller.center.y;
			}
			else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
			{
				p.y = - (0.5f * Controller.height) + Controller.center.y;
			}
			CameraRig.transform.localPosition = p;
		}
		else if (InitialPose != null)
		{
            // 런타임에 useProfileData가 해제 된 경우 초기 포즈로 돌아갑니다.
            CameraRig.transform.localPosition = InitialPose.Value.position;
			CameraRig.transform.localRotation = InitialPose.Value.orientation;
			InitialPose = null;
		}

		CameraHeight = CameraRig.centerEyeAnchor.localPosition.y;

		if (CameraUpdated != null)
		{
			CameraUpdated();
		}

		UpdateMovement();

		Vector3 moveDirection = Vector3.zero;

		float motorDamp = (1.0f + (Damping * SimulationRate * Time.deltaTime));

		MoveThrottle.x /= motorDamp;
		MoveThrottle.y = (MoveThrottle.y > 0.0f) ? (MoveThrottle.y / motorDamp) : MoveThrottle.y;
		MoveThrottle.z /= motorDamp;

		moveDirection += MoveThrottle * SimulationRate * Time.deltaTime;

		// Gravity
		if (Controller.isGrounded && FallSpeed <= 0)
			FallSpeed = ((Physics.gravity.y * (GravityModifier * 0.002f)));
		else
			FallSpeed += ((Physics.gravity.y * (GravityModifier * 0.002f)) * SimulationRate * Time.deltaTime);

		moveDirection.y += FallSpeed * SimulationRate * Time.deltaTime;


		if (Controller.isGrounded && MoveThrottle.y <= transform.lossyScale.y * 0.001f)
		{
            // 고르지 않은지면에 대한 오프셋 보정
            float bumpUpOffset = Mathf.Max(Controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
			moveDirection -= bumpUpOffset * Vector3.up;
		}

		if (PreCharacterMove != null)
		{
			PreCharacterMove();
			Teleported = false;
		}

		Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), new Vector3(1, 0, 1));

		// Move contoller
		Controller.Move(moveDirection);
		Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 0, 1));

		if (predictedXZ != actualXZ)
			MoveThrottle += (actualXZ - predictedXZ) / (SimulationRate * Time.deltaTime);
	}





	public virtual void UpdateMovement()
	{
		if (HaltUpdateMovement)
			return;

		if (EnableLinearMovement)
		{
			bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
			bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
			bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
			bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

			bool dpad_move = false;

			if (OVRInput.Get(OVRInput.Button.DpadUp))
			{
				moveForward = true;
				dpad_move = true;

			}

			if (OVRInput.Get(OVRInput.Button.DpadDown))
			{
				moveBack = true;
				dpad_move = true;
			}

			MoveScale = 1.0f;

			if ((moveForward && moveLeft) || (moveForward && moveRight) ||
				(moveBack && moveLeft) || (moveBack && moveRight))
				MoveScale = 0.70710678f;

            // 우리가 공중에있을 때 위치 이동 없음
            if (!Controller.isGrounded)
				MoveScale = 0.0f;

			MoveScale *= SimulationRate * Time.deltaTime;

            // 키 움직임을 계산합니다.
            float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

			// Run!
			if (dpad_move || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				moveInfluence *= 2.0f;

			Quaternion ort = transform.rotation;
			Vector3 ortEuler = ort.eulerAngles;
			ortEuler.z = ortEuler.x = 0f;
			ort = Quaternion.Euler(ortEuler);

			if (moveForward)
				MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
			if (moveBack)
				MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);
			if (moveLeft)
				MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);
			if (moveRight)
				MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);



			moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

#if !UNITY_ANDROID // Android 게임 패드에서는 LeftTrigger가 유용하지 않습니다.
            moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
#endif

			Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            // 속도 양자화가 활성화 된 경우 입력을 고정 속도 단계 수로 조정합니다.
            if (FixedSpeedSteps > 0)
			{
				primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
				primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
			}

			if (primaryAxis.y > 0.0f)
				MoveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

			if (primaryAxis.y < 0.0f)
				MoveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence *
									   BackAndSideDampen * Vector3.back);

			if (primaryAxis.x < 0.0f)
				MoveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence *
									   BackAndSideDampen * Vector3.left);

			if (primaryAxis.x > 0.0f)
				MoveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * BackAndSideDampen *
									   Vector3.right);
		}

		if (EnableRotation)
		{
			Vector3 euler = transform.rotation.eulerAngles;
			float rotateInfluence = SimulationRate * Time.deltaTime * RotationAmount * RotationScaleMultiplier;

			bool curHatLeft = OVRInput.Get(OVRInput.Button.PrimaryShoulder);

			if (curHatLeft && !prevHatLeft)
				euler.y -= RotationRatchet;

			prevHatLeft = curHatLeft;

			bool curHatRight = OVRInput.Get(OVRInput.Button.SecondaryShoulder);

			if (curHatRight && !prevHatRight)
				euler.y += RotationRatchet;

			prevHatRight = curHatRight;

			euler.y += buttonRotation;
			buttonRotation = 0f;


#if !UNITY_ANDROID || UNITY_EDITOR
			if (!SkipMouseRotation)
				euler.y += Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
#endif

			if (SnapRotation)
			{

				if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
				{
					if (ReadyToSnapTurn)
					{
						euler.y -= RotationRatchet;
						ReadyToSnapTurn = false;
					}
				}
				else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
				{
					if (ReadyToSnapTurn)
					{
						euler.y += RotationRatchet;
						ReadyToSnapTurn = false;
					}
				}
				else
				{
					ReadyToSnapTurn = true;
				}
			}
			else
			{
				Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
				euler.y += secondaryAxis.x * rotateInfluence;
			}

			transform.rotation = Quaternion.Euler(euler);
		}
	}


    /// <summary>
    /// OVRCameraRig의 UpdatedAnchors 콜백에 의해 호출됩니다. Hmd 회전이 플레이어의 방향을 업데이트하도록합니다.
    /// </summary>
    public void UpdateTransform(OVRCameraRig rig)
	{
		Transform root = CameraRig.trackingSpace;
		Transform centerEye = CameraRig.centerEyeAnchor;

		if (HmdRotatesY && !Teleported)
		{
			Vector3 prevPos = root.position;
			Quaternion prevRot = root.rotation;

			transform.rotation = Quaternion.Euler(0.0f, centerEye.rotation.eulerAngles.y, 0.0f);

			root.position = prevPos;
			root.rotation = prevRot;
		}

		UpdateController();
		if (TransformUpdated != null)
		{
			TransformUpdated(root);
		}
	}

    /// <summary>
    /// 점프! 수동으로 활성화해야합니다.
    /// </summary>
    public bool Jump()
	{
		if (!Controller.isGrounded)
			return false;

		MoveThrottle += new Vector3(0, transform.lossyScale.y * JumpForce, 0);

		return true;
	}

    /// <summary>
    /// 이 인스턴스를 중지하십시오.
    /// </summary>
    public void Stop()
	{
		Controller.Move(Vector3.zero);
		MoveThrottle = Vector3.zero;
		FallSpeed = 0.0f;
	}

    /// <summary>
    /// 이동 ​​눈금 배율을 가져옵니다.
    /// </summary>
    /// <param name="moveScaleMultiplier">스케일 배율을 이동하십시오.</param>
    public void GetMoveScaleMultiplier(ref float moveScaleMultiplier)
	{
		moveScaleMultiplier = MoveScaleMultiplier;
	}

    /// <summary>
    /// 이동 눈금 배율을 설정합니다.
    /// </summary>
    /// <param name="moveScaleMultiplier">스케일 배율을 이동하십시오. </param>
    public void SetMoveScaleMultiplier(float moveScaleMultiplier)
	{
		MoveScaleMultiplier = moveScaleMultiplier;
	}

    /// <summary>
    /// 회전 눈금 배율을 가져옵니다.
    /// </summary>
    /// <param name="rotationScaleMultiplier">회전 배율 배율.</param>
    public void GetRotationScaleMultiplier(ref float rotationScaleMultiplier)
	{
		rotationScaleMultiplier = RotationScaleMultiplier;
	}

    /// <summary>
    /// 회전 배율을 설정합니다.
    /// </summary>
    /// <param name="rotationScaleMultiplier">회전 배율 배율.</param>
    public void SetRotationScaleMultiplier(float rotationScaleMultiplier)
	{
		RotationScaleMultiplier = rotationScaleMultiplier;
	}

    /// <summary>
    /// 허용 마우스 회전을 가져옵니다.
    /// </summary>
    /// <param name="skipMouseRotation">마우스 회전 허용</param>
    public void GetSkipMouseRotation(ref bool skipMouseRotation)
	{
		skipMouseRotation = SkipMouseRotation;
	}

    /// <summary>
    /// 허용 마우스 회전을 설정합니다.
    /// </summary>
    /// <param name="skipMouseRotation"> true로 설정하면 마우스를 회전 할 수 있습니다.</param>
    public void SetSkipMouseRotation(bool skipMouseRotation)
	{
		SkipMouseRotation = skipMouseRotation;
	}

    /// <summary>
    /// 업데이트 중지 동작을 가져옵니다.
    /// </summary>
    /// <param name="haltUpdateMovement">업데이트 이동을 중지합니다.</param>
    public void GetHaltUpdateMovement(ref bool haltUpdateMovement)
	{
		haltUpdateMovement = HaltUpdateMovement;
	}

    /// <summary>
    /// 업데이트 중지 동작을 설정합니다.
    /// </summary>
    /// <param name="haltUpdateMovement">true로 설정하면 업데이트 이동이 중지됩니다.</param>
    public void SetHaltUpdateMovement(bool haltUpdateMovement)
	{
		HaltUpdateMovement = haltUpdateMovement;
	}

    /// <summary>
    /// 장치 방향이 재설정 될 때 플레이어 모양 회전을 재설정합니다.
    /// </summary>
    public void ResetOrientation()
	{
		if (HmdResetsY && !HmdRotatesY)
		{
			Vector3 euler = transform.rotation.eulerAngles;
			euler.y = InitialYRotation;
			transform.rotation = Quaternion.Euler(euler);
		}
	}
}

    */