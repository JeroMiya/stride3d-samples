using Stride.BepuPhysics.Definitions.Contacts;

namespace MiyaGrace.Stride.Common.Assets.Scripts.ProjectileScripts;

/// <summary>
/// Simple script to remove an entity from the scene
/// when it colides with something. Also handles doing
/// damage to things with HealthComponents and optionally plays a
/// sound when the hit happens. Requires a BodyComponent
/// to be attached to the same entity.
/// </summary>
public class ProjectileDieOnCollide : SyncScript, IContactHandler
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

    void IContactHandler.OnStartedTouching<TManifold>(Contacts<TManifold> contacts)
    {
        var healthComponent = contacts.Other.Entity.Get<HealthComponent>();
        healthComponent?.DoDamage(DamageAmount);

        PrefabToSpawnOnDeath?.InstantiateInSceneAtEntity(Entity);

        if (SoundToPlayOnDeath != null)
        {
            Entity.Play3DSoundAtEntity(SoundToPlayOnDeath);
        }

        Entity.Scene = null;
    }

    public override void Update() { }
}
