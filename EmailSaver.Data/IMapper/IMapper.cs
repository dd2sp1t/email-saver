using System;
using System.Data.SqlClient;

namespace EmailSaver.Data
{
    internal interface IMapper<out TItem>
    {
        TItem ReadIteam(SqlDataReader reader);
    }
}