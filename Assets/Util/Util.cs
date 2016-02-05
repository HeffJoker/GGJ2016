using UnityEngine;

namespace SF_Tools.Util
{
    public static class Util
    {
        public static bool GetPlayerPref_Bool(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return (PlayerPrefs.GetInt(key) > 0);
            else
                return false;
        }

        public static bool GetPlayerPref_Bool(string key, bool defaultVal)
        {
            if (PlayerPrefs.HasKey(key))
                return (PlayerPrefs.GetInt(key) > 0);
            else
            {
                PlayerPrefs.SetInt(key, (defaultVal ? 1 : 0));
                return defaultVal;
            }
        }

        public static void SetPlayerPref_Bool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, (value ? 1 : 0));
        }

    }
}
