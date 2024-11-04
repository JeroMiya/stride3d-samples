namespace MiyaGrace.Stride.Common.ProjectileScripts;

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

    public override void Update()
    {
        Entity.Transform.UpdateWorldMatrix();
        var movement = Entity.GetModelWorldForward();
        movement.Normalize();
        movement *= MovementSpeed * (float)Game.UpdateTime.Elapsed.TotalSeconds;


        var parent = Entity.GetParent();
        if (parent == null)
        {
            Entity.Transform.Position += movement;
        }
        else
        {
            var inverseParentWorldRotation = parent.GetWorldRotation();
            inverseParentWorldRotation.Invert();
            inverseParentWorldRotation.Normalize();

            movement = inverseParentWorldRotation * movement;

            Entity.Transform.Position += movement;
        }
    }
}
