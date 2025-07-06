using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SWEN_Game._Utils;

namespace SWEN_Game._Sound
{
    public class SFXManager
    {
        private static SFXManager _instance;
        public static SFXManager Instance => _instance ??= new SFXManager();

        private readonly HashSet<string> pitchVariedSounds = new() { "enemyHit", "pistolShoot", "akShoot", "revolverShoot", "precisionShoot", "blunderbussShoot" };
        private readonly HashSet<string> noCooldownSounds = new() { "uiSelect", "uiConfirm", "pistolShoot", "akShoot", "revolverShoot", "precisionShoot", "blunderbussShoot" };

        private readonly Random random = new();

        private Dictionary<string, SoundEffect> _soundEffects;

        private Dictionary<string, float> lastPlayedTime = new();

        private float pitchVariance = 0.2f; // +/- range
        private float volume = 1f;
        private float cooldown = 0.05f; // 50ms cooldown per SFX

        public float GetVolume() => volume;

        public void SetVolume(float newVolume)
        {
            volume = MathHelper.Clamp(newVolume, 0f, 1f);
        }

        private SFXManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadSounds(ContentManager content, Dictionary<string, string> soundsToLoad)
        {
            foreach (var sound in soundsToLoad)
            {
                if (!_soundEffects.ContainsKey(sound.Key))
                {
                    _soundEffects[sound.Key] = content.Load<SoundEffect>(sound.Value);
                }
            }
        }

        public void Play(string name, float basePitch = 0f, float pan = 0f)
        {
            if (!_soundEffects.TryGetValue(name, out var sfx))
            {
                System.Diagnostics.Debug.WriteLine($"[SFXManager] Sound '{name}' not found.");
                return;
            }

            float currentTime = Globals.TotalGameTime;

            // Only check cooldown for sounds not in the whitelist
            if (!noCooldownSounds.Contains(name) &&
                lastPlayedTime.TryGetValue(name, out var lastTime) &&
                currentTime - lastTime < cooldown)
            {
                return;
            }

            float finalPitch = basePitch;

            if (pitchVariedSounds.Contains(name))
            {
                float randomPitchOffset = (float)(random.NextDouble() * 2 * pitchVariance - pitchVariance);
                finalPitch = MathHelper.Clamp(basePitch + randomPitchOffset, -1f, 1f);
            }

            sfx.Play(this.volume, finalPitch, pan);
            lastPlayedTime[name] = currentTime;
        }

        public void ResetCooldowns()
        {
            lastPlayedTime.Clear();
        }
    }
}
