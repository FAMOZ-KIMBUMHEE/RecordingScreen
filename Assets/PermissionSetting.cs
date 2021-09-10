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
        //저장공간 권한 체크Read
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

            yield return new WaitForSeconds(0.2f); // 0.2초의 딜레이 후 focus를 체크하자.
            yield return new WaitUntil(() => Application.isFocused == true);
        }

        //저장공간 권한 체크Write
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            yield return new WaitForSeconds(0.2f); // 0.2초의 딜레이 후 focus를 체크하자.
            yield return new WaitUntil(() => Application.isFocused == true);
        }
    }
}
