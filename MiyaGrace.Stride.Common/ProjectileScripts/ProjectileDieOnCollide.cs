namespace MiyaGrace.Stride.Common.ProjectileScripts;

/// <summary>
/// Simple script to remove an entity from the scene
/// when it colides with something. Also handles doing
/// damage to things with HealthComponents and optionally plays a
/// sound when the hit happens. Requires a RigidbodyComponent
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

    private RigidbodyComponent mCollider = null!;

    public override void Start()
    {
        mCollider = Entity.Get<RigidbodyComponent>()
            ?? throw new InvalidOperationException("Couldn't find a RigidbodyComponent");
    }

    public override void Update()
    {
        if (mCollider.Collisions.Count > 0)
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
