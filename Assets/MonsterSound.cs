using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    private Renderer monsterRenderer;
    private AudioSource monsterAudio;

    void Start()
    {
        monsterRenderer = GetComponent<Renderer>();
        monsterAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (monsterRenderer.isVisible && !monsterAudio.isPlaying)
        {
            // Phát âm thanh khi quái nằm trong khung hình và âm thanh chưa được phát
            monsterAudio.Play();
        }
        else if (!monsterRenderer.isVisible && monsterAudio.isPlaying)
        {
            // Ngừng phát âm thanh khi quái ra khỏi khung hình
            monsterAudio.Stop();
        }
    }
}
