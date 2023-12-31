﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GumbyBot.Services
{
	[Service]
	public class AsyncHttpWebRequest
	{
		readonly HttpClient _client;

		public AsyncHttpWebRequest()
		{
			_client = new();

			ProductInfoHeaderValue product = new("GumbyBot", "1.0");
			_client.DefaultRequestHeaders.UserAgent.Add(product);
		}

		public async Task<string> String(HttpMethod method, string url)
		{ 
			HttpRequestMessage request = new(method, url);
			using HttpResponseMessage response = await _client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
	}
}
