using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    // Use this for initialization
    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }


    void PlayCutscene()
    {
        timeline.Play();
    }

    void StopCutscene()
    {
        timeline.Stop();
    }
}