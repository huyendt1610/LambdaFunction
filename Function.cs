using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaDemo;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var name = "Empty";
        if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("name"))
        {
            name = request.QueryStringParameters["name"];
        }

        if (request.HttpMethod == "POST")
        {  //should be seperate to 2 lambda functions instead of doing this here
            //do something
        }

        /* read data from dynamodb
        var provider = new UserProvider(new AmazonDynamoDBClient()); //can use with DI 
        var users = await provider.GetUsers();

        context.Logger.Log($"Get name {0}");
        return new APIGatewayProxyResponse() { 
            StatusCode = 200,
            Body = JsonConvert.SerializeObject(users)

        };*/

        //insert data from request 
        var user = JsonConvert.DeserializeObject<User>(request.Body);
        if (user == null) { return new APIGatewayProxyResponse() { StatusCode = 400 }; }; //bad request 
        var creator = new UserCreator(new AmazonDynamoDBClient());
        var rs = await creator.CreateUser(user);
        if (!rs) { return new APIGatewayProxyResponse() { StatusCode = 400 }; }; //bad request 
        return new APIGatewayProxyResponse()
        {
            StatusCode = 200
        };

    }
}
