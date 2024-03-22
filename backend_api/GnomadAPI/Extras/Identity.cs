/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Holds the functions for identity
*
************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace TravelCompanionAPI.Extras
{
    public class Identity
    {
        public string Token
        {
            get; private set;
        }

        public Identity(string token)
        {
            Token = token;
        }

        public async Task<bool> validateAsync()
        {
            var payload = await VerifyGoogleTokenId(Token);

            if (payload == null)
            {
                return false;
            }

            return true;
    }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenId(string token)
        {
            try
            {
                // uncomment these lines if you want to add settings: 
                // var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                // { 
                //     Audience = new string[] { "yourServerClientIdFromGoogleConsole.apps.googleusercontent.com" }
                // };
                // Add your settings and then get the payload
                // GoogleJsonWebSignature.Payload payload =  await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                // Or Get the payload without settings.
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);

                return payload;
            }
            catch (Exception)
            {
                Console.WriteLine("invalid google token");
            }
            return null;
        }
    }
}
