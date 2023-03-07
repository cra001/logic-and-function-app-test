using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Azure.Identity;
using Azure.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace narratech.datahub.azure.functions
{
    public static class TokenFetchFunction2
    {
        [FunctionName("TokenFetchFunction2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string clientId =  req.Query["clientId"];
            string clientSecret =  req.Query["clientSecret"];
            string tenantId =  req.Query["tenantId"];
            string authUri = "https://login.microsoftonline.com" + "/" + tenantId;

            var context = new AuthenticationContext(authUri);
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var token = await context.AcquireTokenAsync("https://database.windows.net/", clientCredential);

            string responseMessage = string.IsNullOrEmpty(token.AccessToken)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {token.AccessToken}. This HTTP triggered function executed successfully.";

            var result = new OkObjectResult(responseMessage);
            return result;
        }

        private static async Task<string> GetAzureSqlAccessToken() {
            // See https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-sql
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net" });
            var tokenRequestResult = await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext);
            return tokenRequestResult.Token;
        }
    }
}
