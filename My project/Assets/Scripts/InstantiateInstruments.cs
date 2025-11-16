using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Places the provided instrument prefabs on detected surfaces in front of the player.
/// </summary>
public class InstantiateInstruments : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> instrumentPrefabs = new List<GameObject>();

    [Header("References")]
    [SerializeField] private Transform userReference;
    [SerializeField] private LayerMask surfaceLayers = ~0;
    [SerializeField] private ARPlaneManager planeManager;

    [Header("Placement")]
    [SerializeField] private float minSpawnDistance = 0.6f;
    [SerializeField] private float maxSpawnDistance = 1.2f;
    [SerializeField] private float maxYawDegrees = 30f;
    [SerializeField] private float maxPitchDegrees = 10f;
    [SerializeField] private float surfaceProbeHeight = 0.6f;
    [SerializeField] private int maxSpawnAttemptsPerInstrument = 10;

    private readonly List<GameObject> spawnedInstruments = new List<GameObject>();

    private void Awake()
    {
        if (userReference != null) return;
        if (Camera.main != null)
        {
            userReference = Camera.main.transform;
        }
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
        }
    }

    private void Start()
    {
        if (userReference == null)
        {
            Debug.LogWarning("InstantiateInstruments cannot place anything without a user reference.", this);
            return;
        }

        SpawnInstruments();
    }

    /// <summary>
    /// Clears previously spawned instruments and places them again.
    /// </summary>
    public void SpawnInstruments()
    {
        if (userReference == null || instrumentPrefabs.Count == 0) return;

        ClearSpawned();

        foreach (var prefab in instrumentPrefabs)
        {
            if (prefab == null) continue;
            SpawnSingleInstrument(prefab);
        }
    }

    private void SpawnSingleInstrument(GameObject prefab)
    {
        for (var attempt = 0; attempt < maxSpawnAttemptsPerInstrument; attempt++)
        {
            var direction = SampleDirectionInCone();
            var distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            var target = userReference.position + direction * distance;
            var probeStart = target + Vector3.up * surfaceProbeHeight;

            if (!Physics.Raycast(probeStart, Vector3.down, out var hit, surfaceProbeHeight + 0.1f, surfaceLayers)) continue;
            if (!TryGetPlaneFromHit(hit, out var plane)) continue;

            var spawnPosition = hit.point + hit.normal * 0.01f;
            var spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0, Random.Range(0f, 360f), 0f);
            var instrument = Instantiate(prefab, spawnPosition, spawnRotation, plane.transform);
            instrument.transform.SetParent(plane.transform, true);

            EnsureCollisionAudio(instrument);
            spawnedInstruments.Add(instrument);
            return;
        }

        Debug.LogWarning($"Failed to place {prefab.name} after {maxSpawnAttemptsPerInstrument} tries.", this);
    }

    private bool TryGetPlaneFromHit(RaycastHit hit, out ARPlane plane)
    {
        plane = hit.collider.GetComponentInParent<ARPlane>();
        return plane != null;
    }

    private Vector3 SampleDirectionInCone()
    {
        var yaw = Random.Range(-maxYawDegrees, maxYawDegrees);
        var pitch = Random.Range(-maxPitchDegrees, maxPitchDegrees);
        var rotation = userReference.rotation * Quaternion.Euler(pitch, yaw, 0f);
        return rotation * Vector3.forward;
    }

    private void EnsureCollisionAudio(GameObject instrument)
    {
        if (instrument.GetComponent<InstrumentCollisionSound>() != null) return;
        instrument.AddComponent<InstrumentCollisionSound>();
    }

    private void ClearSpawned()
    {
        for (var i = spawnedInstruments.Count - 1; i >= 0; i--)
        {
            var instrument = spawnedInstruments[i];
            if (instrument != null)
            {
                Destroy(instrument);
            }
        }

        spawnedInstruments.Clear();
    }
}

