using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[System.Serializable]
public struct EnemyAnimator
{
    public enum Clip { Move, Intro, Outro }
    public Clip CurrentClip { get; private set;}
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

    public void PlayIntro()
    {
        SetWeight(Clip.Intro, 1f);
        CurrentClip = Clip.Intro;
        graph.Play();
    }
    void SetWeight(Clip clip,float weight)
    {
        mixer.SetInputWeight((int)clip, weight);
    }
    public void Stop()
    {
        graph.Stop();
    }

    public void Play(float speed)//удалить после 3
    {
        graph.GetOutput(0).GetSourcePlayable().SetSpeed(speed);
        graph.Play();
    }
    
    public void Destroy()
    {
        graph.Destroy();
    }
    public void PlayMove(float speed)
    {
        SetWeight(CurrentClip, 0f);
        SetWeight(Clip.Move, 1f);
        GetPlayable(Clip.Move).SetSpeed(speed);
        CurrentClip = Clip.Move;
    }

    Playable GetPlayable(Clip clip)
    {
        return mixer.GetInput((int)clip);
    }

    public void PlayOutro()
    {
        SetWeight(CurrentClip, 0f);
        SetWeight(Clip.Outro, 1f);
        CurrentClip = Clip.Outro;
    }
}
