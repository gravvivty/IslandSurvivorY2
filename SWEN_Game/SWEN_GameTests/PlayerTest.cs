using System;
using Xunit;
using SWEN_Game;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using System.Runtime.ConstrainedExecution;
using SWEN_Game._Entities;
using SWEN_Game._Items;

namespace SWEN_GameTests
{
    public class PlayerTest
    {
        //Überprüft, ob der Player nach der Erstellung korrekte Standardwerte hat.
        [Fact]
        public void Player_Initialization_DefaultValues() 
        {
            var playerData = new PlayerGameData();
            var player = new Player(PlayerGameData.Instance);
            Assert.Equal(130f, player.Speed);
            Assert.Equal(new Vector2(450, 450), player.Position);
            Assert.Equal(new Vector2(456, 458), player.RealPos);
        }
        //Ob die Methode SetPosition die Positionen korrekt aktualisiert
        [Fact]
        public void SetPosition_UpdatesPositionAndRealPos()
        {
            var playerData = new PlayerGameData();
            var player = new Player(PlayerGameData.Instance);
            player.SetPosition(new Vector2(200, 200));
            Assert.Equal(new Vector2(200, 200), player.Position);
            Assert.Equal(new Vector2(206, 208), player.RealPos);
        }
        //Ob die Richtung (z. B. Bewegung nach rechts) korrekt gesetzt und abgefragt wird.
        [Fact]
        public void SetDirection_GetDirection_Works()
        {
            var playerData = new PlayerGameData();
            var player = new Player(PlayerGameData.Instance);
            var direction = new Vector2(1, 0); 
            player.SetDirection(direction);
            Assert.Equal(direction, player.Direction);
        }
    }
}