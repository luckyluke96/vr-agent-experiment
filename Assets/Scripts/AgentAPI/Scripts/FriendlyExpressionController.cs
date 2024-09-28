using UnityEngine;

public class FriendlyExpressionController : MonoBehaviour
{
    private OVRLipSyncContext lipSyncContext; // Reference to OVRLipSyncContext
    public SkinnedMeshRenderer faceMesh; // Reference to the character's face mesh
    public int smileBlendshapeIndex = 0; // Index of the smile blendshape
    public int frownBlendshapeIndex = 1; // Index of the frown blendshape
    public float smileIntensity = 50f; // Intensity of the smile when idle
    public float frownIntensity = 0f; // Reset frown intensity when idle

    void Update()
    {
        lipSyncContext = GetComponent<OVRLipSyncContext>();

        if (lipSyncContext != null)
        {
            OVRLipSync.Frame frame = lipSyncContext.GetCurrentPhonemeFrame();

            // Check if there is minimal lip movement (not speaking)
            if (frame != null && IsNotSpeaking(frame))
            {
                // Apply friendly expression (light smile, neutral face)
                faceMesh.SetBlendShapeWeight(smileBlendshapeIndex, smileIntensity);
                faceMesh.SetBlendShapeWeight(frownBlendshapeIndex, frownIntensity);
            }
            else
            {
                // Reset facial expression to neutral when speaking
                faceMesh.SetBlendShapeWeight(smileBlendshapeIndex, 0);
                faceMesh.SetBlendShapeWeight(frownBlendshapeIndex, 0);
            }
        }
    }

    // Simple function to detect if the character is not speaking
    bool IsNotSpeaking(OVRLipSync.Frame frame)
    {
        // You can adjust this condition depending on your viseme setup
        float totalVisemeStrength = 0;
        for (int i = 0; i < frame.Visemes.Length; i++)
        {
            totalVisemeStrength += frame.Visemes[i];
        }

        // Define a threshold to consider the character as "not speaking"
        return totalVisemeStrength < 0.1f;
    }
}
