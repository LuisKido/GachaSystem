using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GachaSystem.BLL
{
    public class UserBLL
    {
        AmazonDynamoDBClient client; 

        public UserBLL() {

            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1 // Reemplaza con la región de tu tabla
            };

            client = new AmazonDynamoDBClient(config);
        }
        public async Task CreateUser(string userId, string userName, string email)
        {
            var request = new PutItemRequest
            {
                TableName = "User",
                Item = new Dictionary<string, AttributeValue>()
            {
                { "UserId", new AttributeValue { S = userId } },
                { "UserName", new AttributeValue { S = userName } },
                { "Email", new AttributeValue { S = email } },
                { "CreationDate", new AttributeValue { S = DateTime.UtcNow.ToString() } },
                { "LastLoginDate", new AttributeValue { S = DateTime.UtcNow.ToString() } },
                { "LastUpdateTime", new AttributeValue { S = DateTime.UtcNow.ToString() } },
                { "TotalScore", new AttributeValue { N = "0" } },
            }
            };

            var response = await client.PutItemAsync(request);

            // Manejar la respuesta según sea necesario
        }

        public async Task AddGachaTicketToUser(string userId, int ticketCount)
        {
            var request = new UpdateItemRequest
            {
                TableName = "User",
                Key = new Dictionary<string, AttributeValue>() { { "UserId", new AttributeValue { S = userId } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
        {
            {"#T", "GachaTickets"},
        },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {":inc", new AttributeValue {N = ticketCount.ToString()}},
            {":zero", new AttributeValue {N = "0"}},
        },
                ReturnValues = ReturnValue.UPDATED_NEW,
                UpdateExpression = "SET #T = if_not_exists(#T, :zero) + :inc"
            };

            var response = await client.UpdateItemAsync(request);

            // Manejar la respuesta según sea necesario
        }
    }
}
