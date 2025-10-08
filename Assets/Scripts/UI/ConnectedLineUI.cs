using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class ConnectedLineUI : MonoBehaviour
{
    [SerializeField] private Material _sharedMaterial; // Reference to the shared material
    [SerializeField] private float _revealDuration = 1f;

    private Material _instanceMaterial;
    private Renderer _renderer;

    // private void Awake()
    // {
    //     // Get the renderer component
    //     _renderer = GetComponent<Renderer>();
    //     
    //     // Create an instance of the material to avoid affecting other objects
    //     _instanceMaterial = _renderer.material = new Material(_sharedMaterial);
    // }
    //
    // private void OnEnable()
    // {
    //     // Initialize cutoff to 0 when enabled
    //     _instanceMaterial.SetFloat("_Cutoff", 0f);
    //     StartCoroutine(RevealRoutine());
    // }

    private IEnumerator RevealRoutine()
    {
        float time = 0f;
        while (time < _revealDuration)
        {
            time += Time.deltaTime;
            float cutoff = Mathf.Clamp01(time / _revealDuration);
            _instanceMaterial.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

        // Ensure cutoff is set to 1 at the end
        _instanceMaterial.SetFloat("_Cutoff", 1f);
    }

    private void OnDestroy()
    {
        // Clean up the instanced material to prevent memory leaks
        if (_instanceMaterial != null)
        {
            Destroy(_instanceMaterial);
        }
    }
}