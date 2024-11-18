using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    public static GameObject InstantiateWithLogging(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Object.Instantiate(prefab, position, rotation);
        print($"������ ������Ʈ: {obj.name} (��ġ: {position}) - {GetCallerMethod()}");
        return obj;
    }

    private static string GetCallerMethod()
    {
        // ���� ������ ���� ȣ���� �޼��� ������ ������
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        // 1: ���� �޼���, 2: ȣ���� �޼���
        var frame = stackTrace.GetFrame(2);
        return $"{frame.GetMethod().DeclaringType}.{frame.GetMethod().Name} (����: {frame.GetFileLineNumber()})";
    }
}
