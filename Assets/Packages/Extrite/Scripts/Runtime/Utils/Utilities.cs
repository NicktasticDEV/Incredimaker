using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Extrite;
using System;
using System.IO;
using System.Text;
using System.IO.Compression;


namespace Extrite
{
    public class Utilities : MonoBehaviour
    {
        public static void ExportSparrowAnimationPack(SO_SparrowAnimationPack sparrowAnimationPack, string path)
        {
            if (path.Length == 0)
            {
                return;
            }

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    // Add the texture to the zip archive
                    ZipArchiveEntry textureEntry = zipArchive.CreateEntry("texture.png");
                    using (Stream stream = textureEntry.Open())
                    {
                        byte[] textureData = sparrowAnimationPack.texture.EncodeToPNG();
                        stream.Write(textureData, 0, textureData.Length);
                    }

                    // Add the atlas to the zip archive
                    ZipArchiveEntry atlasEntry = zipArchive.CreateEntry("atlas.xml");
                    using (Stream stream = atlasEntry.Open())
                    {
                        byte[] atlasData = Encoding.UTF8.GetBytes(sparrowAnimationPack.atlas.text);
                        stream.Write(atlasData, 0, atlasData.Length);
                    }

                    // Add the animations configuration to the zip archive
                    ZipArchiveEntry binaryEntry = zipArchive.CreateEntry("animations.esac");
                    using (Stream stream = binaryEntry.Open())
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            // Write Header
                            writer.Write(Encoding.UTF8.GetBytes("ESAC")); // Identifier (Extrite Sparrow Animation Configuration)
                            writer.Write((ushort)1); // Version

                            // Padding
                            writer.Write(new byte[10]);

                            // Global Offset
                            writer.Write(sparrowAnimationPack.globalOffset.x);
                            writer.Write(sparrowAnimationPack.globalOffset.y);

                            // Write Animation Count
                            writer.Write(sparrowAnimationPack.animations.Length);

                            // Write Animations
                            foreach (Extrite.Animation animation in sparrowAnimationPack.animations)
                            {
                                writer.Write(animation.name.Length);
                                writer.Write(Encoding.UTF8.GetBytes(animation.name));
                                
                                writer.Write(animation.prefix.Length);
                                writer.Write(Encoding.UTF8.GetBytes(animation.prefix));

                                writer.Write(animation.fps);
                                writer.Write(animation.loop);
                                writer.Write(animation.offset.x);
                                writer.Write(animation.offset.y);
                            }
                        }
                    }
                }
            }   
        }
    
        public static SO_SparrowAnimationPack ImportSparrowAnimationPack(string path)
        {
            if (path.Length == 0)
            {
                return null;
            }

            SO_SparrowAnimationPack sparrowAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    // Load the texture
                    ZipArchiveEntry textureEntry = zipArchive.GetEntry("texture.png");
                    using (Stream stream = textureEntry.Open())
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            byte[] textureData = reader.ReadBytes((int)textureEntry.Length);
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(textureData);
                            sparrowAnimationPack.texture = texture;
                        }
                    }

                    // Load the atlas
                    ZipArchiveEntry atlasEntry = zipArchive.GetEntry("atlas.xml");
                    using (Stream stream = atlasEntry.Open())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string atlasData = reader.ReadToEnd();
                            sparrowAnimationPack.atlas = new TextAsset(atlasData);
                        }
                    }

                    // Load the animations configuration
                    ZipArchiveEntry binaryEntry = zipArchive.GetEntry("animations.esac");
                    using (Stream stream = binaryEntry.Open())
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            // Read Header
                            string identifier = Encoding.UTF8.GetString(reader.ReadBytes(4));
                            ushort version = reader.ReadUInt16();

                            // Padding
                            reader.ReadBytes(10);

                            // Global Offset
                            sparrowAnimationPack.globalOffset = new Vector2(reader.ReadSingle(), reader.ReadSingle());

                            // Read Animation Count
                            int animationCount = reader.ReadInt32();

                            // Read Animations
                            sparrowAnimationPack.animations = new Extrite.Animation[animationCount];
                            for (int i = 0; i < animationCount; i++)
                            {
                                Extrite.Animation animation = new Extrite.Animation();
                                animation.name = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
                                animation.prefix = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
                                animation.fps = reader.ReadInt32();
                                animation.loop = reader.ReadBoolean();
                                animation.offset = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                                sparrowAnimationPack.animations[i] = animation;
                            }
                        }
                    }
                }

                return sparrowAnimationPack;
            }
        }

        public static Texture2D GetTextureFromAnimationPack(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    // Load the texture
                    ZipArchiveEntry textureEntry = zipArchive.GetEntry("texture.png");
                    using (Stream stream = textureEntry.Open())
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            byte[] textureData = reader.ReadBytes((int)textureEntry.Length);
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(textureData);
                            return texture;
                        }
                    }
                }
            }
        }

        public static TextAsset GetTextAssetFromAnimationPack(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    // Load the atlas
                    ZipArchiveEntry atlasEntry = zipArchive.GetEntry("atlas.xml");
                    using (Stream stream = atlasEntry.Open())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string atlasData = reader.ReadToEnd();
                            return new TextAsset(atlasData);
                        }
                    }
                }
            }
        }
    
        public static string GetPathFromDialogue(string title, string extension, bool save)
        {
            string path = "";
            
            #if UNITY_EDITOR
                if (save)
                {
                    path = UnityEditor.EditorUtility.SaveFilePanel(title, "", "", extension);
                }
                else
                {
                    path = UnityEditor.EditorUtility.OpenFilePanel(title, "", extension);
                }
            #elif UNITY_STANDALONE && !UNITY_STANDALONE_OSX
                if (save)
                {
                    path = StandaloneFileBrowser.SaveFilePanel(title, "", "", extension);
                }
                else
                {
                    path = StandaloneFileBrowser.OpenFilePanel(title, "", extension, false);
                }
            #elif UNITY_STANDALONE_OSX
                Debug.LogError("File dialogue not supported on this platform");
            #endif

            return path;
        }
    }
}
