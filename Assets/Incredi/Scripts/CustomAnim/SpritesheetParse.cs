using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SpritesheetParse : MonoBehaviour
{
    // This script will be used to parse spritesheets with sparrow format
    // Sparrow format is an XML

    /* Example of XML format:
    <TextureAtlas imagePath="Image.png">
	    <SubTexture name="Character 10000" x="5" y="5" width="184" height="290" frameX="-4" frameY="-5" frameWidth="192" frameHeight="295"/>
        <SubTexture name="Character 10001" x="194" y="5" width="184" height="292" frameX="-4" frameY="-3" frameWidth="192" frameHeight="295"/>
    </TextureAtlas>
    */

    public TextAsset spritesheetXML;
    public Texture2D spritesheetTexture;

    private Dictionary<string, Sprite> sprites;

    void Start()
    {
        ParseSpritesheet();
    }

    void ParseSpritesheet()
    {
        sprites = new Dictionary<string, Sprite>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(spritesheetXML.text);

        XmlNodeList subTextures = xmlDoc.GetElementsByTagName("SubTexture");

        foreach (XmlNode subTexture in subTextures)
        {
            string name = subTexture.Attributes["name"].Value;
            int x = int.Parse(subTexture.Attributes["x"].Value);
            int y = int.Parse(subTexture.Attributes["y"].Value);
            int width = int.Parse(subTexture.Attributes["width"].Value);
            int height = int.Parse(subTexture.Attributes["height"].Value);

            Rect rect = new Rect(x, y, width, height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Center pivot

            Sprite sprite = Sprite.Create(spritesheetTexture, rect, pivot);
            sprites.Add(name, sprite);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        if (sprites.ContainsKey(name))
        {
            return sprites[name];
        }
        return null;
    }

}
