using BepuPhysics.Collidables;

using Stride.BepuPhysics.Definitions.Contacts;

namespace MiyaGrace.Stride.Common.ProjectileScripts;

/// <summary>
/// Simple script to remove an entity from the scene
/// when it colides with something. Also handles doing
/// damage to things with HealthComponents and optionally plays a
/// sound when the hit happens. Requires a BodyComponent
/// to be attached to the same entity.
/// </summary>
public class ProjectileDieOnCollide : SyncScript, IContactEventHandler
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

    public bool NoContactResponse => true;

    public override void Update()
    {
        
    }

    void IContactEventHandler.OnStartedTouching<TManifold>(
        CollidableComponent eventSource,
        CollidableComponent other,
        ref TManifold contactManifold,
        bool flippedManifold,
        int workerIndex,
        BepuSimulation bepuSimulation)
    {
        // When something enters inside this object
        var healthComponent = other.Entity.Get<HealthComponent>();
        healthComponent?.DoDamage(DamageAmount);

        PrefabToSpawnOnDeath?.InstantiateInSceneAtEntity(Entity);

        if (SoundToPlayOnDeath != null)
        {
            Entity.Play3DSoundAtEntity(SoundToPlayOnDeath);
        }

        Entity.Scene = null;
    }
}
