namespace MiyaGrace.Stride.Common.Extensions;

public static class TransformExtensions
{
    /// <summary>
    /// Gets world rotation from a transform. This may
    /// already have a similar method in the community toolkit,
    /// so this version may be removed in the future.
    /// </summary>
    public static Quaternion GetWorldRotation(this TransformComponent transform)
    {
        transform.WorldMatrix.Decompose(out _, out Quaternion rotation, out _);
        return rotation;
    }

    /// <summary>
    /// Get the world-forward vector of a transform in world-model space.
    /// </summary>
    public static Vector3 GetModelWorldForward(this TransformComponent transform)
    {
        return -transform.WorldMatrix.TranslationVector;
    }

    /// <summary>
    /// Get model world-forward rotation of a transform in world-model space.
    /// </summary>
    public static Quaternion GetModelWorldForwardRotation(this TransformComponent transform)
    {
        var modelForward = transform.GetModelWorldForward();
        var modelUp = transform.WorldMatrix.Up;
        return Quaternion.LookRotation(modelForward, modelUp);
    }
}
