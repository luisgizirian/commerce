// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Common;

public interface ICmmrcClient
{
    Task<string> TestConnection();
}

public class CmmrcClient : ICmmrcClient, IDisposable
{
    private readonly SshClient _tunnel;
    private readonly ForwardedPortLocal _port;
    private bool disposedValue;
    private readonly ILogger<CmmrcClient> _logger;

    public CmmrcClient(ILogger<CmmrcClient> logger)
    {
        _logger = logger;
        _tunnel = new SshClient("<replace-me>", "<replace-me>", "<replace-me>");
        _tunnel.Connect();

        _port = new ForwardedPortLocal("localhost", 10000, "<replace-me>", 5202);
        _tunnel.AddForwardedPort(_port);
        _logger.LogInformation("Connected to tunnel");

        _port.Exception += delegate(object sender, ExceptionEventArgs e)
        {
            _logger.LogError(e.Exception, "Exception in port forwarding");
        };
        _port.Start();
    }

    public async Task<string> TestConnection()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:10000/");
        var response = await client.PostAsync("/h/orders?message={ssh-content:true}", new StringContent("{ssh-content:true}"));
        _logger.LogInformation("Response from server: {0}", response.StatusCode);
        return await response.Content.ReadAsStringAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _logger.LogInformation("Disposing tunnel");
                // TODO: dispose managed state (managed objects)
                _port.Stop();
                _tunnel.Disconnect();
                _port.Dispose();
                _tunnel.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~CmmrcClient()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}