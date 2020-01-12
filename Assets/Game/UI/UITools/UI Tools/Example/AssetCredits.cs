using UnityEngine;

/// <summary>
/// UI Tool For Unity
/// 
/// Developed by Dibbie.
/// Developer Website: http://www.simpleminded.x10host.com
/// Developer Email: mailto:strongstrenth@hotmail.com
/// Discord Server: https://discord.gg/33PFeMv
/// 
/// Artwork by Caroldot.
/// Artist Website: https://www.instagram.com/caroldot.art/
/// Artist Email: mailto:ana.carolina.m.franca@gmail.com
/// 
/// - You can send an email to the developer for any technical support, assistance or bug reports with the asset.
/// - Feel free to join the Discord server and say hi. Directly communicate with the developer & some of my dev friends who help work
/// on amazing games, assets and ideas with me. You can also make suggestions and report bugs there, as well as ask for assistance.
/// </summary>

public class AssetCredits : MonoBehaviour {
    
    public void OnMouseOver(Texture2D icon)
    {
        Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseClick(string website)
    {
        print("Loading external link: " + website + " (" + name + " website)");
        Application.OpenURL(website);
    }
}
