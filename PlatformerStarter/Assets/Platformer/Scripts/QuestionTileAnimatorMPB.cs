using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class QuestionTileAnimatorMPB : MonoBehaviour
{
    public int tilesY = 5;
    public float secondsPerFrame = 0.12f;
    public bool playOnStart = true;

    Renderer rend;
    MaterialPropertyBlock mpb;

    // Built-in pipeline (some shaders)
    static readonly int MainTexST = Shader.PropertyToID("_MainTex_ST");
    // URP Unlit / URP Lit
    static readonly int BaseMapST = Shader.PropertyToID("_BaseMap_ST");

    Coroutine routine;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        if (playOnStart) routine = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        int frame = 0;

        while (true)
        {
            rend.GetPropertyBlock(mpb);

            float tilingY = 1f / tilesY;
            float offsetY = frame * tilingY;   // 0, -0.2, -0.4, ...

            // (tiling.x, tiling.y, offset.x, offset.y)
            Vector4 st = new Vector4(-1f, -tilingY, 0f, offsetY);

            mpb.SetVector(BaseMapST, st); // ✅ URP Unlit uses this
            mpb.SetVector(MainTexST, st); // optional compatibility

            rend.SetPropertyBlock(mpb);

            frame = (frame + 1) % tilesY;
            
            // Apply animation over time, *_f means waitTime * scalar
            yield return new WaitForSeconds(secondsPerFrame * 1.5f);
        }
    }
}