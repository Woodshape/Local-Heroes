using LH.GOAP;
using UnityEngine;

namespace DefaultNamespace {
    //The class to link the GAgent code with the Inspector Editor so the agent's
    //properties can be displayed in the Inspector
    [ExecuteInEditMode]
    public class GAgentVisualizer : MonoBehaviour
    {
        public GAgent thisAgent;

        // Start is called before the first frame update
        void Start()
        {
            thisAgent = this.GetComponent<GAgent>();
        }
    }
}
