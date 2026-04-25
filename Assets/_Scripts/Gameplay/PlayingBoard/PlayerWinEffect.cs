using System;
using System.Collections;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class PlayerWinEffect : SnekMonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private LineRenderer _line;

    [Space(10f)]
    [Min(0.1f)]
    [SerializeField] private float _moveDuration = 1f;

    protected override void Validate()
    {
        if (!_particles)
            FailValidation("Particle System not assigned.");

        if (!_line)
            FailValidation("Line Renderer not assigned.");
    }

    public void Play(Vector3 startPosition, Vector3 endPosition, Action onFinishCallback = null)
    {
        StartCoroutine(PlayAnimation(startPosition, endPosition, onFinishCallback));
    }

    private IEnumerator PlayAnimation(Vector3 startPosition, Vector3 endPosition, Action onFinishCallback = null)
    {
        _line.positionCount = 2;

        _line.SetPosition(0, startPosition);
        _line.SetPosition(1, startPosition);

        _line.gameObject.SetActive(true);
        _particles.gameObject.SetActive(true);

        float t = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            t = elapsedTime / _moveDuration;

            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
            
            _particles.transform.position = currentPosition;
            _line.SetPosition(1, currentPosition);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        onFinishCallback?.Invoke();
    }

    public void Stop()
    {
        _line.gameObject.SetActive(false);
        _particles.gameObject.SetActive(false);
    }
}
