using System;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.Elasticsearch;

namespace LogstashTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public const string FLAT_FORMAT = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Properties}{NewLine}";

        // GET api/values
        [HttpGet]
        [Route("f")]
        public ActionResult File()
        {
            Log(lc => lc.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, outputTemplate: FLAT_FORMAT), "Logging directly to File");
            return Ok();
        }

        // GET api/values
        [HttpGet]
        [Route("es")]
        public ActionResult ElasticsearchDirect()
        {
            Log(lc => lc.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))), "Logging directly to Elasticsearch");
            return Ok();
        }

        [HttpGet]
        [Route("es2")]
        public ActionResult ElasticsearchViaDockerProvider()
        {
            Log(lc => lc.WriteTo.Console(outputTemplate: FLAT_FORMAT), "Logging to Elasticsearch via docker provider");
            return Ok();
        }

        [HttpGet]
        [Route("ls")]
        public ActionResult LogstashDirect()
        {
            Log(lc => lc.WriteTo.LogstashHttp("https://elk-host:8443"), "Logging directly to Logstash");
            return Ok();
        }

        [HttpGet]
        [Route("ls2")]
        public ActionResult LogstashViaDockerProvider()
        {
            Log(lc => lc.WriteTo.Console(outputTemplate: FLAT_FORMAT), "Logging to Logstash via docker provider");
            return Ok();
        }

        private void Log(Func<LoggerConfiguration, LoggerConfiguration> func, string message)
        {
            var loggerCoonfiguration = new LoggerConfiguration().Enrich.FromLogContext();
            using (var log = func.Invoke(loggerCoonfiguration).CreateLogger())
            {
                using (LogContext.PushProperty("conversationId", Guid.NewGuid()))
                {
                    log.Information(message);
                    log.Warning(message);
                    log.Error(message);
                }
            }
        }
    }
}
