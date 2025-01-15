using UnityEngine;
namespace VRTeamProject01
{
    public class BatCapsule : MonoBehaviour
    {
        [SerializeField]
        private BatCapsuleFollower _batCapsuleFollowerPrefab;

        private void SpawnBatCapsuleFollower()
        {
            var follower = Instantiate(_batCapsuleFollowerPrefab);
            GameObject batFollower = GameObject.Find("Bat Follower");
            if(batFollower == null)
            {
                batFollower = new GameObject("Bat Follower");
            }
            follower.transform.parent = batFollower.transform;
            follower.transform.position = transform.position;
            follower.transform.rotation = transform.rotation;
            follower.SetFollowTarget(this);
            follower.Disable();
            //follower.gameObject.SetActive(false);
        }

        public void Started()
        {
            SpawnBatCapsuleFollower();
        }
    }
}