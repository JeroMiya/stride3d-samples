namespace MiyaGrace.Stride.Common.Extensions;

public static class EntityExtensions
{
    /// <summary>
    /// Get the world rotation from an entity's transform. I think there's
    /// already a community kit method for this, so this may go away.
    /// </summary>
    public static Quaternion GetWorldRotation(this Entity entity)
    {
        entity.Transform.WorldMatrix.Decompose(out _, out Quaternion rotation, out _);
        return rotation;
    }

    /// <summary>
    /// Convenience method for getting world forward from an entity
    /// - there may be a community kit method already for this, so
    /// this may go away.
    /// </summary>
    public static Vector3 GetWorldForward(this Entity entity)
    {
        return entity.Transform.WorldMatrix.Forward;
    }

    /// <summary>
    /// Gets the world rotation of an entity in model-world space.
    /// </summary>
    public static Quaternion GetModelWorldRotation(this Entity entity)
    {
        var ret = Quaternion.LookRotation(
            entity.GetModelWorldForward(),
            entity.Transform.WorldMatrix.Up);

        return ret;
    }

    /// <summary>
    /// Gets the world position of an entity. There may be
    /// a community kit method for this already, so this method
    /// may go away.
    /// </summary>
    public static Vector3 GetWorldPosition(this Entity entity)
    {
        return entity.Transform.WorldMatrix.TranslationVector;
    }

    /// <summary>
    /// Sets the world rotation of an entity to the desired world rotation.
    /// </summary>
    public static void SetWorldRotation(this Entity entity, Quaternion desiredWorldRotation)
    {
        var parentEntity = entity.GetParent();
        if (parentEntity == null)
        {
            entity.Transform.Rotation = desiredWorldRotation;
        }
        else
        {
            var inverseParentWorldRotation = parentEntity.GetWorldRotation();
            inverseParentWorldRotation.Invert();
            inverseParentWorldRotation.Normalize();

            var newLocalRotation = inverseParentWorldRotation * desiredWorldRotation;
            newLocalRotation.Normalize();
            entity.Transform.Rotation = newLocalRotation;
            entity.Transform.UpdateWorldMatrix();
        }
    }

    /// <summary>
    /// Get world forward for an entity in model-world space (+Z forward).
    /// </summary>
    public static Vector3 GetModelWorldForward(this Entity entity)
    {
        return -entity.Transform.WorldMatrix.Forward;
    }

    /// <summary>
    /// Make an entity "look" at another entity in model-world space, using
    /// +Y as the "up" vector.
    /// </summary>
    public static void ModelLookAt(this Entity entity, Entity targetEntity)
    {
        entity.ModelLookAt(targetEntity, Vector3.UnitY);
    }

    /// <summary>
    /// Make an entity "look" at another entity in model-world space, using
    /// the given up vector.
    /// </summary>
    public static void ModelLookAt(this Entity entity, Entity targetEntity, Vector3 up)
    {
        var newForward = entity.GetWorldForwardTowards(targetEntity);
        entity.ModelLookAtWorld(newForward, up);
    }

    /// <summary>
    /// Make an entity look in the direction of worldForward in model-world space,
    /// using the given up vector.
    /// </summary>
    public static void ModelLookAtWorld(this Entity entity, Vector3 worldForward, Vector3 up)
    {
        var desiredWorldRotation = Quaternion.LookRotation(worldForward, up);
        entity.SetWorldRotation(desiredWorldRotation);
    }

    /// <summary>
    /// Get a world forward vector pointing from entity to targetEntity.
    /// </summary>
    public static Vector3 GetWorldForwardTowards(this Entity entity, Entity targetEntity)
    {
        var target = targetEntity.GetWorldPosition();
        var source = entity.GetWorldPosition();
        var newForward = target - source;
        newForward.Normalize();
        return newForward;
    }

    /// <summary>
    /// Get a world rotation pointing from entity to targetEntity,
    /// using the given up vector, in model-world space.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="targetEntity"></param>
    /// <param name="up"></param>
    /// <returns></returns>
    public static Quaternion GetModelWorldRotationTowards(this Entity entity, Entity targetEntity, Vector3 up)
    {
        var desiredWorldForward = entity.GetWorldForwardTowards(targetEntity);
        return Quaternion.LookRotation(-desiredWorldForward, up);
    }

    /// <summary>
    /// Get a world rotation pointing from entity to targetEntity,
    /// using +Y as the up vector, in model-world space.
    /// </summary>
    public static Quaternion GetModelWorldRotationTowards(this Entity entity, Entity targetEntity)
    {
        return entity.GetModelWorldRotationTowards(targetEntity, Vector3.UnitY);
    }

    /// <summary>
    /// WARNING: untested, may be incorrect - do not use
    /// </summary>
    public static Vector3 GetWorldVectorFromRelative(this Entity entity, Vector3 local)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var worldTransform = entity.GetWorldRotation();
        worldTransform.Rotate(ref local);
        return local;
    }

    /// <summary>
    /// Get the entity in the Collision that is not the target entity.
    /// </summary>
    public static PhysicsComponent GetOtherEntityColliderInCollision(this Entity entity, Collision collision)
    {
        return Object.ReferenceEquals(entity, collision.ColliderA.Entity)
            ? collision.ColliderB : collision.ColliderA;
    }

    /// <summary>
    /// Play a 3D sound in the position of an entity
    /// </summary>
    /// <param name="entity">The entity whose transform affects 
    ///   where in 3D space the sound will be played</param>
    /// <param name="sound">The sound to play</param>
    /// <param name="applyEntityVelocity">Apply the target entity's velocity
    /// to the sound.</param>
    public static void Play3DSoundAtEntity(this Entity entity, Sound? sound, bool applyEntityVelocity = false)
    {
        if (sound is null) { return; }
        AudioEmitter audioEmitter = new();
        audioEmitter.Position = entity.Transform.WorldMatrix.TranslationVector;
        if(applyEntityVelocity)
        {
            var rigidBody = entity.Get<RigidbodyComponent>();
            if(rigidBody != null)
            {
                audioEmitter.Velocity = rigidBody.LinearVelocity;
            }
        }

        var instance = sound.CreateInstance(
            listener: null,
            useHrtf: false,
            directionalFactor: 0,
            environment: HrtfEnvironment.Outdoors);        
        instance.Apply3D(audioEmitter);
        instance.Play();
    }
}
