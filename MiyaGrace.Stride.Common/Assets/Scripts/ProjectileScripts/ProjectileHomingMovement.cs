namespace MiyaGrace.Stride.Common.Assets.Scripts.ProjectileScripts;

/// <summary>
/// Simple homing projectile movement script. Attempts to smoothly
/// rotate an entity towards a target each update.
/// </summary>
public class ProjectileHomingMovement : SyncScript
{

    /// <summary>
    /// The target entity to attempt to follow
    /// </summary>
    public Entity? Target { get; set; }

    /// <summary>
    /// The turn speed in terms of amount of the difference between
    /// the current and target rotations, from 0.0f to 1.0f, with 1.0f
    /// being always looking directly at the target every update.
    /// </summary>
    public float TurnSpeed { get; set; } = 0.5f;

    /// <summary>
    /// Projectile movement speed in world units per second
    /// </summary>
    public float MovementSpeed { get; set; } = 0.1f;

    private BodyComponent body = null!;

    public override void Start()
    {
        if(Target == null)
        {
            Game.Services.GetServiceLate<PlayerEntityService>(s => Target = s.PlayerEntity);
        }

        body = Entity.Get<BodyComponent>()
            ?? throw new InvalidOperationException("Could not find BodyComponent");
    }

    public override void Update()
    {
        // Target not yet ready, do nothing
        if (Target == null) { return; }

        var currentForward = Quaternion.LookRotation(Entity.GetModelWorldForward(), Vector3.UnitY);
        var desiredForward = Quaternion.LookRotation(Target.GetWorldPosition() - Entity.GetWorldPosition(), Vector3.UnitY);
        if(Quaternion.AngleBetween(currentForward, desiredForward) < MathUtil.DegreesToRadians(90))
        {
            desiredForward.Normalize();
            var lerpedForward = Quaternion.Lerp(currentForward, desiredForward, TurnSpeed);
            lerpedForward.Normalize();

            Entity.SetWorldRotation(lerpedForward);
            Entity.Transform.UpdateWorldMatrix();
        }

        var movement = Entity.GetModelWorldForward();
        movement.Normalize();
        movement *= MovementSpeed;

        body.LinearVelocity = movement;
    }
}
