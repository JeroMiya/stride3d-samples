namespace MiyaGrace.Stride.Common.Extensions;

public static class PrefabExtensions
{
    /// <summary>
    /// Instatiates the specified Prefab and adds it
    /// to the scene under a new root Entity. It then
    /// adds the root entity to the Scene of the Entity
    /// you pass in, at the world position and rotation
    /// of the Entity.
    /// </summary>
    public static Entity InstantiateInSceneAtEntityWrapped(this Prefab prefab, Entity entity)
    {
        var worldPosition = entity.GetWorldPosition();
        var worldRotation = entity.GetWorldRotation();

        var newEntity = new Entity(
            name: null,
            position: worldPosition,
            rotation: worldRotation);

        var entities = prefab.Instantiate();
        newEntity.AddChildRange(entities);
        entity.Scene.Entities.Add(newEntity);
        return newEntity;
    }

    /// <summary>
    /// Instatiates the specified Prefab and adds all of
    /// its entities directly into the current scene,
    /// at the world position and rotation of the given entity.
    /// </summary>
    public static void InstantiateInSceneAtEntity(this Prefab prefab, Entity entity)
    {
        var worldPosition = entity.GetWorldPosition();
        var worldRotation = entity.GetWorldRotation();

        var entities = prefab.Instantiate();
        foreach(var prefabEntity in entities)
        {
            var localPosition = prefabEntity.Transform.Position;
            var localRotation = prefabEntity.Transform.Rotation;
            prefabEntity.Transform.Position = worldPosition + localPosition;
            prefabEntity.Transform.Rotation = worldRotation * localRotation;
            entity.Scene.Entities.Add(prefabEntity);
        }
    }

    /// <summary>
    /// Instantiates the Prefab as children of the specified Entity
    /// </summary>
    public static void InstantiateInEntity(this Prefab prefab, Entity entity)
    {
        var entities = prefab.Instantiate();
        entity.AddChildRange(entities);
    }

    /// <summary>
    /// Instantiate the Prefab as children of the specified Scene with
    /// no additional transformations
    /// </summary>
    public static void InstantiateInScene(this Prefab prefab, Scene scene)
    {
        var entities = prefab.Instantiate();
        scene.Entities.AddRange(entities);
    }

    /// <summary>
    /// Add all Entities in children as children of the specified Entity
    /// </summary>
    public static void AddChildRange(this Entity entity, IList<Entity> children)
    {
        for (int i = 0; i < children.Count; i++)
        {
            entity.AddChild(children[i]);
        }
    }
}
