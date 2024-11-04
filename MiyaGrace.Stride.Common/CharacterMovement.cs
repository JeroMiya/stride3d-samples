namespace MiyaGrace.Stride.Common;

/// <summary>
/// Basic first person character movement script from the
/// Stride tutorials. Just a placeholder until something better
/// can be written.
/// Review note: Why is this separate from FirstPersonCamera script?
/// Could we combine them into one script? Should we?
/// </summary>
public class CharacterMovement : SyncScript
{
    /// <summary>
    /// The movement multiplier to use by default, in camera space.
    /// Controls how fast the player moves by default.
    /// </summary>
    public Vector3 MovementMultiplier = new Vector3(3, 0, 4);

    /// <summary>
    /// 
    /// </summary>
    public float RunMultiplier = 1.5f;

    private CharacterComponent character = null!;

    public override void Start()
    {
        character = Entity.Get<CharacterComponent>()
            ?? throw new InvalidOperationException("Couldn't find CharacterComponent on entity");
    }

    public override void Update()
    {
        var velocity = new Vector3();
        if (Input.IsKeyDown(Keys.W))
        {
            velocity.Z++;
        }
        if (Input.IsKeyDown(Keys.S))
        {
            velocity.Z--;
        }

        if (Input.IsKeyDown(Keys.A))
        {
            velocity.X++;
        }
        if (Input.IsKeyDown(Keys.D))
        {
            velocity.X--;
        }

        velocity.Normalize();
        velocity *= MovementMultiplier;
        if (Input.IsKeyDown(Keys.LeftShift))
        {
            velocity *= RunMultiplier;
        }
        velocity = Vector3.Transform(velocity, Entity.Transform.Rotation);
        character.SetVelocity(velocity);
    }
}
