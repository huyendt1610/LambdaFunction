using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaDemo
{
    public class UserCreator : IUserCreator
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public UserCreator(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }
        public async Task<bool> CreateUser(User user)
        {
            var request = new PutItemRequest()
            {
                TableName = "user-table",
                Item = new Dictionary<string, AttributeValue>() {
                    { "city", new AttributeValue{S = user.City }},
                    { "email", new AttributeValue(user.Email) },
                    { "address", new AttributeValue(user.Address) },
                    { "phone", new AttributeValue(user.Phone) }
                }
            };
            var rs = await _amazonDynamoDB.PutItemAsync(request);

            return rs?.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
