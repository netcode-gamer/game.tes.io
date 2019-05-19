﻿using Gamer.Core;
using Gamer.Proxy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gamer.Estate.Ultima
{
    public static class UltimaExtensions
    {
        public static UltimaGame ToUltimaGame(this Uri uri, out ProxySink proxySink, out string[] filePaths)
        {
            filePaths = null;
            // game
            var fragment = uri.Scheme == "game" ? uri.Host : uri.Fragment?.Substring(1);
            var gameName = Enum.GetNames(typeof(UltimaGame)).FirstOrDefault(x => string.Equals(x, fragment, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentOutOfRangeException(nameof(uri), uri.ToString());
            var game = (UltimaGame)Enum.Parse(typeof(UltimaGame), gameName);
            // scheme
            proxySink = uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps
                ? new ProxySinkClient(uri)
                : new ProxySink();
            return game;
        }

        public static Task<IDataPack> GetUltimaDataPackAsync(this Uri uri)
        {
            var game = uri.ToUltimaGame(out var proxySink, out var filePaths);
            return Task.FromResult((IDataPack)new UltimaDataPack((uint)game));
        }
    }
}