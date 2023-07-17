using GachaSystem.BLL;
using GachaSystem.Models;
using Microsoft.VisualBasic.ApplicationServices;

namespace GachaSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CreateSampleCharacters();
        }

        public async void CreateSampleCharacters()
        {
            List<Character> sampleCharacters = new List<Character>
    {
        new Character { Name = "Yasuke", Rarity = 1 },
        new Character { Name = "Tomoe", Rarity = 2 },
        new Character { Name = "Jirou", Rarity = 10 },
        new Character { Name = "Kaiyo", Rarity = 5 },
        new Character { Name = "Isamu", Rarity = 20 }
    };

            foreach (Character character in sampleCharacters)
            {

                await new CharacterBLL().CreateCharacter(character);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Character result = await new GachaSystemBLL().SpinGacha();

            // Aquí es donde podrías agregar el personaje obtenido a la cuenta del jugador
            // Esto dependería de cómo estés manejando las cuentas de los jugadores y sus personajes
            lblGachaResult.Text = result.Name;
            await new CharacterBLL().AddCharacterToUser("2", result.Name);
            //Console.WriteLine($"Obtuvo: {result.Name}");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //await new UserBLL().CreateUser("2", "Kido2", "l.orostizaga@gmail.com");
            await new UserBLL().AddGachaTicketToUser("2", 100);
        }
    }

}