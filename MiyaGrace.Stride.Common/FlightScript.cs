namespace MiyaGrace.Stride.Common;

/// <summary>
/// Very broken and cursed first attempt at a flight script
/// while learning the engine. Does not work - don't use.
/// </summary>
public class FlightScript : SyncScript
{
    const float GamepadDeadzone = 0.1f;

    /// <summary>
    /// How much the throttle ramps up or down.
    /// </summary>
    public float ThrottleIncrement { get; set; } = 0.1f;

    /// <summary>
    /// Maximum Engine Thrust when at 100% throttle
    /// </summary>
    public float MaxThrust { get; set; } = 1000f;

    /// <summary>
    /// How responsive the plane is with rolling, pitching and yawing
    /// </summary>
    public float Responsiveness = 10f;

    public float Lift { get; set; } = 135f;

    private RigidbodyComponent mRigidBody = null!;

    private float mThrottle;
    private float mRoll;
    private float mPitch;
    private float mYaw;

    private float ResponseModifier
    {
        get
        {
            return mRigidBody.Mass / 10f * Responsiveness;
        }
    }

    public override void Start()
    {
        base.Start();
        mRigidBody = Entity.Get<RigidbodyComponent>()
            ?? throw new InvalidOperationException("Could not find RigidBodyComponent"); ;

    }

    public override void Update()
    {
        if (Input.HasGamePad)
        {
            HandleInputs(Input.DefaultGamePad.State);

            mRigidBody.ApplyForce(GetForward() * MaxThrust * mThrottle);
            mRigidBody.ApplyTorque(GetUp() * mYaw * ResponseModifier);
            mRigidBody.ApplyTorque(GetRight() * mPitch * ResponseModifier);
            mRigidBody.ApplyTorque(GetBackward() * mRoll * ResponseModifier);
            mRigidBody.ApplyForce(Vector3.UnitY * mRigidBody.LinearVelocity.Length() * Lift);
        }
    }

    private void HandleInputs(GamePadState gamePadState)
    {
        // Set rotational values from our axis inputs.
        if (gamePadState.LeftThumb.Length() > GamepadDeadzone)
        {
            mRoll = -gamePadState.LeftThumb.X;
            mPitch = -gamePadState.LeftThumb.Y;
        }
        if (MathF.Abs(gamePadState.RightThumb.X) > GamepadDeadzone)
        {
            mYaw = -gamePadState.RightThumb.X;
        }

        // Handle throttle value being sure to clamp it between 0 and 100

        if (Input.HasGamePad && gamePadState.RightTrigger > GamepadDeadzone)
        {
            mThrottle = gamePadState.RightTrigger;
        }

        if (mThrottle < 0.0f) mThrottle = 0f;
        if (mThrottle > 1f) mThrottle = 1f;
    }

    private Vector3 GetForward()
    {
        var rotation = Entity.Transform.Rotation;
        var x = rotation.X;
        var y = rotation.Y;
        var z = rotation.Z;
        var w = rotation.W;

        var forward = new Vector3(
            2 * (x * z + w * y),
            2 * (y * z - w * x),
            1 - 2 * (x * x + y * y));
        forward.Normalize();
        return forward;
    }

    private Vector3 GetBackward() => -GetForward();

    private Vector3 GetUp()
    {
        var rotation = Entity.Transform.Rotation;
        var x = rotation.X;
        var y = rotation.Y;
        var z = rotation.Z;
        var w = rotation.W;

        var up = new Vector3(
            2 * (x * y - w * z),
            1 - 2 * (x * x + z * z),
            2 * (y * z + w * x));

        up.Normalize();
        return up;
    }

    private Vector3 GetDown() => -GetUp();

    private Vector3 GetLeft()
    {
        var rotation = Entity.Transform.Rotation;
        var x = rotation.X;
        var y = rotation.Y;
        var z = rotation.Z;
        var w = rotation.W;

        var left = new Vector3(
            1 - 2 * (y * y + z * z),
            2 * (x * y + w * z),
            2 * (x * z - w * y));

        left.Normalize();
        return left;
    }

    private Vector3 GetRight() => -GetLeft();
}
