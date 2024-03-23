// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class TextFade : MonoBehaviour
//     {
//     public Text text;
//     public float fadeDuration = 0.5f;

//     private bool isFading = false;

//     private void Start()
//     {
//         // Instantiate your object and then start the fade coroutine
//         StartCoroutine(FadeText());
//     }

//     private IEnumerator FadeText()
//     {
//         // Set text color to fully transparent
//         SetTextColorAlpha(0f);

//         // Wait for a short delay before starting the fade in animation
//         yield return new WaitForSeconds(0.5f);

//         // Fade in
//         yield return FadeToAlpha(1f);

//         // Wait for a short delay before starting the fade out animation
//         yield return new WaitForSeconds(0.5f);

//         // Fade out
//         yield return FadeToAlpha(0f);

//         // Restart the process
//         StartCoroutine(FadeText());
//     }

//     private IEnumerator FadeToAlpha(float targetAlpha)
//     {
//         isFading = true;

//         Color currentColor = text.color;
//         float startAlpha = currentColor.a;
//         float startTime = Time.time;

//         while (Time.time < startTime + fadeDuration)
//         {
//             float elapsedTime = Time.time - startTime;
//             float percentageComplete = elapsedTime / fadeDuration;

//             currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, percentageComplete);
//             SetTextColorAlpha(currentColor.a);

//             yield return null;
//         }

//         isFading = false;
//     }

//     private void SetTextColorAlpha(float alpha)
//     {
//         Color color = text.color;
//         color.a = alpha;
//         text.color = color;
//     }
//     }
