using Azure.Core;

public class AccessTokenCredential : TokenCredential
{
    private readonly AccessToken _accessToken;

    public AccessTokenCredential(AccessToken accessToken)
    {
        _accessToken = accessToken;
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return _accessToken;
    }

    public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_accessToken);
    }
}