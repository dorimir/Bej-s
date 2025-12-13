using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager estático que controla qué popups ya se mostraron
/// </summary>
public static class PopupManager
{
    private static HashSet<string> shownPopupsThisSession = new HashSet<string>();

    public static bool HasBeenShown(string popupID)
    {
        // Verificar en esta sesión
        if (shownPopupsThisSession.Contains(popupID))
            return true;

        // Verificar en PlayerPrefs (guardado permanente)
        string saveKey = "Popup_" + popupID + "_Shown";
        return PlayerPrefs.GetInt(saveKey, 0) == 1;
    }

    public static void MarkAsShown(string popupID)
    {
        // Marcar en esta sesión
        shownPopupsThisSession.Add(popupID);

        // Guardar permanentemente
        string saveKey = "Popup_" + popupID + "_Shown";
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();
    }

    public static void ResetPopup(string popupID)
    {
        shownPopupsThisSession.Remove(popupID);
        string saveKey = "Popup_" + popupID + "_Shown";
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
    }

    public static void ResetAllPopups()
    {
        shownPopupsThisSession.Clear();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}