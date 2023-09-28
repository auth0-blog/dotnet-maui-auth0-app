using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;

namespace MauiAuth0App.Auth0;

public class WebBrowserAuthenticator : IdentityModel.OidcClient.Browser.IBrowser
{
  public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
  {
    try
    {
      WebAuthenticatorResult result = await WebAuthenticator.Default.AuthenticateAsync(
          new Uri(options.StartUrl),
          new Uri(options.EndUrl));

      var url = new RequestUrl(options.EndUrl)
          .Create(new Parameters(result.Properties));

      // Workaround for Facebook's issue (https://stackoverflow.com/questions/7131909/facebook-callback-appends-to-return-url)
      if (url.EndsWith("%23_%3D_"))
      {
        url = url.Substring(0, url.LastIndexOf("%23_%3D_"));
      }

      return new BrowserResult
      {
        Response = url,
        ResultType = BrowserResultType.Success
      };
    }
    catch (TaskCanceledException)
    {
      return new BrowserResult
      {
        ResultType = BrowserResultType.UserCancel,
        ErrorDescription = "Login canceled by the user."
      };
    }
  }
}