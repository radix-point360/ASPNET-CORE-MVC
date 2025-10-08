var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) => {
    if (context.Request.Method == "GET" && context.Request.Path == "/")
    {
        int? firstNumber = 0, secondNumber = 0;
        string? calculate = null;
        long? result = 0;

        //Read and validate firstNumber 
        if (context.Request.Query.ContainsKey("firstNumber")){
            //query string always sends parameteres as strings.
            string? firstNumberString = context.Request.Query["firstNumber"][0];

            if (!string.IsNullOrEmpty(firstNumberString)) { 
                firstNumber = Convert.ToInt32(firstNumberString);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid First Operand \n");
                return;
            }
        }
        else
        {
            if(context.Response.StatusCode == 200)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid First Operand \n");
                return;
            }
        }

        //read and validate secondNumber
        if (context.Request.Query.ContainsKey("secondNumber"))
        {
            string? secondNumberString = context.Request.Query["secondNumber"];

            if (!string.IsNullOrEmpty(secondNumberString))
            {
                secondNumber = Convert.ToInt32(secondNumberString);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid second operand");
                return;
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Please provide valid operand \n");
                return;
            }
        }

        // read and validate operator
        if (context.Request.Query.ContainsKey("calculate"))
        {
             calculate = context.Request.Query["calculate"][0];

            if (string.IsNullOrEmpty(calculate))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Operator not provided!");
                return;
            }

            // calculate value
            switch (calculate)
            {
                case "add":
                    result = firstNumber + secondNumber;
                    break;
                case "subs":
                    result = firstNumber - secondNumber;
                    break;
                case "mul":
                    result = firstNumber * secondNumber;
                    break;
                case "devide":
                    result = firstNumber / secondNumber;
                    break;
                case "mod":
                    result = firstNumber % secondNumber;
                    break;
                default:
                    await context.Response.WriteAsync("Please provide a valid operator! \n Valid Operators: 1) add \t 2) subs \n Valid Operators: 3) mul \t 4) devide \n Valid Operators: 5) mod");
                    break;
            }

            if (result.HasValue)
            {
                await context.Response.WriteAsync(result.Value.ToString());
            }
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Operator not provided!");
        }
    }
});

app.Run();