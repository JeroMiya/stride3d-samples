using MiyaGrace.Stride.Common.Services;

namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script initializes the PlayerEntityService on scene startup.
/// It sets the PlayerEntity and CameraPivotEntity properties of that
/// service to the properties set in the editor. If the PlayerEntityService
/// is already added when this script runs, it will be replaced.
/// </summary>
public class PlayerEntityStartupScript : StartupScript
{
    /// <summary>
    /// The top level player entity. This is required.
    /// </summary>
    public required Entity PlayerEntity { get; set; }

    /// <summary>
    /// The CameraPivot entity (I was experimenting with targeting
    /// the camera pivot entity instead of the center of the player
    /// entity to make turrets look more like they are targeting where
    /// the player is looking towards. This is required.
    /// </summary>
    public required Entity CameraPivotEntity { get; set; }


    public override void Start()
    {
        if(PlayerEntity is null)
        {
            throw new InvalidOperationException("PlayerEntity was not set in the editor");
        }
        if(CameraPivotEntity is null)
        {
            throw new InvalidOperationException("CameraPivotEntity was not set in editor");
        }

        if(Game.Services.GetService<PlayerEntityService>() != null)
        {
            Game.Services.RemoveService<PlayerEntityService>();
        }
        Game.Services.AddService(new PlayerEntityService(PlayerEntity, CameraPivotEntity));
    }
}
