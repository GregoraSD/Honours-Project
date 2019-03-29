using UnityEngine;

[CreateAssetMenu(fileName = "Step Cycle Collection")]
public class StepCycleCollection : ScriptableObject
{
    public StepCycleGroup[] cycleGroups;

    public StepCycleGroup FindGroupWithTag(string tag)
    {
        for(int i = 0; i < cycleGroups.Length; i++)
        {
            if(cycleGroups[i].tag == tag)
            {
                return cycleGroups[i];
            }
        }

        return null;
    }
}