using Service;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate next = next;
    private readonly ILogger<ErrorHandlingMiddleware> logger = logger;

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the request.");

            if (ex is FluentValidation.ValidationException validationException)
            {
                // Use a dictionary where the property name is the key and a list of error messages is the value
                var propertyErrors = validationException.Errors
                    .GroupBy(e => e.PropertyName.ToLower())  // Group by the property name
                    .ToDictionary(
                        group => group.Key, 
                        group => group.Select(e => e.ErrorMessage).ToArray()  // Concatenate all error messages for a property
                    );

                ctx.Response.StatusCode = 400;  // Bad Request
                await ctx.Response.WriteAsJsonAsync(
                    new
                    {
                        message = validationException.Message,  // General error message
                        errors = propertyErrors  // Detailed property validation errors
                    }
                );
            }
            else if (ex is AppError apiError)
            {
                ctx.Response.StatusCode = apiError switch
                {
                    NotFoundError => 404,
                    UnauthorizedError => 401,
                    ForbiddenError => 403,
                    ValidationError => 400,
                    _ => 500,
                };
                if (apiError is ValidationError)
                {
                    await ctx.Response.WriteAsJsonAsync(
                        new
                        {
                            message = apiError.Message,
                            errors = (apiError as ValidationError)?.Errors
                        }
                    );
                }
                else
                {
                    await ctx.Response.WriteAsJsonAsync(new { error = apiError.Message });
                }
            }
            else
            {
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
            }
        }
    }
}
