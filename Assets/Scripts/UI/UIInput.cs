using UnityEngine;
using UnityEngine.Events;

public class UIInput : MonoBehaviour
{
    public UnityEvent<int> OnDigitPressed;

    public void DigitButtonPressed(int digit)
    {
        OnDigitPressed?.Invoke(digit);
    }

    public UnityEvent OnDeletePressed;

    public void DeleteButtonPressed()
    {
        OnDeletePressed?.Invoke();
    }
}