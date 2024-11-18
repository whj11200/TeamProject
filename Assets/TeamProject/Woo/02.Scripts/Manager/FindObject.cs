using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    public static GameObject InstantiateWithLogging(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Object.Instantiate(prefab, position, rotation);
        print($"생성된 오브젝트: {obj.name} (위치: {position}) - {GetCallerMethod()}");
        return obj;
    }

    private static string GetCallerMethod()
    {
        // 스택 추적을 통해 호출한 메서드 정보를 가져옴
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        // 1: 현재 메서드, 2: 호출한 메서드
        var frame = stackTrace.GetFrame(2);
        return $"{frame.GetMethod().DeclaringType}.{frame.GetMethod().Name} (라인: {frame.GetFileLineNumber()})";
    }
}
