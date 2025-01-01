using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Extrite
{
    [XmlRoot(ElementName="SubTexture")]
    public class SubTexture { 

        [XmlAttribute(AttributeName="name")] 
        public string name { get; set; } 

        // Frame location
        [XmlAttribute(AttributeName="x")] 
        public int x { get; set; } 

        [XmlAttribute(AttributeName="y")] 
        public int y { get; set; } 

        [XmlAttribute(AttributeName="width")] 
        public int width { get; set; } 

        [XmlAttribute(AttributeName="height")] 
        public int height { get; set; } 

        // (We know FrameX and frameY are for positioning the frame, but frameWidth and frameHeight are unknown)
        [XmlAttribute(AttributeName="frameX")] 
        public int frameX { get; set; } 

        [XmlAttribute(AttributeName="frameY")] 
        public int frameY { get; set; } 

        [XmlAttribute(AttributeName="frameWidth")] 
        public int frameWidth { get; set; } 

        [XmlAttribute(AttributeName="frameHeight")] 
        public int frameHeight { get; set; } 
    }

    [XmlRoot(ElementName="TextureAtlas")]
    public class TextureAtlas { 

        [XmlElement(ElementName="SubTexture")] 
        public List<SubTexture> SubTexture { get; set; } 

        [XmlAttribute(AttributeName="imagePath")] 
        public string imagePath { get; set; } 
    }
}

