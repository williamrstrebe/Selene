using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public List<AudioClip> soundtrack;
    private AudioSource source;

    private void Awake()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("GameMusic");
        index = 0;

        if (musicObjs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        source = GetComponent<AudioSource>();
    }

    private float index;
    public void changeMusic()
    {
        this.source.Stop();
        this.source.clip = soundtrack[index == 0 ? 1 : 0];
        this.source.Play();

    }
}
