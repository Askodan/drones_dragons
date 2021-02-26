using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlendShapeAnimator : MonoBehaviour
{
    public float frame_delay = 0;
    int max;
    SkinnedMeshRenderer SMR;
    void Start()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        max = SMR.sharedMesh.blendShapeCount;
        StartCoroutine(Animate());
    }
    IEnumerator Animate()
    {
        int frame = 0;
        while (true)
        {
            SMR.SetBlendShapeWeight(frame, 0f);
            frame++;
            if (frame == max)
            {
                frame = 0;
            }
            SMR.SetBlendShapeWeight(frame, 100f);

            yield return new WaitForSeconds(frame_delay);
        }
    }
}
