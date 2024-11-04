namespace MiyaGrace.Stride.Common;

/// <summary>
/// First person camera controller, based on the Stride
/// tutorials. Placeholder until something better can be
/// implemented.
/// </summary>
public class FirstPersonCamera : SyncScript
{
    public float MouseSpeed = 0.6f;
    public float MaxLookUpAngle = -50;
    public float MaxLookDownAngle = 50;
    public bool InvertMouseY = false;

    private Entity firstPersonCameraPivot = null!;
    private Vector3 camRotation;
    private Vector2 maxCameraAnglesRadians;
    private CharacterComponent character = null!;

    public override void Start()
    {
        firstPersonCameraPivot = Entity.FindChild("CameraPivot")
            ?? throw new InvalidOperationException("Couldn't find child entity CameraPivot");

        // Convert the Max camera angles from Degress to Radions
        maxCameraAnglesRadians = new Vector2(MathUtil.DegreesToRadians(MaxLookUpAngle), MathUtil.DegreesToRadians(MaxLookDownAngle));

        // Store the initial camera rotation
        camRotation = Entity.Transform.RotationEulerXYZ;

        character = Entity.Get<CharacterComponent>()
            ?? throw new InvalidOperationException("Couldn't find CharacterComponent on attached entity");
    }


    public override void Update()
    {
        var mouseMovement = Input.MouseDelta * MouseSpeed;

        // Update camera rotation values
        camRotation.Y += -mouseMovement.X;
        camRotation.X += InvertMouseY ? -mouseMovement.Y : mouseMovement.Y;
        camRotation.X = MathUtil.Clamp(camRotation.X, maxCameraAnglesRadians.X, maxCameraAnglesRadians.Y);

        // Apply Y rotation to character entity
        character.Orientation = Quaternion.RotationY(camRotation.Y);

        // Apply X camera rotation to the existing camera rotations
        firstPersonCameraPivot.Transform.Rotation = Quaternion.RotationX(camRotation.X);
    }
}
