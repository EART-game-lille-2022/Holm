using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPackage : MonoBehaviour
{
    public static List<LostPackage> LostPackageList = new List<LostPackage>();

    void Awake()
    {
        LostPackageList.Add(this);
    }

    public void OnPickUp()
    {
        gameObject.SetActive(false);
        CanvasManager.instance.SetLostPackageFill(PackagePickUpRatio(out int disablePackage), disablePackage);
    }

    private float PackagePickUpRatio(out int disablePackage)
    {
        disablePackage = 0;
        foreach (var item in LostPackageList)
            disablePackage += item.gameObject.activeSelf ? 0 : 1;

        print("AP : " + disablePackage);
        print("LP count : " + LostPackageList.Count);
        print("RA : " + Mathf.InverseLerp(0, LostPackageList.Count, disablePackage));

        return Mathf.InverseLerp(0, LostPackageList.Count, disablePackage);
    }
}
