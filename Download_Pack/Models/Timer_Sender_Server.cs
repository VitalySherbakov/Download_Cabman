using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Download_Pack.Models
{
    public class Timer_Sender_Server
    {
        private readonly Timer _timer;
        private readonly TimeSpan _time_send;
        private readonly MethodSend _method;
        private readonly MethodPC _methodPc;
        private readonly bool _FlagTesting=false;
        private TimeSpan _time_ticket { get; set; }
        public delegate void MethodSend();
        public delegate void MethodPC();

        /// <summary>
        /// Вход Данних
        /// </summary>
        /// <param name="TimerSend">Таймер Визова Функции Method</param>
        /// <param name="Method">Функция Вызова</param>
        /// <param name="TestingFlag">Тестинг</param>
        /// <param name="PerformancePCMethod">Функция Диагностики Памяти</param>
        public Timer_Sender_Server(TimeSpan TimerSend, MethodSend Method , bool TestingFlag=false, MethodPC PerformancePCMethod=null)
        {
            this._time_send = TimerSend;
            this._time_ticket= TimerSend;
            this._method = Method;
            this._FlagTesting = TestingFlag;
            this._methodPc = PerformancePCMethod;
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            
        }

        /// <summary>
        /// Сикунди Виполнения
        /// </summary>
        public int Second;
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Second++;
            if (_FlagTesting)
            {
                Console.WriteLine($"Test Second: {Second} | {TimeSpan.FromSeconds(Second)} | {_time_send} | {_time_ticket}");
            }

            if (_methodPc != null)
            {
                this._methodPc();
            }
            if (TimeSpan.FromSeconds(Second)== _time_ticket)
            {
                this._method();
                _time_ticket = TimeSpan.FromSeconds(Second + _time_send.Seconds);
            }
        }

        /// <summary>
        /// Остановить
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }
        /// <summary>
        /// Запустить
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }
        /// <summary>
        /// Очистить Ресурс
        /// </summary>
        public void Clear()
        {
            _timer.Dispose();
        }
    }
}
