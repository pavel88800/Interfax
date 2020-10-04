using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DB;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class LoggingDb
    {
        /// <summary>
        ///     Записать данные в БД
        /// </summary>
        /// <param name="dt">Дата</param>
        /// <param name="mSize">память в байтах</param>
        /// <returns>
        ///     <see cref="Task" />
        /// </returns>
        public async Task WriteDateAsync(DateTime dt, long mSize)
        {
            using (var db = new LogServiceContext())
            {
                var log = new Log
                {
                    Date = dt,
                    MemorySize = mSize
                };

                await db.AddAsync(log);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Получить данные из БД
        /// </summary>
        /// <returns>List<Log></returns>
        public async Task<List<Log>> Get()
        {
            List<Log> result;
            using (var db = new LogServiceContext())
            {
                result = await db.LogsTable.ToListAsync();
            }

            return result;
        }
    }
}