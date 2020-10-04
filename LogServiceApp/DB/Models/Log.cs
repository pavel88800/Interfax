using System;

namespace DB.Models
{
    public class Log
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Дата записи
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Кол-во занимаемой памяти
        /// </summary>
        public long MemorySize { get; set; }
    }
}