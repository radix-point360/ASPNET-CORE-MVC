
using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;

namespace Login_Using_Middleware.CustomMiddleware
{
    // You can create custom middleware in ASP.NET Core without using IMiddleware.
    public class LoginMiddleware : IMiddleware
    {
        // RequestDelegate next refers to the next middleware component in the HTTP request pipeline.
        // It's a delegate that processes the incoming HttpContext.
        //private readonly RequestDelegate _next;

        //public LoginMiddleware(RequestDelegate next)
        //{
        //    _next = next;
        //}

        // The middleware type must contain a public method named Invoke or InvokeAsync.
        // Invoke and InvokeAsync are the two method signatures ASP.NET Core looks for when executing custom middleware.
        // They define how your middleware processes HTTP requests.
        // You must implement one of them for your middleware to work.
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Method == "POST" && context.Request.Path == "/")
            {
                string? UserName = "", password = "";

                // Stream could be contain anything: JSON, XML, plain text, binary data.
                // StreamReader is used to read characters from a byte stream.
                // e.g. reading text files line by line or in full. It's part of the System.IO namespace
                StreamReader readBody = new StreamReader(context.Request.Body);
                string body = await readBody.ReadToEndAsync();

                // QueryHelpers.ParseQuery - It parses a query string (like ?UserName=Admin&password=Admin123) into a dictionary of key-value pairs.
                // ParseQuery - It converts the string into a dictionary.
                // StringValues - a flexible type that can hold one or more values for a key (e.g., "John" or ["red", "blue"])
                Dictionary<string,StringValues> bodyDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

                // Checking and validating UserName
                if (context.Request.Query.ContainsKey("UserName"))
                {
                    UserName = bodyDict["UserName"][0];

                    if (string.IsNullOrEmpty(UserName)) 
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Please enter User Name!");
                    }

                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("UserName not provided!");
                }

                // Checking and validating Password
                if (context.Request.Query.ContainsKey("password"))
                {
                    password = bodyDict["password"][0];

                    if (string.IsNullOrEmpty(password))
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Please enter password!");
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Password not provided!");
                }

                // Authenticate using UserName and Password

                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(password))
                {
                    string validUserName = "Admin", validPasssword = "#Admin@123!";
                    bool isAuthencated = false;

                    if(validUserName == UserName && validPasssword == password)
                    {
                        isAuthencated = true;
                    }

                    if (isAuthencated)
                    {
                        await context.Response.WriteAsync("Login successful!");
                    }

                    if (!isAuthencated)
                    {
                        await context.Response.WriteAsync("Login failed!");
                    }
                }


            }
            else
            {
               await next(context);
            }
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtention
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
           return builder.UseMiddleware<LoginMiddleware>();
        }
    }

}
