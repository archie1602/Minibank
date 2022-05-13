namespace Minibank.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        public readonly RequestDelegate _next;

        public CustomAuthenticationMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var payload = GetPayloadFromBearerToken(context.Request.Headers);

            if (payload is not null)
            {
                var expDate = GetExpirationTimeFromPayload(payload);
                var currentDate = DateTime.Now.ToLocalTime();

                //var currentDateH = currentDate.AddHours(1);


                //Console.Write("\n\n\n");
                //Console.WriteLine("===============EXPIRATION TOKEN TIME===============");

                //Console.WriteLine("Current Time: " + currentDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                //Console.WriteLine("Current TimeH: " + currentDateH.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                //Console.WriteLine("Expiration Time: " + expDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"));

                //Console.WriteLine($"Inequality: {currentDate < expDate}");
                //Console.WriteLine($"Inequality + hour: {currentDateH < expDate}");

                //Console.Write("\n\n\n");

                if (currentDate >= expDate)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    
                    var msg = "JWT token has expired!";
                    await context.Response.WriteAsJsonAsync(new { msg });

                    // interrupt middleware pipeline, because JWT token has expired
                    return;
                }
            }

            await _next(context);
        }

        private DateTime GetExpirationTimeFromPayload(string payload)
        {
            var jsonDoc = JsonDocument.Parse(payload);

            var expTime = jsonDoc.RootElement
                .GetProperty("exp")
                .GetInt64();

            return ConvertUnixTimeToDateTime(expTime);
        }

        private DateTime ConvertUnixTimeToDateTime(long unixTime) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTime)
                .ToLocalTime();

        private string GetPayloadFromBearerToken(IHeaderDictionary headerDict)
        {
            // read Authorization header
            if (headerDict.TryGetValue("Authorization", out var authHeader))
            {
                // read bearer token value
                var token = authHeader.ToString().Substring(7);

                var tokenPartsBase64 = token.Split('.');

                if (tokenPartsBase64.Length != 3)
                    return null;

                return DecodeBase64JWT(tokenPartsBase64[1]);
            }

            return null;
        }

        private string DecodeBase64JWT(string base64Str)
            => Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));

        //var tmp = base64Str
        //    .Replace('-', '+')  // '-' -> '+'
        //    .Replace('_', '/'); // '_' -> '/'

        //private void ReadAuthorizationHeader(IHeaderDictionary headerDict)
        //{
        //    // Authorization
        //    if (headerDict.TryGetValue("Authorization", out var authHeader))
        //    {
        //        var bearerToken = authHeader.ToString();

        //        Console.Write("\n\n\n");
        //        Console.WriteLine("===============Authorization HEADER===============");

        //        Console.WriteLine(bearerToken);

        //        Console.Write("\n\n\n");
        //    }
        //}

        //private void PrintAllHeaders(IHeaderDictionary headerDict)
        //{
        //    var requestHeaders = new Dictionary<string, string>();

        //    foreach (var header in headerDict)
        //    {
        //        requestHeaders.Add(header.Key, header.Value);
        //    }

        //    Console.Write("\n\n\n");
        //    Console.WriteLine("===============HEADERS===============");

        //    foreach (var header in requestHeaders)
        //        Console.WriteLine($"{header.Key}: {header.Value}");

        //    Console.Write("\n\n\n");
        //}
    }
}