using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    /// <summary>
    /// Manages all weapons available in the game and initializes their attributes and textures.
    /// </summary>
    public class WeaponManager
    {
        private Dictionary<string, Weapon> _weapons;

        public WeaponManager()
        {
            _weapons = new Dictionary<string, Weapon>();
        }

        /// <summary>
        /// Loads textures and initializes the weapon data for all available weapons.
        /// Also sets the player's base and current weapon to the pistol by default.
        /// </summary>
        public void InitWeapons()
        {
            // Pistol
            Texture2D vanillaBulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/VanillaBullet");

            Texture2D pistolIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/pistol");
            Texture2D pistolIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/pistol");
            Weapon pistol = new Weapon(0.25f, 300f, 1f, 1, 1, 10f, 8, 1f, 0, vanillaBulletTexture, pistolIconTexture, pistolIngameTexture);
            _weapons.Add("Pistol", pistol);

            Texture2D assaultRifleIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/assault_rifle");
            Texture2D assaultIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/assault_rifle");
            Weapon assault_rifle = new Weapon(0.15f, 300f, 0.7f, 1, 1, 4f, 24, 1.5f, 0, vanillaBulletTexture, assaultRifleIconTexture, assaultIngameTexture);
            _weapons.Add("Assault_Rifle", assault_rifle);

            Texture2D precisionRifleIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/precision_rifle");
            Texture2D precisionRifleIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/precision_rifle");
            Weapon precision_rifle = new Weapon(0.4f, 500f, 0.7f, 1, 1, 25f, 4, 2f, 3, vanillaBulletTexture, precisionRifleIconTexture, precisionRifleIconTexture);
            _weapons.Add("Precision_Rifle", precision_rifle);

            // Get it twice cuz if not they have the same reference
            PlayerGameData.BaseWeapon = this.GetWeapon("Pistol");
            PlayerGameData.CurrentWeapon = this.GetWeapon("Pistol");
            PlayerGameData.UpdateWeaponGameData();
        }

        /// <summary>
        /// Retrieves a cloned copy of the weapon by its name.
        /// </summary>
        /// <param name="weaponName">The name of the weapon to retrieve.</param>
        /// <returns>A cloned <see cref="Weapon"/> instance if found; otherwise, null.</returns>
        public Weapon GetWeapon(string weaponName)
        {
            if (_weapons.TryGetValue(weaponName, out Weapon weapon))
            {
                return weapon.Clone();
            }

            // This should never return Null
            // If this returns Null. We are fucked.
            // We dont need exception handling ~ Nico

            // How about just a fallback to current base weapon? :eyes: ~ Steven
            return PlayerGameData.BaseWeapon;
        }
    }
}
