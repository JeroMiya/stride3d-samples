namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script plays a sound on Start of an entity. Typically,
/// this is an entity that has been added after the scene has been
/// loaded. This script requires an AudioEmitterComponent and an
/// AudioEmitterSoundController on the same entity, which will be
/// used to play the sound. 
/// </summary>
public class PlaySoundOnStartup : SyncScript
{
    /// <summary>
    /// The key of the sound controller to use to play the sound. Must
    /// be present in the AudioEmitterComponent's controller list.
    /// </summary>
    public string SoundControllerKey { get; set; } = null!;

    /// <summary>
    /// Defaults to false - set to true for this script to remove the Entity
    /// from the scene once the sound is finished playing.
    /// </summary>
    public bool RemoveEntityOnSoundFinished { get; set; } = false;

    /// <summary>
    /// Defaults to false - set to true to remove the parent Entity from
    /// the scene instead of the attached entity. Requires that the entity
    /// have a parent entity at the time the sound is finished playing.
    /// </summary>
    public bool RemoveParentEntity { get; set; } = false;

    private AudioEmitterComponent mAudioEmitter = null!;
    private AudioEmitterSoundController mSoundController = null!;


    public override void Start()
    {
        if(string.IsNullOrEmpty(SoundControllerKey))
        {
            throw new InvalidOperationException("SoundControllerKey is required");
        }

        mAudioEmitter = Entity.Get<AudioEmitterComponent>()
            ?? throw new InvalidOperationException("Could not find AudioEmitterComponent");
        mSoundController = mAudioEmitter[SoundControllerKey]
            ?? throw new InvalidOperationException($"Could not find sound controller with key {SoundControllerKey}");

        mSoundController.Play();
    }

    public override void Update()
    {
        if (!RemoveEntityOnSoundFinished) { return; }
        if(mSoundController.PlayState != PlayState.Playing)
        {
            if(RemoveParentEntity)
            {
                if(Entity.GetParent() != null)
                {
                    Entity.GetParent().Scene = null;
                }
                else
                {
                    Entity.Scene = null;
                }
            }
        }
    }
}
