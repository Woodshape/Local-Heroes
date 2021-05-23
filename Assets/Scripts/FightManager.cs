using LH.Actions;
using UnityEngine;

namespace DefaultNamespace {
    public static class FightManager {
        private static float offset = 5f;
        
        public static void SetFightDestination(GameObject aggressor, GameObject defender) {
            GameObject go = new GameObject("Fight Destination");

            Vector3 point = GetPoint(aggressor, defender);

            go.transform.position = point;
            
            aggressor.GetComponent<FightAction>().destinationGO = go;
            defender.GetComponent<FightAction>().destinationGO = go;
        }

        private static Vector3 GetPoint(GameObject p1, GameObject p2) {
            //get the positions of our transforms
            Vector3 pos1 = p1.transform.position;
            Vector3 pos2 = p2.transform.position;
    
            //get the direction between the two transforms -->
            Vector3 dir = (pos2 - pos1).normalized;
 
            //get a direction that crosses our [dir] direction
            //NOTE! : this can be any of a buhgillion directions that cross our [dir] in 3D space
            //To alter which direction we're crossing in, assign another directional value to the 2nd parameter
            Vector3 perpDir = Vector3.Cross(dir, Vector3.right);
 
            //get our midway point
            Vector3 midPoint = (pos1 + pos2) / 2f;
 
            //get the offset point
            //This is the point you're looking for.
            Vector3 offsetPoint = midPoint + (perpDir * offset);
            offsetPoint.y = 1.1f;

            return offsetPoint;
        }
    }
}
