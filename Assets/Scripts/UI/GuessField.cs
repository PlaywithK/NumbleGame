using UnityEngine;
using TMPro;
using System.Collections;

public enum FieldState { Empty, Digit, Cursor }

public class GuessField : MonoBehaviour
{
    [Header("Guess Field")]
    public TMP_Text digitText;
    private FieldState _state = FieldState.Empty;
    private Coroutine _blinkCoroutine;

    public void SetDigit(char digit)
    {
        _state = FieldState.Digit;
        StopBlinking();
        digitText.text = digit.ToString();
    }

    public void SetEmpty()
    {
        _state = FieldState.Empty;
        StopBlinking();
        digitText.text = "_";
    }

    public void SetCursor(bool active)
    {
        _state = active ? FieldState.Cursor : FieldState.Empty;

        if (active)
        {
            StartBlinking();
        }
        else
        {
            StopBlinking();
            digitText.text = "";
        }
    }

    private void StartBlinking()
    {
        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = StartCoroutine(Blink());
    }

    private void StopBlinking()
    {
        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;
    }

    public void SetColor(Color c)
    {
        digitText.color = c;
    }

    private IEnumerator Blink()
    {
        while (_state == FieldState.Cursor)
        {
            digitText.text = "█";
            yield return new WaitForSeconds(0.5f);
            digitText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}