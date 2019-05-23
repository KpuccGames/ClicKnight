using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using SimpleJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum TextColors
{
    Green = 0,
    Yellow = 1,
    Red = 2,
    Light = 3,
}

public static class Helper
{
#if UNITY_EDITOR
    public static bool m_ApplyIphonexSafeArea;
#endif    
    
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly Regex GuildTextInput = new Regex("^[0-9A-Za-z ]+$");
    
    public static bool IsPointerOverGameObject()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current.IsPointerOverGameObject();
#else
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                return true;
        }
        return false;
#endif
    }

    /// <summary>
    /// Определяет, находится ли поинтер непосредственно над UI объектом
    /// </summary>
    public static bool IsPointerOverUI(int pointerId, Vector3 pointerPos)
    {
        if (!EventSystem.current.IsPointerOverGameObject(pointerId))
            return false;

        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            position = pointerPos
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            return raycastResults[0].gameObject.layer == LayerMask.NameToLayer("UI");
        }
        
        return false;
    }

    public static string GetSaveFilesPath()
    {
        string path = Path.Combine(Application.persistentDataPath, "save");
        
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }
    
    public static double DateTimeToTimestampMs(DateTime time) 
    {
        return Math.Round(time.Subtract(Epoch).TotalMilliseconds);
    }

    /// <summary>
    /// Возвращает угол (в градусах) между векторами [-pi, +pi]
    /// </summary>
    public static float GetAngleDegVector2(Vector2 v1, Vector2 v2) 
    {
        Vector2 an = v1.normalized;
        Vector2 bn = v2.normalized;
        float x = an.x * bn.x + an.y * bn.y;
        float y = an.y * bn.x - an.x * bn.y;
        
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Парсит значение Enum из строки, в случае неудачи возвращает дефолтное значение
    /// </summary>
    public static T ParseEnum<T>(string data, T defaultType, bool ignoreCase = true) where T : struct
    {
        try 
        {
            T t = (T)Enum.Parse(typeof(T), data, ignoreCase);
            
            if (!Enum.IsDefined(typeof(T), t))
            {
                return defaultType;
            }
            
            return t;
        } 
        catch
        {
            return defaultType;
        }
    }

    ///////////////
    public static DateTime ParseDateTime(string time)
    {
        return DateTime.Parse(time).ToUniversalTime();
    }

    /// <summary>
    /// Проверяет шанс попадания в указанный диапазон [0, 1]
    /// </summary>
    public static bool CheckChance01(float chance)
    {
        float rnd = UnityEngine.Random.value;
        return rnd < chance;
    }

    /// <summary>
    /// Парсит json из строки
    /// </summary>
    public static JsonObject ParseJson(string data)
    {
        JsonObject json = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(data);
        return json;
    }

    /// <summary>
    /// Парсит json array из строки
    /// </summary>
    public static JsonArray ParseJsonArray(string data)
    {
        JsonArray json = SimpleJson.SimpleJson.DeserializeObject<JsonArray>(data);
        return json;
    }

    ///////////////
    private static void StatFormat(StringBuilder sb, string fmt, float value)
    {
        if (!Mathf.Approximately(value, 0))
        {
            sb.AppendFormat(fmt, value);
            sb.AppendLine();
        }
    }

    ///////////////
    public static Rect GetScreenSafeArea()
    {
        #if UNITY_EDITOR
        if (m_ApplyIphonexSafeArea)
            return new Rect(x: 132.00f, y: 63.00f, width: 2172.00f, height: 1062.00f);
        #endif
        
        return Screen.safeArea;
    }

    ////////////////
    public static string CalculateMD5(string data)
    {
        string sum = string.Empty;

        // переводим строку баланса в MD5 Hash
        byte[] hash = Encoding.UTF8.GetBytes(data);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] hashenc = md5.ComputeHash(hash);

        foreach (byte b in hashenc)
        {
            sum += b.ToString("x2");
        }

        return sum;
    }
}