namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script requires a HealthComponent to also be
/// attached to the same entity. When the entity's health
/// reaches zero, this script will remove the entity from
/// the scene. Optionally, it will play a sound on death
/// and also optionally spawn a prefab at the entity's location
/// on death.
/// </summary>
public class RemoveEntityOnDeath : SyncScript
{
    /// <summary>
    /// Optional property - if set, will play the given sound when the
    /// HealthComponent reports that the entity has died.
    /// </summary>
    public Sound? SoundToPlayOnDeath { get; set; }

    /// <summary>
    /// Optional property - if set, will spawn the given Prefab when the
    /// HealthComponent reports that the entity has died.
    /// </summary>
    public Prefab? PrefabToSpawnOnDeath { get; set; }


    HealthComponent? mHealthComponent;

    bool mMarkedForRemoval = false;

    public override void Start()
    {
        mHealthComponent = Entity.Get<HealthComponent>();
        mHealthComponent.Died += OnDeath;
    }

    private void OnDeath(object? sender, EventArgs e)
    {
        // NOTE: there's probably a more idiomatic way to schedule
        // entity removal in Stride.
        mMarkedForRemoval = true;
    }

    public override void Update()
    {
        if(mMarkedForRemoval)
        {
            Entity.Play3DSoundAtEntity(SoundToPlayOnDeath);
            if (mHealthComponent != null)
            {
                mHealthComponent.Died -= OnDeath;
                mHealthComponent = null;
            }

            PrefabToSpawnOnDeath?.InstantiateInSceneAtEntity(Entity);
            Entity.Scene = null;
        }
    }
}
