namespace MiyaGrace.Stride.Common.ProjectileScripts;

/// <summary>
/// Simple script to remove the projectile entity from
/// the scene a certain amount of time after it has been
/// added to the scene. Optionally spawns a prefab when
/// the projectile is despawned.
/// </summary>
public class ProjectileLifetimeScript : SyncScript
{
    /// <summary>
    /// Amount of time to allow the entity to live.
    /// </summary>
    public double LifetimeSeconds { get; set; } = 10.0;

    /// <summary>
    /// Optional Prefab to spawn at the entity's location
    /// after death.
    /// </summary>
    public Prefab? PrefabToSpawnOnDeath { get; set; }


    private double mStartTime = 0;

    public override void Start()
    {
        mStartTime = Game.UpdateTime.Total.TotalSeconds;
    }

    public override void Update()
    {
        if (Game.UpdateTime.Total.TotalSeconds - mStartTime > LifetimeSeconds)
        {
            PrefabToSpawnOnDeath?.InstantiateInSceneAtEntity(Entity);
            Entity.Scene = null;
        }
    }
}
