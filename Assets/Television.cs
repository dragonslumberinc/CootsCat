using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : LaserPainting
{
    public Renderer renderer;

    public Texture2D[] frames;

    public float framesPerSecond = 10f;

    public bool active = false;

    public AudioSource audioSource;

    private void Awake()
    {
        renderer.gameObject.SetActive(false);
    }

    public override void laserHit(Vector3 pos, Vector3 normalUp)
    {
        if (!active)
        {
            renderer.gameObject.SetActive(true);
            active = true;
            GameController.Instance.spawner.spawnAt("Madness", this.transform.position, normalUp, GameController.Instance.transform);
        }

        if(!audioSource.isPlaying)
            audioSource.Play();
    }

    void Update()
    {
        if (active)
        {
            int index = Mathf.FloorToInt(Time.fixedTime * framesPerSecond) % frames.Length;
            renderer.material.mainTexture = frames[index];
        }
    }
}
