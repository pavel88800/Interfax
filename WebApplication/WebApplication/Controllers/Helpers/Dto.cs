using System.Collections.Generic;

namespace WebApplication.Controllers.Helpers
{
    internal class Dto
    {
        /// <summary>
        ///     Коллекция логов
        /// </summary>
        public List<Log> Logs { get; set; }

        /// <summary>
        ///     Куда пишется
        /// </summary>
        public string Writing { get; set; }
    }
}