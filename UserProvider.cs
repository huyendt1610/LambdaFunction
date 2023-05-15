using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaDemo
{
    public class UserProvider
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public UserProvider(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }
        public async Task<User[]> GetUsers()
        {
            var rs = await _amazonDynamoDB.ScanAsync(new ScanRequest()
            {
                TableName = "user-table"
            });

            if (rs != null && rs.Items != null) {
                var users = new List<User>();
                foreach (var user in rs.Items)
                {
                    user.TryGetValue("city", out var city);//case-sensitive
                    user.TryGetValue("email", out var email);
                    user.TryGetValue("address", out var address);
                    user.TryGetValue("phone", out var phone);

                    users.Add(new User() { 
                        City = city?.S, 
                        Email = email?.S,
                        Address = address?.S,
                        Phone = address?.S,
                    });
                }
                return users.ToArray();
            }
            return Array.Empty<User>();
        }
    }
}
