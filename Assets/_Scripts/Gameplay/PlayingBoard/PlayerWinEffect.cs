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

    [SerializeField] private AnimationCurve _moveCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Min(0f)]
    [SerializeField] private float _lineOvershootOffset = 0.15f;

    private float _lineStartColorAlpha = 1f;
    private float _lineEndColorAlpha = 1f;

    protected override void Validate()
    {
        if (!_particles)
            FailValidation("Particle System not assigned.");

        if (!_line)
            FailValidation("Line Renderer not assigned.");

        if (_moveCurve == null || _moveCurve.length < 1)
            FailValidation("Move curve data is invalid.");
    }

    protected override void OnInitializationSuccess()
    {
        _lineStartColorAlpha = _line.startColor.a;
        _lineEndColorAlpha = _line.endColor.a;
    }

    public void SetColor(Color color)
    {
        SetParticleColor(color);
        SetLineColor(color);
    }

    private void SetParticleColor(Color color)
    {
        ParticleSystem.MainModule particlesData = _particles.main;
        particlesData.startColor = color;
    }

    private void SetLineColor(Color color)
    {
        Color startColor = color;
        startColor.a = _lineStartColorAlpha;

        Color endColor = color;
        endColor.a = _lineEndColorAlpha;

        _line.startColor = startColor;
        _line.endColor = endColor;
    }

    public void Play(Vector3 startPosition, Vector3 endPosition, Action onFinishCallback = null)
    {
        StartCoroutine(PlayAnimation(startPosition, endPosition, onFinishCallback));
    }

    private IEnumerator PlayAnimation(Vector3 startPosition, Vector3 endPosition, Action onFinishCallback = null)
    {
        startPosition = Vector3.LerpUnclamped(startPosition, endPosition, 0f - _lineOvershootOffset);
        endPosition = Vector3.LerpUnclamped(startPosition, endPosition, 1f + _lineOvershootOffset);

        _line.positionCount = 2;

        _line.SetPosition(0, startPosition);
        _line.SetPosition(1, startPosition);

        _line.gameObject.SetActive(true);
        _particles.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            float t = elapsedTime / _moveDuration;
            float tCurved = _moveCurve.Evaluate(t);

            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, tCurved);
            
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
