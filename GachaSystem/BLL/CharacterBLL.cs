using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using GachaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GachaSystem.BLL
{
    public class CharacterBLL
    {

        AmazonDynamoDBClient client;

        public CharacterBLL() {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1 // Reemplaza con la región de tu tabla
            };

            client = new AmazonDynamoDBClient(config);

        }
        public async Task CreateCharacter(Character character)
        {

            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1 // Reemplaza con la región de tu tabla
            };

            client = new AmazonDynamoDBClient(config);
            var table = Table.LoadTable(client, "Character");

            var item = new Amazon.DynamoDBv2.DocumentModel.Document();
            item["Name"] = character.Name;
            item["Rarity"] = character.Rarity;
            // Añade otros campos aquí

            await table.PutItemAsync(item);
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1 // Reemplaza con la región de tu tabla
            };

            var client = new AmazonDynamoDBClient(config);
            var table = Table.LoadTable(client, "Character");

            var scanOptions = new ScanOperationConfig();
            var results = table.Scan(scanOptions);

            List<Amazon.DynamoDBv2.DocumentModel.Document> data = await results.GetNextSetAsync();

            return data.Select(doc => new Character
            {
                Name = doc["Name"],
                Rarity = (int)doc["Rarity"]
                // Agrega otros campos aquí
            }).ToList();
        }

        public async Task AddCharacterToUser(string userId, string characterName)
        {
            // Obtén el usuario actual
            var getItemRequest = new GetItemRequest
            {
                TableName = "User",
                Key = new Dictionary<string, AttributeValue>() { { "UserId", new AttributeValue { S = userId } } }
            };

            var getItemResult = await client.GetItemAsync(getItemRequest);

            // Verificar si el usuario tiene suficientes tickets
            int currentTicketCount = 0;
            if (getItemResult.Item.ContainsKey("GachaTickets"))
            {
                currentTicketCount = int.Parse(getItemResult.Item["GachaTickets"].N);
            }

            if (currentTicketCount < 20)
            {
                // El usuario no tiene suficientes tickets, devuelve un error
                return;
                //throw new Exception("Not enough gacha tickets");
            }

            // Decrementa el contador de tickets
            var updateTicketsRequest = new UpdateItemRequest
            {
                TableName = "User",
                Key = new Dictionary<string, AttributeValue>() { { "UserId", new AttributeValue { S = userId } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
        {
            {"#T", "GachaTickets"},
        },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {":dec", new AttributeValue {N = "20"}},
        },
                ReturnValues = ReturnValue.UPDATED_NEW,
                UpdateExpression = "SET #T = #T - :dec"
            };

            await client.UpdateItemAsync(updateTicketsRequest);


            //var getItemRequest = new GetItemRequest
            //{
            //    TableName = "User",
            //    Key = new Dictionary<string, AttributeValue>() { { "UserId", new AttributeValue { S = userId } } }
            //};

            //var getItemResult = await client.GetItemAsync(getItemRequest);

            // Verificar si el campo Characters existe
            if (!getItemResult.Item.ContainsKey("Characters"))
            {
                // El campo Characters no existe, crea un nuevo mapa y agrega el primer personaje
                var putItemRequest = new PutItemRequest
                {
                    TableName = "User",
                    Item = getItemResult.Item
                };
                putItemRequest.Item["Characters"] = new AttributeValue
                {
                    M = new Dictionary<string, AttributeValue>
            {
                { characterName, new AttributeValue { N = "1" } }
            }
                };
                await client.PutItemAsync(putItemRequest);
            }
            else
            {
                // El campo Characters ya existe, agrega el nuevo personaje
                var updateItemRequest = new UpdateItemRequest
                {
                    TableName = "User",
                    Key = new Dictionary<string, AttributeValue>() { { "UserId", new AttributeValue { S = userId } } },
                    ExpressionAttributeNames = new Dictionary<string, string>()
            {
                {"#C", "Characters"},
                {"#N", characterName},
            },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                {":inc", new AttributeValue {N = "1"}},
                {":zero", new AttributeValue {N = "0"}},
            },
                    ReturnValues = ReturnValue.UPDATED_NEW,
                    UpdateExpression = "SET #C.#N = if_not_exists(#C.#N, :zero) + :inc"
                };
                await client.UpdateItemAsync(updateItemRequest);
            }

        }

    }
}

