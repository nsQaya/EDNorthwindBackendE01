using Core.Entities.Concrete;
using Core.Extentions;
using Core.Utilities.Security.Encyption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        DateTime _accessTokenExpretions;

        public JwtHelper(IConfiguration configuration)
        {
            //IConfiguration configuration = new ConfigurationBuilder()
            //    .SetbasePath()
            //    .AddJsonFile()
            //    .Build();

            Configuration = configuration;
            _tokenOptions = new TokenOptions { SecurityKey = "" 
            ,Issuer=""
            ,Audience=""
            ,AccessTokenExpiration=15};
            //Configuration.GetSection(key: "TokenOptions").Bind <TokenOptions>();
            //_tokenOptions = Configuration.GetSection("TokenOptions")
            _accessTokenExpretions = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }

        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            
            var securityKey = SecurityKeyHelper.CreateSecurtiKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt= CreateJwtSecurtyToken(_tokenOptions,user,signingCredentials,operationClaims);

            var jwtSecurtyTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurtyTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpretions,
            };


        }
        public JwtSecurityToken CreateJwtSecurtyToken(
            TokenOptions tokenOptions,
            User user,
            SigningCredentials signingCredentials,
            List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer:_tokenOptions.Issuer,
                audience:_tokenOptions.Audience,
                expires: _accessTokenExpretions,
                notBefore:DateTime.Now,
                claims:SetClaims(user,operationClaims),
                signingCredentials:signingCredentials
                );
            return jwt;
        }
        public IEnumerable<Claim> SetClaims(User user,List<OperationClaim> operationClaims )
        {
            var claims= new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }
    }
}
