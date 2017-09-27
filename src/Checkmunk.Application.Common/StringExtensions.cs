using LeadPipe.Net.Extensions;
using System;

namespace Checkmunk.Application.Common
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string text)
        {
            if (text.IsNullOrEmpty()) return Guid.Empty;

            var guid = Guid.Empty;

            Guid.TryParse(text, out guid);

            return guid;
        }
    }
}