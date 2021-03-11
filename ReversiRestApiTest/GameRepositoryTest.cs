using NUnit.Framework;
using ReversiRestApi.Model;
using System.Linq;

namespace ReversiRestApiTest
{
    [TestFixture]
    public class GameRepositoryTest
    {
        [Test]
        public void AddSpel_AddNieuwSpel_CheckSpellen(){
            // Arrange
            GameRepository spelRepository = new GameRepository();
            Game spel4 = new Game();

            // Act
            spelRepository.AddGame(spel4);

            // Assert
            Assert.That(spelRepository.GetGames().Any(x => x == spel4));
        }

        [Test]
        public void GetSpel_AddNieuwSpelMetToken_ReturnNieuwSpel()
        {
            // Arrange
            GameRepository spelRepository = new GameRepository();
            Game spel4 = new Game();
            spel4.Token = "test";
            spelRepository.AddGame(spel4);

            // Act
            var actual = spelRepository.GetGame("test");

            // Assert
            Assert.AreEqual(spel4, actual);
        }

        [Test]
        public void GetSpellen_AddNieuwSpel_ReturnSpellen()
        {
            // Arrange
            GameRepository spelRepository = new GameRepository();
            Game spel4 = new Game();
            spelRepository.AddGame(spel4);

            // Act
            var actual = spelRepository.GetGames();

            // Assert
            Assert.AreNotEqual(0, actual.Count());
        }
    }
}
