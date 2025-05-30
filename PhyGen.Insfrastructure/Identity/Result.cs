
namespace PhyGen.Insfrastructure.Identity
{
    public class Result
    {
        public bool Succeeded { get; private set; }
        public string Error { get; private set; }

        public static Result Success(string token) => new Result { Succeeded = true };
        public static Result Failure(string error) => new Result { Succeeded = false, Error = error };

      
    }

   

}