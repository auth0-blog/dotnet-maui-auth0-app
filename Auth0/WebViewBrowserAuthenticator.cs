using IdentityModel.OidcClient.Browser;

namespace MauiAuth0App;

public class WebViewBrowserAuthenticator : IdentityModel.OidcClient.Browser.IBrowser
{
  private readonly WebView _webView;

  public WebViewBrowserAuthenticator(WebView webView)
  {
    _webView = webView;
  }

  public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
  {
    var tcs = new TaskCompletionSource<BrowserResult>();

    _webView.Navigated += (sender, e) =>
    {
      if (e.Url.StartsWith(options.EndUrl))
      {
        _webView.WidthRequest = 0;
        _webView.HeightRequest = 0;
        if (tcs.Task.Status != TaskStatus.RanToCompletion)
        {
          tcs.SetResult(new BrowserResult
          {
            ResultType = BrowserResultType.Success,
            Response = e.Url.ToString()
          });
        }
      }

    };

    _webView.WidthRequest = 600;
    _webView.HeightRequest = 600;
    _webView.Source = new UrlWebViewSource { Url = options.StartUrl };

    return await tcs.Task;
  }
}
