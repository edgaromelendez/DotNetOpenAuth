﻿//-----------------------------------------------------------------------
// <copyright file="WrapUtilities.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using DotNetOpenAuth.Messaging;

namespace DotNetOpenAuth.OAuthWrap {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Text;

	/// <summary>
	/// Some common utility methods for OAuth WRAP.
	/// </summary>
	internal static class WrapUtilities {
		/// <summary>
		/// Authorizes an HTTP request using an OAuth WRAP access token in an HTTP Authorization header.
		/// </summary>
		/// <param name="request">The request to authorize.</param>
		/// <param name="accessToken">The access token previously obtained from the Authorization Server.</param>
		internal static void AuthorizeWithOAuthWrap(this HttpWebRequest request, string accessToken) {
			Contract.Requires<ArgumentNullException>(request != null);
			Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(accessToken));
			request.Headers[HttpRequestHeader.Authorization] = string.Format(
				CultureInfo.InvariantCulture,
				Protocol.HttpAuthorizationHeaderFormat,
				accessToken);
		}

		internal static DotNetOpenAuth.OAuth.ChannelElements.IConsumerDescription GetClientOrThrow(this IAuthorizationServer authorizationServer, string clientIdentifier) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(clientIdentifier));
			Contract.Ensures(Contract.Result<DotNetOpenAuth.OAuth.ChannelElements.IConsumerDescription>() != null);

			try {
				return authorizationServer.GetClient(clientIdentifier);
			} catch (KeyNotFoundException ex) {
				throw ErrorUtilities.Wrap(ex, OAuth.OAuthStrings.ConsumerOrTokenSecretNotFound);
			} catch (ArgumentException ex) {
				throw ErrorUtilities.Wrap(ex, OAuth.OAuthStrings.ConsumerOrTokenSecretNotFound);
			}
		}
	}
}
