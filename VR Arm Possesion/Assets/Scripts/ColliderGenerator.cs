using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGenerator : MonoBehaviour
{
    GameObject Model;
    public GameObject[] Bones;
    GameObject GlobalLeft;
    GameObject GlobalRight;
    [SerializeField] float SizeThreshold = 0;
    [SerializeField] float RadiusMultiplier = 1;
    [SerializeField] float HeightMultiplier = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (SizeThreshold == 0)
            SizeThreshold = 0.05f;

        Model = transform.Find("MODEL").transform.GetChild(0).gameObject;
        GlobalLeft = GameObject.Find("GLOBAL_LEFT");
        GlobalRight = GameObject.Find("GLOBAL_RIGHT");
        GetAllBones();
        AddColliders();
    }

    // Gets which side the arm is on
    float GetClosestSide(GameObject origin)
    {
        //Debug.Log("LEFT DISTANCE: " + Vector3.Distance(origin.transform.position, GlobalLeft.transform.position) +
        //    " RIGHT DISTANCE: " + Vector3.Distance(origin.transform.position, GlobalRight.transform.position));

        if (Vector3.Distance(origin.transform.position, GlobalLeft.transform.position) > Vector3.Distance(origin.transform.position, GlobalRight.transform.position))
            return 1;
        else
            return -1;
    }

    // Gets all bones from the current arm models and sets them to the correct layer.
    // Will ignore any GameObject with a SkinnedMeshRenderer tagged with "IGNORE"
    void GetAllBones()
    {
        List<GameObject> boneList = new List<GameObject>();
        var renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i].gameObject.tag != "IGNORE")
                for (int j = 0; j < renderer[i].bones.Length; j++)
                {
                    boneList.Add(renderer[i].bones[j].gameObject);
                }
        }
        Bones = boneList.ToArray();

        for (int i = 0; i < Bones.Length; i++)
        {
            Bones[i].layer = 3;
            if (Bones[i].tag == "IGNORE")
            {
                Transform[] children = Bones[i].GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    child.tag = "IGNORE";
                }
            }
        }
    }

    // Adds a capsule collider and rigidbody to every bone in the model.
    // Additionally, each bone will be tagged with "ARM_COL" on Start()
    void AddColliders()
    {
        for (int i = 0; i < Bones.Length; i++)
        {
            if (Bones[i].tag != "IGNORE")
            {
                var col = Bones[i].AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
                var rig = Bones[i].AddComponent(typeof(Rigidbody)) as Rigidbody;
                var wa = Bones[i].AddComponent(typeof(WhichArm)) as WhichArm;
                if (GetClosestSide(Bones[i]) == 1)
                    Bones[i].tag = "COL_RIGHT";
                else
                    Bones[i].tag = "COL_LEFT";
                rig.isKinematic = true;
                rig.useGravity = false;
                col.isTrigger = false;

                // float distanceFromClosestBone = Vector3.Distance(Bones[i].transform.position, GetClosestBone(Bones, Bones[i]).position);

                /*
                if (distanceFromClosestBone >= SizeThreshold)
                {
                    col.height = distanceFromClosestBone * 12f;
                    col.radius = distanceFromClosestBone * 2.5f;
                    col.center = new Vector3(0, distanceFromClosestBone * 5f, 0);
                }
                else
                {
                    col.radius = distanceFromClosestBone * 2.5f;
                    col.center = new Vector3(0, distanceFromClosestBone * 5f, 0);
                }
                */

                col.radius = 1;
                col.center = Vector3.zero;

                col.radius *= RadiusMultiplier;
                col.height *= HeightMultiplier;
            }
        }
    }

    // Returns the closest bone's transform (boneToExclude) connected to the same arm
    // in the given bone array.
    Transform GetClosestBone(GameObject[] bone, GameObject boneToExclude)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in bone)
        {
            if (t != boneToExclude && (t.transform.Find(boneToExclude.name) || t.transform.parent.gameObject == boneToExclude))
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
        }
        // Debug.Log("Closest Bone to " + boneToExclude.ToString() + " is " + tMin.gameObject.ToString());
        return tMin;
    }

}
