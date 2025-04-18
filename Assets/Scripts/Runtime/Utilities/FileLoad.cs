using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace Incredimaker
{
    public class FileLoad
    {
        
        /// <summary>
        /// Get an AudioClip from a given path
        /// </summary>
        public static AudioClip GetAudioClipFromPath(string path)
        {
            AudioClip audioClip;
            AudioType audioType;

            // Check the file extension
            string extension = Path.GetExtension(path);
            switch (extension.ToLower())
            {
                case ".wav":
                    audioType = AudioType.WAV;
                    break;
                case ".mp3":
                    audioType = AudioType.MPEG;
                    break;
                case ".ogg":
                    audioType = AudioType.OGGVORBIS;
                    break;
                default:
                    Debug.LogError("Unsupported audio format: " + extension);
                    return null;
            }

            // Load the audio file
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
            {
                var asyncOperation = www.SendWebRequest();
                while (!asyncOperation.isDone)
                {
                    // Wait for the request to complete
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to load audio clip: " + www.error);
                    return null;
                }

                audioClip = DownloadHandlerAudioClip.GetContent(www);
                return audioClip;
            }
        }
        
    }
}
