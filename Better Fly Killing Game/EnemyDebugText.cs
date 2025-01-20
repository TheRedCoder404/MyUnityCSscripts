using UnityEngine;
using TMPro;

public class EnemyDebugText : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior enemy;

    public TMP_Text textBox;

    Vector2 enemyPos;


    // checks at the start if debugText was enabled, if true it activates the text and starts printing the debug stuff
    private void Awake()
    {
        if (Stats.Instance.debugText)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // checks if debugText is still true or if it is false
    private void Update()
    {
        if (Stats.Instance.debugText)
        {
            printDebugText();
        }
        else
        {
            textBox.text = "";
            gameObject.SetActive(false);
        }
    }

    // prints the debug stuff and checks if the debug Text was switched off in the meantime
    void printDebugText()
    {
        textBox.text = ("Touching Racket: " + enemy.touchingRacket);
        enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
        enemyPos.y += 30;
        transform.position = enemyPos;
    }
}
