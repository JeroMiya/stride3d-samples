using BepuPhysics.Collidables;

namespace MiyaGrace.Stride.Common.ProjectileScripts;

/// <summary>
/// Simple script to remove an entity from the scene
/// when it colides with something. Also handles doing
/// damage to things with HealthComponents and optionally plays a
/// sound when the hit happens. Requires a BodyComponent
/// to be attached to the same entity.
/// </summary>
public class ProjectileDieOnCollide : SyncScript
{
    /// <summary>
    /// If the entity collides with an entity that has a HealthComponent
    /// this script will call DoDamage on that health script with this
    /// damage amount.
    /// </summary>
    public float DamageAmount { get; set; }

    /// <summary>
    /// Optional sound to play when the entity collides.
    /// </summary>
    public Sound? SoundToPlayOnDeath { get; set; }

    /// <summary>
    /// Optional Prefab to spawn when the entity collides
    /// </summary>
    public Prefab? PrefabToSpawnOnDeath { get; set; }

    private BodyComponent mCollider = null!;

    public override void Start()
    {
        mCollider = Entity.Get<BodyComponent>()
            ?? throw new InvalidOperationException("Couldn't find a BodyComponent");
    }

    public override void Update()
    {
        var simulation = mCollider.Simulation;
        if (simulation == null) { return; }

        if(simulation.SweepCast(
            shape: new Box(0.25f, 0.25f, 0.25f),
            pose: new RigidPose(mCollider.Position, mCollider.Orientation),
            velocity: new BodyVelocity(mCollider.LinearVelocity, Vector3.Zero),
            maxDistance: 1.0f,
            out HitInfo result))
        {
            foreach (var collision in mCollider.Collisions)
            {
                var otherEntity = Entity
                    .GetOtherEntityColliderInCollision(collision).Entity;
                var healthComponent = otherEntity.Get<HealthComponent>();
                healthComponent?.DoDamage(DamageAmount);
            }

            PrefabToSpawnOnDeath?.InstantiateInSceneAtEntity(Entity);
            
            if(SoundToPlayOnDeath != null)
            {
                Entity.Play3DSoundAtEntity(SoundToPlayOnDeath);
            }

            Entity.Scene = null;
        }
    }
}
