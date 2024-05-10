using UnityEngine;

public class SCR_OverlayController : MonoBehaviour
{
    [field: Header("_")]
    [field: SerializeField] private FullScreenPassRendererFeature overlayRenderer = null;

    [field: Space(10), Header("_")]
    [field: SerializeField] private Material overlayScopeMaterial = null;
    [field: SerializeField] private Material overlayDamageMaterial = null;
    [field: SerializeField] private Material overlayMaskMaterial = null;

    [field: Space(10), Header("_")]
    [field: SerializeField] private Vector2 overlaySwayAmplitude = Vector2.one;
    [field: SerializeField, Min(0)] private float overlaySwayAcceleration = 2.5f;

    private bool isEnabled = false;
    private Vector2 targetVector = Vector2.zero;
    private Vector2 currentVector = Vector2.zero;
    private Material currentMaterial = null;

    private void Start()
    {
        currentMaterial = overlayScopeMaterial;
        Hide();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        if (!isEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMaterial = overlayDamageMaterial;
            Show();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMaterial = overlayScopeMaterial;
            Show();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMaterial = overlayMaskMaterial;
            Show();
        }

        if (currentMaterial == overlayMaskMaterial || currentMaterial == overlayDamageMaterial)
        {
            return;
        }

        targetVector.y += Input.GetAxisRaw("Mouse Y") * overlaySwayAmplitude.y * Time.deltaTime;
        targetVector.x += Input.GetAxisRaw("Mouse X") * overlaySwayAmplitude.x * Time.deltaTime;
        targetVector = Vector2.Lerp(targetVector, Vector2.zero, overlaySwayAcceleration * Time.deltaTime);
        currentVector = Vector2.Lerp(currentVector, targetVector, overlaySwayAcceleration * Time.deltaTime);
        currentMaterial.SetVector("_VignetteOffset", currentVector);
    }
#if UNITY_EDITOR
    private void OnApplicationQuit() => Hide();
#endif
    private void Show()
    {
        overlayRenderer.SetActive(true);
        overlayRenderer.passMaterial = currentMaterial;
    }
    private void Hide()
    {
        overlayRenderer.SetActive(false);
        targetVector = Vector2.zero;
        currentVector = Vector2.zero;
        currentMaterial.SetVector("_VignetteOffset", Vector2.zero);
    }
}
