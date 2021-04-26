using Rhythmify;
using UnityEngine;
using UnityEngine.Audio;

public class BlockController : _AbstractRhythmObject
{
    public Rigidbody2D group;
    public AudioMixer audioMixerOther;
    public AudioSource audioSourceOther;
    private int _counter = 0;

    override protected void init()
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Hit!");
            _counter++;

            if (_counter == 7)
            {
                group.transform.position = Vector2.down * 5;
                audioSourceOther.bypassEffects = false;
                audioMixerOther.SetFloat("Octave", 5.0f);
                audioMixerOther.SetFloat("Pitch", .8f);
                _counter -= 7;
            }
        }
    }

    override protected void rhythmUpdate(int beat)
    {
    }
}