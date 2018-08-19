using System;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace LogstashTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("ElasticsearchDirect")]
        public ActionResult ElasticsearchDirect()
        {
            using (var log = new LoggerConfiguration().WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))).CreateLogger())
            {
                var loggingDirectlyToEs = "Logging directly to Elasticsearch";
                log.Information(loggingDirectlyToEs);
                log.Warning(loggingDirectlyToEs);
                log.Error(loggingDirectlyToEs);
            }

            return Ok();
        }

        [HttpGet]
        [Route("ElasticsearchViaDockerProvider")]
        public ActionResult ElasticsearchViaDockerProvider()
        {
            using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
            {
                const string loggingToEsViaDockerProvider = "Logging to Elasticsearch via docker provider";
                log.Information(loggingToEsViaDockerProvider);
                log.Warning(loggingToEsViaDockerProvider);
                log.Error(loggingToEsViaDockerProvider);
            }

            return Ok();
        }

        [HttpGet]
        [Route("LogstashDirect")]
        public ActionResult LogstashDirect()
        {
            using (var log = new LoggerConfiguration().WriteTo.LogstashHttp("https://elk-host:8443").CreateLogger())
            {
                const string loggingDirectlyToLs = "Logging directly to Logstash";
                log.Information(loggingDirectlyToLs);
                log.Warning(loggingDirectlyToLs);
                log.Error(loggingDirectlyToLs);
            }

            return Ok();
        }

        [HttpGet]
        [Route("LogstashViaDockerProvider")]
        public ActionResult LogstashViaDockerProvider()
        {
            using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
            {
                const string loggingToLsViaDockerProvider = "Logging to Logstash via docker provider";
                log.Information(loggingToLsViaDockerProvider);
                log.Warning(loggingToLsViaDockerProvider);
                log.Error(loggingToLsViaDockerProvider);
            }

            return Ok();
        }
    }
}
