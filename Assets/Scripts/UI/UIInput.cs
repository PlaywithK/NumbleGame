using UnityEngine;
using UnityEngine.Events;

public class UIInput : MonoBehaviour
{
    public UnityEvent<int> OnDigitPressed;
    public UnityEvent OnDeletePressed;

    public void DigitButtonPressed(int digit)
    {
        OnDigitPressed?.Invoke(digit);
    }

    public void DeleteButtonPressed()
    {
        OnDeletePressed?.Invoke();
    }
}