using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GachaSystem.Models;

namespace GachaSystem.BLL
{
    public class GachaSystemBLL
    {
        private List<Character> characters;  // La lista de todos los personajes posibles
        private Random random;


        public GachaSystemBLL()
        {
            random = new Random();
        }

        public GachaSystemBLL(List<Character> characters)
        {
            this.characters = characters;
            random = new Random();
        }

        public Character Spin()
        {
            int totalRarity = characters.Sum(character => character.Rarity);
            int spin = random.Next(1, totalRarity + 1);

            int cumulativeRarity = 0;
            foreach (Character character in characters)
            {
                cumulativeRarity += character.Rarity;
                if (spin <= cumulativeRarity)
                {
                    return character;
                }
            }

            // Este punto no debería ser alcanzado
            return null;
        }

        public async Task<Character> SpinGacha()
        {
            // Recupera la lista de todos los personajes posibles de la base de datos
            List<Character> characters = await new CharacterBLL().GetAllCharacters();

            // Crea una instancia del sistema gacha con la lista de personajes
            GachaSystemBLL gacha = new GachaSystemBLL(characters);

            // Realiza un giro y devuelve el resultado
            return gacha.Spin();
        }
    }

}
