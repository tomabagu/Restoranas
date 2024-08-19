using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restoranas.Interfaces;

namespace Restoranas.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string _filePath;

        public LoggerService(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            try
            {
                File.AppendAllText(_filePath, message + Environment.NewLine);

            }
            catch (Exception)
            {
                throw new Exception(message);
            }
        }
    }
}
