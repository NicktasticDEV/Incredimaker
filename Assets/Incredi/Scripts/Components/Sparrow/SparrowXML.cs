using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


[XmlRoot(ElementName="SubTexture")]
public class SubTexture { 

	[XmlAttribute(AttributeName="name")] 
	public string Name { get; set; } 

	[XmlAttribute(AttributeName="x")] 
	public int X { get; set; } 

	[XmlAttribute(AttributeName="y")] 
	public int Y { get; set; } 

	[XmlAttribute(AttributeName="width")] 
	public int Width { get; set; } 

	[XmlAttribute(AttributeName="height")] 
	public int Height { get; set; } 

	[XmlAttribute(AttributeName="frameX")] 
	public int FrameX { get; set; } 

	[XmlAttribute(AttributeName="frameY")] 
	public int FrameY { get; set; } 

	[XmlAttribute(AttributeName="frameWidth")] 
	public int FrameWidth { get; set; } 

	[XmlAttribute(AttributeName="frameHeight")] 
	public int FrameHeight { get; set; } 
}

[XmlRoot(ElementName="TextureAtlas")]
public class TextureAtlas { 

	[XmlElement(ElementName="SubTexture")] 
	public List<SubTexture> SubTexture { get; set; } 

	[XmlAttribute(AttributeName="imagePath")] 
	public string ImagePath { get; set; } 

    public static TextureAtlas Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TextureAtlas));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as TextureAtlas;
        }
    }
}

