public class ApiSettings
{
    public string EndpointUrl { get; set; }
    public ApiSettings()
    {
        EndpointUrl = "http://localhost:8000/kafka";
    }
}