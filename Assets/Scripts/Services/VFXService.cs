// VFXService.cs
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class VFXService
{
    private readonly Dictionary<string, Queue<GameObject>> _pool = new();

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        var key = prefab.name;
        if (!_pool.ContainsKey(key)) _pool[key] = new Queue<GameObject>();

        GameObject vfx;
        if (_pool[key].Count > 0)
        {
            vfx = _pool[key].Dequeue();
            vfx.transform.position = position;
            vfx.SetActive(true);
        }
        else
        {
            vfx = Object.Instantiate(prefab, position, Quaternion.identity);
        }

        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            float duration = ps.main.duration + ps.main.startLifetime.constantMax;
            ReturnAfterDelay(vfx, key, duration);
        }

        return vfx;
    }

    private async void ReturnAfterDelay(GameObject vfx, string key, float delay)
    {
        await System.Threading.Tasks.Task.Delay((int)(delay * 1000));
        if (vfx == null) return;
        vfx.SetActive(false);
        _pool[key].Enqueue(vfx);
    }
}