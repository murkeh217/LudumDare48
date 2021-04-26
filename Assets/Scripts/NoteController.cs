/* @Author: Gkxd
 * 
 * */

using System;
using UnityEngine;
using System.Collections;
using Rhythmify;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class ColorTransition
{
    public Color startColor;
    public Color endColor;
    public bool smooth;
}


public class NoteController : _AbstractRhythmObject
{
    public Vector2[] positions;
    public int[] indicesP;
    public int offsetP;
    public bool local;
    public bool relative;
    public bool rigid;

    private Vector3 startPosition;
    private Rigidbody2D rigidBody;

    public ColorTransition[] colorTransitions;
    public int[] indicesC;
    public int offsetC;
    public bool shared;
    public Renderer renderer;




    override protected void init()
    {
        if (relative)
        {
            if (local)
            {
                startPosition = gameObject.transform.localPosition;
            }
            else
            {
                startPosition = gameObject.transform.position;
            }
        }
        else
        {
            startPosition = Vector3.zero;
        }

        if (rigid)
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody == null)
            {
                Debug.LogError("The GameObject " + gameObject + " has no RigidBody component attached!");
                Debug.Break();
            }
        }
    }

    override protected void rhythmUpdate(int beat)
    {
        int sizeP = positions.Length;

        if (sizeP <= 1)
        {
            return;
        }

        int idxP = beat + offsetP;
        if (indicesP.Length > 0)
        {
            int idxA = indicesP[idxP % indicesP.Length];
            int idxB = indicesP[(idxP + 1) % indicesP.Length];
            StartCoroutine(move(positions[idxA % sizeP], positions[idxB % sizeP], secondsPerBeat));
        }
        else
        {
            StartCoroutine(move(positions[idxP % sizeP], positions[(idxP + 1) % sizeP], secondsPerBeat));
        }

        int sizeC = colorTransitions.Length;

        if (sizeC < 0)
        {
            return;
        }

        int idxC = beat + offsetC;
        if (indicesC.Length > 0)
        {
            idxC = indicesC[idxC % indicesC.Length];
        }

        if (idxC < 0)
        {
            return;
        }

        ColorTransition t = colorTransitions[idxC % sizeC];

        if (t.smooth)
        {
            StartCoroutine(changeSmooth(t.startColor, t.endColor, secondsPerBeat));
        }
        else
        {
            if (shared)
            {
                renderer.sharedMaterial.color = t.endColor;
            }
            else
            {
                renderer.material.color = t.endColor;
            }
        }
    }

    private IEnumerator move(Vector3 startPos, Vector3 endPos, float duration)
    {
        float startTime = Time.time;
        if (rigid && rigidBody != null)
        {
            while (Time.time <= startTime + duration)
            {
                float slerpPercent = Mathf.Clamp01((Time.time - startTime) / duration);
                rigidBody.MovePosition(Vector3.Slerp(startPos, endPos, slerpPercent) + startPosition);
                yield return null;
            }
        }
        else if (local)
        {
            while (Time.time <= startTime + duration)
            {
                float slerpPercent = Mathf.Clamp01((Time.time - startTime) / duration);
                transform.localPosition = Vector3.Slerp(startPos, endPos, slerpPercent);
                transform.localPosition += startPosition;
                yield return null;
            }
        }
        else
        {
            while (Time.time <= startTime + duration)
            {
                float slerpPercent = Mathf.Clamp01((Time.time - startTime) / duration);
                transform.position = Vector3.Slerp(startPos, endPos, slerpPercent);
                transform.position += startPosition;
                yield return null;
            }
        }
    }

    private IEnumerator changeSmooth(Color startColor, Color endColor, float duration)
    {
        float startTime = Time.time;
        while (Time.time <= startTime + duration)
        {
            float lerpPercent = Mathf.Clamp01((Time.time - startTime) / duration);
            if (shared)
            {
                renderer.sharedMaterial.color = Color.Lerp(startColor, endColor, lerpPercent);
            }
            else
            {
                renderer.material.color = Color.Lerp(startColor, endColor, lerpPercent);
            }

            yield return null;
        }
    }


//----------------------------------------------------------------------------------------------------------------------
    
}