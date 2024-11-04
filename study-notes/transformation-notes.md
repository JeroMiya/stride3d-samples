Model space (by convention):
 * Forward: +Z axis
 * Right: -X axis
 * Up: +Y axis

What is the default local and world directions with a top level entity with zeroed out transformation?


 * World:
   * Forward: -Z axis
   * Right: +X axis
   * Up: +Y axis

 * Local:
   * Forward: -Z axis
   * Right: +X axis
   * Up: +Y axis

Child entity with no transform of its own, child of parent with +90 rotation around Y?
 * World:
   * Forward: -X axis
   * Right: -Z axis
   * Up: +Y axis
 * Local:
   * Forward: -Z axis
   * Right: +X axis
   * Up: +Y axis

Notes: 
 * Local doesn't change, as expected.
 * World rotated 90 degrees around Y axis, as expected.
 * Positive rotation around Y axis results in clockwise rotation when looking towards +Y (counterclockwise when looking towards -Y)

-Z unit vector multiplied by world rotation:
 * Transformed -Z: -X axis

Calculating a new local rotation, given a desired world rotation and optionally a parent transformation:
 * If the entity does not have a parent entity, set rotation directly to the desired world rotation
 * If the entity does have a parent entity:
   * Get the INVERSE of the parent entity world rotation
     * parentEntity.Transform.WorldMatrix.Decompose(out _, out Quaternion inverseParentWorldRotation, out _);
     * inverseParentWorldRotation.Invert();
     * inverseParentWorldRotation.Normalize(); // avoid accumulation of floating point error?
   * var newLocalRotation = inverseParentWorldRotation * desiredWorldRotation;
   * newLocalRotation.Normalize();
   * Entity.Transform.Rotation = newLocalRotation;
   * Entity.Transform.UpdateWorldMatrix();
