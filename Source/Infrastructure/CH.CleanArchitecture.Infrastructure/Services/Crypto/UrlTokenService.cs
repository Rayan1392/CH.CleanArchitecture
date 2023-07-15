﻿using CH.CleanArchitecture.Common;
using CH.CleanArchitecture.Core.Application;

namespace CH.CleanArchitecture.Infrastructure.Services
{
    public class UrlTokenService : IUrlTokenService
    {
        private readonly IJWTService _jwtService;
        private const int TOKEN_LIFETIME_DAYS = 7;

        public UrlTokenService(IJWTService jwtService) {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Creates a safe url token containing the given payload
        /// Creates a JWT from the provided payload, encrypted with random salt and URL encoded in Base64
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public string CreateSafeUrlToken(object payload) {
            var jwt = _jwtService.CreateJWT(payload, TOKEN_LIFETIME_DAYS);
            string cipheredJwt = StringCipherHelper.EncryptWithRandomSalt(jwt);
            return cipheredJwt.Base64UrlEncode();
        }

        /// <summary>
        /// Reads a safe url token and returns the containing payload
        /// Reads and validates a token created by this service and returns the underlying payload
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public T ReadSafeUrlToken<T>(string token) {
            string decodedToken = token.Base64UrlDecode();
            string decipheredToken = StringCipherHelper.DecryptWithRandomSalt(decodedToken);
            return _jwtService.ReadJWT<T>(decipheredToken);
        }
    }
}
