namespace MiyaGrace.Stride.Common.Assets.Scripts.ProjectileScripts;

/// <summary>
/// Very simple movement script that makes the entity move
/// in the direction it was spawned in at a static speed.
/// </summary>
public class ProjectileStaticMovementScript : SyncScript
{
    /// <summary>
    /// The static movement speed of the projectile in world units
    /// per second.
    /// </summary>
    public float MovementSpeed { get; set; } = 50f;

    private BodyComponent body = null!;

    public override void Start()
    {
        body = Entity.Get<BodyComponent>()
            ?? throw new InvalidOperationException("Could not find BodyComponent");
        body.Awake = true;
    }

    public override void Update()
    {
        Entity.Transform.UpdateWorldMatrix();
        var movement = Entity.GetModelWorldForward();
        movement.Normalize();
        movement *= MovementSpeed;

        body.LinearVelocity = movement;
    }
}
