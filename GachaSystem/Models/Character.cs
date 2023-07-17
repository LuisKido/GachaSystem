using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GachaSystem.Models
{
    public class Character
    {
        [DynamoDBHashKey]
        public string Name { get; set; }
        public int Rarity { get; set; }  // Por ejemplo, 1 para común, 2 para raro, 3 para ultra raro, etc.
                                         // Otras propiedades de los personajes aquí
    }
}
