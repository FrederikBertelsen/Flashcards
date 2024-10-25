using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Exceptions
{
    public static class ExceptionHandler
    {
        private const bool PrintExceptions = true;

        public static async Task<ActionResult> HandleAsync(Func<Task<ActionResult>> func)
        {
            try
            {
                return await func();
            }
            catch (ModelNotFoundException e)
            {
                if (PrintExceptions)
                    Console.WriteLine("Model not found: " + e.Message);

                return new NotFoundObjectResult(e.Message);
            }
            catch (ModelAlreadyExistsException e)
            {
                if (PrintExceptions)
                    Console.WriteLine("Model already exists: " + e.Message);
                
                return new ConflictObjectResult(e.Message);
            }
            catch (ArgumentException e)
            {
                if (PrintExceptions)
                    Console.WriteLine("Argument exception: " + e.Message);
                
                return new BadRequestObjectResult(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                if (PrintExceptions)
                    Console.WriteLine("Unauthorized access: " + e.Message);
                
                return new UnauthorizedObjectResult(e.Message);
            }
            catch (Exception e)
            {
                if (PrintExceptions)
                    Console.WriteLine("Internal server error: " + e.Message);
                
                return new ObjectResult(e.Message) { StatusCode = 500 };
            }
        }
    }
}