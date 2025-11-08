using UnityEngine;
using UnityEngine.UI;

public class SpriteArrayAnimation : MonoBehaviour
{
    private const float FPS = 2f;
    [SerializeField] private Image _target;
    [SerializeField] private Sprite[] _sprites;

    private float _timer = 0;
    private int _currentFrame = 0;

    private void Start()
    {
        _target.sprite = _sprites[0];
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 1f / FPS)
        {
            _timer -= 1f / FPS;
            _currentFrame++;
            if(_currentFrame >= _sprites.Length)
            {
                _currentFrame = 0;
            }

            _target.sprite = _sprites[_currentFrame];
        }
    }
}