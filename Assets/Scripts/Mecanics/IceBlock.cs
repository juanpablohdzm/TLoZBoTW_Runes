using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(AudioSource))]
public class IceBlock : MonoBehaviour
{
    [SerializeField] private float lifeTimeSpan = 100.0f;
    [SerializeField] private AudioClip iceBlockSpawnSound;
    private float deactivationTime;
    private AudioSource audioSource;
    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = iceBlockSpawnSound;
    }

    private void OnEnable()
    {
        audioSource.Play();
        transform.localScale  = new Vector3(1.0f,0.0f,1.0f);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOScale(Vector3.one, 1.0f));
        s.OnComplete(() => { IsActive = true; });

        deactivationTime = Time.realtimeSinceStartup+ lifeTimeSpan;
        
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup >= deactivationTime) 
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        IsActive = false;
    }
}