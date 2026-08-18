using UnityEngine;

public static class Idioma
{
    public const string PT = "pt";
    public const string EN = "en";

    private const string ChavePref = "Idioma";

    private static string atual;

    public static string Atual
    {
        get
        {
            if (atual == null)
                atual = PlayerPrefs.GetString(ChavePref, PT);
            return atual;
        }
        set
        {
            atual = value;
            PlayerPrefs.SetString(ChavePref, value);
            PlayerPrefs.Save();
        }
    }
}
