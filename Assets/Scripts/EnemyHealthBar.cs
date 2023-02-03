using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private RectTransform rectTransform;

    private float _maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        _maxWidth = rectTransform.rect.width;
    }

    void Update()
    {
        // TODO: Only change this when enemy health updates (sub to event on enemy?)
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _maxWidth * EnemyHealthPercentage());
    }

    private float EnemyHealthPercentage() => enemy.CurrentHealth * 1f / enemy.MaxHealth;
}