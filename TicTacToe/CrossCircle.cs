using System.Linq;
using UnityEngine;

public class CrossCircle : MonoBehaviour
{
    [SerializeField]
    private GameObject Cross;
    [SerializeField]
    private GameObject Circle;
    [SerializeField]
    private int ID;

    private bool clickable = true;

    private void OnMouseDown()
    {
        if (clickable && Stats.Instance.playerTurn && Stats.Instance.isPlayable)
        {
            Instantiate(Circle, transform.position, Quaternion.identity);
            clickable = false;
            Stats.Instance.pressedByP2 = Stats.Instance.pressedByP2.Append(ID).ToArray();
            Stats.Instance.CheckWinCons();
        }
        else if (clickable && Stats.Instance.isPlayable)
        {
            Instantiate(Cross, transform.position, Quaternion.identity);
            clickable = false;
            Stats.Instance.pressedByP1 = Stats.Instance.pressedByP1.Append(ID).ToArray();
            Stats.Instance.CheckWinCons();
        }
    }
}
