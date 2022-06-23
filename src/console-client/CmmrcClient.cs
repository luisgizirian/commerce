// See https://aka.ms/new-console-template for more information
using Renci.SshNet;
using Renci.SshNet.Common;

public class CmmrcClient : IDisposable
{
    private readonly SshClient _tunnel;
    private readonly ForwardedPortLocal _port;
    private bool disposedValue;

    public CmmrcClient()
    {
        _tunnel = new SshClient("<replace-me>", "<replace-me>", "<replece-me>");
        _tunnel.Connect();

        _port = new ForwardedPortLocal("localhost", 10000, "<replace-me>", 5202);
        _tunnel.AddForwardedPort(_port);

        _port.Exception += delegate(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        };
        _port.Start();
    }

    public async Task<string> TestConnection()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:10000/");
        var response = await client.PostAsync("/h/orders?message={ssh-content:true}", new StringContent("{ssh-content:true}"));

        return await response.Content.ReadAsStringAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
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