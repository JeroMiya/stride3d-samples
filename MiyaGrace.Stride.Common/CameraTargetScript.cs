namespace MiyaGrace.Stride.Common;

/// <summary>
/// This is a very basic camera controller that follows one or
/// more camera targets that you can switch between using keys 1-N.
/// </summary>
public class CameraTargetScript : SyncScript
{
    int mTargetIndex = 0;
    Entity CurrentTarget
    {
        get
        {
            if (mTargetIndex >= 0 && mTargetIndex < CameraTargets.Count)
            {
                return CameraTargets[mTargetIndex];
            }
            throw new InvalidOperationException($"mTargetIndex {mTargetIndex} is out of bounds");
        }
    }

    Quaternion mLerpStartRotation = Quaternion.Identity;
    Vector3 mLerpStartPosition = Vector3.Zero;
    TimeSpan mLerpStartTime = TimeSpan.Zero;

    /// <summary>
    /// A reference to the Camera to control.
    /// review note: Why can't we attach this script directly to
    /// the camera?
    /// </summary>
    public required CameraComponent Camera { get; set; }

    /// <summary>
    /// A list of camera targets, represented as Entity references.
    /// When switching to a target, the camera will transition to
    /// the target's position and direction over time.
    /// </summary>
    public List<Entity> CameraTargets = [];

    /// <summary>
    /// Amount of seconds it takes to transition from the camera's current
    /// position and rotation to the target position and rotation after
    /// the player presses the button to switch to a different target.
    /// </summary>
    public float TargetSwitchTransitionTimeSeconds = 0.25f;

    public override void Start()
    {
        if (Camera == null) { throw new InvalidOperationException("Camera property is required"); }
        if (CameraTargets.Count == 0) { throw new InvalidOperationException("At least one camera target is required"); }
        if (TargetSwitchTransitionTimeSeconds < 0.0f) { throw new InvalidOperationException("Invalid TargetSwitchTransitionTimeSeconds. Must be greater than or equal to 0.0f"); }

        SetEntityToTargetPositionAndRotation();
    }

    readonly Keys[] mKeysToTargetMap = [
        Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0
        ];

    public override void Update()
    {
        for (int i = 0; i < CameraTargets.Count && i < mKeysToTargetMap.Length; i++)
        {
            if (mTargetIndex != i && Input.IsKeyPressed(mKeysToTargetMap[i]))
            {
                mTargetIndex = i;
                mLerpStartRotation = Camera.Entity.Transform.Rotation;
                mLerpStartPosition = Camera.Entity.Transform.Position;
                mLerpStartTime = Game.UpdateTime.Total;
            }
        }

        var currentTotalTime = Game.UpdateTime.Total.TotalSeconds;
        var startTotalTime = mLerpStartTime.TotalSeconds;
        var elapsedTime = currentTotalTime - startTotalTime;
        if (TargetSwitchTransitionTimeSeconds > 0.0f && elapsedTime >= 0 && elapsedTime < TargetSwitchTransitionTimeSeconds)
        {
            var currentTarget = CurrentTarget;

            var lerpAmount = (float)(elapsedTime / TargetSwitchTransitionTimeSeconds);
            var positionLerp = Vector3.Lerp(mLerpStartPosition, currentTarget.Transform.Position, lerpAmount);
            var rotationLerp = Quaternion.Lerp(mLerpStartRotation, currentTarget.Transform.Rotation, lerpAmount);
            Camera.Entity.Transform.Position = positionLerp;
            Camera.Entity.Transform.Rotation = rotationLerp;
        }
        else
        {
            SetEntityToTargetPositionAndRotation();
        }
    }

    private void SetEntityToTargetPositionAndRotation()
    {
        var currentTarget = CurrentTarget;

        Camera.Entity.Transform.Position = currentTarget.Transform.Position;
        Camera.Entity.Transform.Rotation = currentTarget.Transform.Rotation;
    }
}
