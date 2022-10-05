﻿using IdentityModel.Client;
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