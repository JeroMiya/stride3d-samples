namespace MiyaGrace.Stride.Common.Services;

public class PlayerEntityService
{
    public Entity PlayerEntity { get; set; }
    public Entity CameraPivotEntity { get; set; }

    public PlayerEntityService(Entity playerEntity, Entity cameraPivot)
    {
        PlayerEntity = playerEntity
            ?? throw new ArgumentNullException(nameof(playerEntity));
        CameraPivotEntity = cameraPivot
            ?? throw new ArgumentNullException(nameof(cameraPivot));
    }
}
