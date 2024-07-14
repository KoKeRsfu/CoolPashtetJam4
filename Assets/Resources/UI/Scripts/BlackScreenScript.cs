using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using static PlayerController;

public class BlackScreenScript : MonoBehaviour
{
    public float animationTime;
    public float deathWaitTime;

    public LensDistortion lensdis_value;
    public PaniniProjection paniniproj_value;
    public Vignette vignette_value;

    public float _current = 1, _target = 0, _current2 = 1, _target2 = 0;
    public float t = 3.5f, t2 = 0;

    public AudioClip changeClip;

    private int position = 0;
    private const int minPosition = 0;
    private const int maxPosition = 80 * 8;

    private float elapsedTime = 0f;

    public IEnumerator AmakeAnimation()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
        elapsedTime = Time.deltaTime;
        while (elapsedTime < animationTime)
        {
            position = (int)(elapsedTime / animationTime * maxPosition);
            this.transform.position = new Vector3((float)position / 8f, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = new Vector3((float)maxPosition / 8f, 0f, 0f);
    }

    public IEnumerator AwayAnimation()
    {
        _target = 1;
        yield return new WaitForSeconds(deathWaitTime);
        GameObject soundPlayer2 = Instantiate(Resources.Load<GameObject>("Audio/Prefabs/SoundPlayer"));
        soundPlayer2.GetComponent<SoundManager>().audioClip = changeClip;
        this.transform.position = new Vector3((float)maxPosition / -8f, 0f, 0f);
        elapsedTime = Time.deltaTime;
        while (elapsedTime < animationTime)
        {
            position = (int)(elapsedTime / animationTime * maxPosition);
            this.transform.position = new Vector3(-80f + (float)position / 8f, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        VolumeProfile volume = Camera.main.GetComponent<Volume>().profile;
        LensDistortion lensdis;
        ChromaticAberration chromab;
        FilmGrain filmgr;
        PaniniProjection paniniproj;
        Vignette vignette;
        LiftGammaGain lgg;
        this.transform.position = new Vector3(0f, 0f, 0f);
        if (volume.TryGet<LensDistortion>(out lensdis)) lensdis_value = lensdis;
        if (volume.TryGet<PaniniProjection>(out paniniproj)) paniniproj_value = paniniproj;
        if (volume.TryGet<Vignette>(out vignette)) vignette_value = vignette;
    }
    private void Awake()
    {
        this.StartCoroutine(AmakeAnimation());
    }

    void Update()
    {
        _current = Mathf.MoveTowards(_current, _target, t * Time.deltaTime);

        lensdis_value.intensity.value = (_current * 0.10f) + 0.2f;
        lensdis_value.scale.value = 1f - (_current * 0.15f);
        paniniproj_value.distance.value = (_current * 0.15f);
        vignette_value.intensity.value = (_current * 0.2f);
    }
}
