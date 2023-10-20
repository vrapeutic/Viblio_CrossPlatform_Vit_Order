using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform laserTarget;
    [SerializeField] GameObject[] laserComponents;
    Vector3 laserDir;
    LineRenderer line;
    float beamEndOffset = .2f;//How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    // Start is called before the first frame update
    void Start()
    {

        line = laserComponents[1].GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        laserDir = laserTarget.position-laserTarget.position;
        ShootBeamInDir(transform.position, laserDir);
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        laserComponents[0].transform.position = start;

        Vector3 end = Vector3.zero;
        //RaycastHit hit;
        //if (Physics.Raycast(start, dir, out hit))
        //    end = hit.point - (dir.normalized * beamEndOffset);
        //else
        //    end = transform.position + (dir * 100);

        //
        end = laserTarget.position - (dir.normalized * beamEndOffset); ;
        //laserComponents[2].transform.position = end;
        line.SetPosition(1, end);

        laserComponents[0].transform.LookAt(end);
        //laserComponents[2].transform.LookAt(laserComponents[0].transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    ////public void SetTarget(Transform target)
    ////{

    ////}
}
