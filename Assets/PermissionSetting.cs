using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetPermission());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetPermission()
    {
        //������� ���� üũRead
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

            yield return new WaitForSeconds(0.2f); // 0.2���� ������ �� focus�� üũ����.
            yield return new WaitUntil(() => Application.isFocused == true);
        }

        //������� ���� üũWrite
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            yield return new WaitForSeconds(0.2f); // 0.2���� ������ �� focus�� üũ����.
            yield return new WaitUntil(() => Application.isFocused == true);
        }
    }
}
