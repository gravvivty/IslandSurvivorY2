using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SWEN_Game._Sound
{
    public class SongManager
    {
        private static SongManager _instance;
        public static SongManager Instance => _instance ??= new SongManager();

        private Dictionary<string, Song> _songs;
        private float volume = 0.3f;
        private string _currentSongName;

        private SongManager()
        {
            _songs = new Dictionary<string, Song>();
            MediaPlayer.IsRepeating = true; // Always loop music
        }

        public void LoadSongs(ContentManager content, Dictionary<string, string> songsToLoad)
        {
            foreach (var kvp in songsToLoad)
            {
                if (!_songs.ContainsKey(kvp.Key))
                {
                    _songs[kvp.Key] = content.Load<Song>(kvp.Value);
                }
            }
        }

        public void Play(string name, bool forceRestart = false)
        {
            if (_songs.TryGetValue(name, out var song))
            {
                if (_currentSongName != name || forceRestart || MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Volume = MathHelper.Clamp(this.volume, 0f, 1f);
                    MediaPlayer.Play(song);
                    _currentSongName = name;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[SongManager] Song '{name}' not found.");
            }
        }

        public void Stop()
        {
            MediaPlayer.Stop();
            _currentSongName = null;
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Resume()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }

        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
            this.volume = volume;
        }
    }
}