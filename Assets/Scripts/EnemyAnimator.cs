using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[System.Serializable]
public struct EnemyAnimator
{
    public enum Clip { Move, Intro, Outro }
    AnimationMixerPlayable mixer;
    PlayableGraph graph;
    public void Configure(Animator animator, EnemyAnimationConfig config)
    {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        mixer = AnimationMixerPlayable.Create(graph, 3);
        var clip = AnimationClipPlayable.Create(graph, config.Move);
        mixer.ConnectInput((int)Clip.Move, clip, 0);
        clip = AnimationClipPlayable.Create(graph, config.Intro);
        clip = AnimationClipPlayable.Create(graph, config.Intro);
        mixer.ConnectInput((int)Clip.Intro, clip, 0);
        clip = AnimationClipPlayable.Create(graph, config.Outro);
        mixer.ConnectInput((int)Clip.Outro, clip, 0);
        var output = AnimationPlayableOutput.Create(graph, "Enemy", animator);
        output.SetSourcePlayable(mixer);
    }

    public void Play(float speed)
    {
        graph.GetOutput(0).GetSourcePlayable().SetSpeed(speed);
        graph.Play();
    }
    public void Stop()
    {
        graph.Stop();
    }

    public void Destroy()
    {
        graph.Destroy();
    }
}
