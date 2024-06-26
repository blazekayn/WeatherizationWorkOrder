﻿using Microsoft.Data.SqlClient;

namespace WeatherizationWorkOrder.Data
{
    public static class ReaderExtensions
    {
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
        }
    }
}
