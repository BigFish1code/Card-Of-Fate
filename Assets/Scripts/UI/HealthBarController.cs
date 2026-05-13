using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    [Header(header: "Elements")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    private VisualElement defenseElement;
    private Label defenseAmountlabel;

    private VisualElement buffElement;
    private Label buffRound;
    [Header(header: "buffsprite")]
    public Sprite buffSprite;
    public Sprite debuffSpirte;

    private Enemy enemy;
    private VisualElement IntentSprite;
    private Label IntentAmount;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        enemy = GetComponent<Enemy>();
    }
    private void OnEnable()
    {
       InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>(name: "HealthBar");

        healthBar.highValue = currentCharacter.MaxHP;
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);

        defenseElement = healthBar.Q<VisualElement>(name: "Defense");
        defenseAmountlabel = defenseElement.Q<Label>(name: "DefenseAmount");
        defenseElement.style.display = DisplayStyle.None;

        buffElement = healthBar.Q<VisualElement>(name: "Buff");
        buffRound = buffElement.Q<Label>(name: "BuffRound");
        buffElement.style.display = DisplayStyle.None;

        IntentSprite = healthBar.Q<VisualElement>(name: "Intent");
        IntentAmount = healthBar.Q<Label>(name: "IntentAmount");
        IntentSprite.style.display = DisplayStyle.None;
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }
        if (healthBar != null)
        {
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
            healthBar.value = currentCharacter.CurrentHP;

            healthBar.RemoveFromClassList(className: "highHealth");
            healthBar.RemoveFromClassList(className: "mediumHealth");
            healthBar.RemoveFromClassList(className: "lowHealth");

            var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;
            if (percentage < 0.3f)
            {
                healthBar.AddToClassList(className: "lowHealth");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList(className: "mediumHealth");
            }
            else
            {
                healthBar.AddToClassList(className: "highHealth");
            }
            //防御
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountlabel.text = currentCharacter.defense.currentValue.ToString();
            //力量回合更新
            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ?
                DisplayStyle.Flex : DisplayStyle.None;
            buffRound.text = currentCharacter.buffRound.currentValue.ToString();
            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1 ?
                new StyleBackground(buffSprite) : new StyleBackground(debuffSpirte);

        }
    }
    public void SetIntentElement()
    {
        IntentSprite.style.display = DisplayStyle.Flex;
        IntentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);

        //判断是否攻击
        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(AttackEffect))
        {
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }
        IntentAmount.text = value.ToString();
    }
    public void HideIntentElement()
    {
        IntentSprite.style.display= DisplayStyle.None;
    }
}
