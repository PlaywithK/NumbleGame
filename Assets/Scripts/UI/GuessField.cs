using UnityEngine;
using TMPro;
using System.Collections;

public enum FieldState { Empty, Digit, Cursor }

public class GuessField : MonoBehaviour
{
    [Header("Guess Field")]
    public TMP_Text text;
    private FieldState state = FieldState.Empty;
    private Coroutine blinkCoroutine;

    public void SetDigit(char digit)
    {
        state = FieldState.Digit;
        StopBlinking();
        text.text = digit.ToString();
    }

    public void SetEmpty()
    {
        state = FieldState.Empty;
        StopBlinking();
        text.text = "_";
    }

    public void SetCursor(bool active)
    {
        state = active ? FieldState.Cursor : FieldState.Empty;

        if (active)
        {
            StartBlinking();
        }
        else
        {
            StopBlinking();
            text.text = "";
        }
    }

    private void StartBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(Blink());
    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
    }

    public void SetColor(Color c)
    {
        text.color = c;
    }

    private IEnumerator Blink()
    {
        while (state == FieldState.Cursor)
        {
            text.text = "█";
            yield return new WaitForSeconds(0.5f);
            text.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}