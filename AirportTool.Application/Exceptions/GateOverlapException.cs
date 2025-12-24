namespace AirportTool.Application.Exceptions
{
    public class GateOverlapException : Exception
    {
        public GateOverlapException(string? message) : base(message)
        {
        }
    }
}

