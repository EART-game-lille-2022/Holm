using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FeedBackButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Button button;
    private AudioSource audioSource;

    public float scaleFactor = 1.2f; // Facteur d'échelle pour le zoom
    public AudioClip hoverSound; // Son à jouer lors du survol
    public float soundVolume = 1.0f; // Niveau sonore du son
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Assurez-vous que le bouton a un composant RectTransform pour accéder à la taille
        if (button != null)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                originalScale = rectTransform.localScale;
            }
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Appliquer le zoom quand la souris entre sur le bouton
        if (button != null)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = originalScale * scaleFactor;
            }
        }

        // Jouer le son lors du survol avec le niveau sonore spécifié
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound, soundVolume);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Rétablir la taille d'origine quand la souris quitte le bouton
        if (button != null)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = originalScale;
            }
        }
    }
}
