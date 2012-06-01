using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using redis_sharp.server;

namespace redis_sharp_test.server
{
    [TestFixture]
    public class RedisRequestTest
    {

        [Test]
        public void ShouldAskToReadFourBytesInTheBeggining()
        {
            var redisRequest = new RedisRequest(null);

            Assert.That(redisRequest.IsComplete(), Is.False);
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));
        }

        [Test]
        public void ShouldReadFirstRequestLineTillCrLfAsNumberOfLinesInTheCommand()
        {
            var redisRequest = new RedisRequest(null);
            
            redisRequest.AddData("*42\r");
            Assert.That(redisRequest.NumberOfCommands, Is.EqualTo(0));
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));
            Assert.That(redisRequest.IsComplete(), Is.False);

            redisRequest.AddData("\n$36");
            Assert.That(redisRequest.NumberOfCommands, Is.EqualTo(42));
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));
            Assert.That(redisRequest.IsComplete(), Is.False);
        }

        [Test]
        public void ShouldParseTheNextLineAsNumberOfBytesToRead()
        {
            var redisRequest = new RedisRequest(null);

            redisRequest.AddData("*4\r\n");
            redisRequest.AddData("$36\r");
            redisRequest.AddData("\nPIN");

            Assert.That(redisRequest.NumberOfCommands, Is.EqualTo(4));
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(35));
            Assert.That(redisRequest.IsComplete(), Is.False);
        }

        [Test]
        public void ShouldReadCommandTextLineAndSetupToReadNextCommandLength()
        {
            var redisRequest = new RedisRequest(null);

            redisRequest.AddData("*4\r\n");
            redisRequest.AddData("$5\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(7));

            redisRequest.AddData("LPUSH\r\n");
            Assert.That(redisRequest.IsComplete(), Is.False);
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));
            Assert.That(redisRequest.Command, Is.EqualTo("LPUSH"));
            Assert.That(redisRequest.Args, Is.Empty);
        }

        [Test]
        public void ShouldCompleteWhenAllCommandsAreRead()
        {
            var redisRequest = new RedisRequest(null);
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("*4\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("$6\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(8));

            redisRequest.AddData("LRANGE\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("$10\r");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("\nABC");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(9));

            redisRequest.AddData("DEFGHIJ\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("$1\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(3));

            redisRequest.AddData("0\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("$2\r\n");
            Assert.That(redisRequest.NumberOfBytesToRead, Is.EqualTo(4));

            redisRequest.AddData("20\r\n");
            Assert.That(redisRequest.IsComplete(), Is.True);
            Assert.That(redisRequest.Command, Is.EqualTo("LRANGE"));
            Assert.That(redisRequest.Args.Count, Is.EqualTo(3));
            Assert.That(redisRequest.Args[0], Is.EqualTo("ABCDEFGHIJ"));
            Assert.That(redisRequest.Args[1], Is.EqualTo("0"));
            Assert.That(redisRequest.Args[2], Is.EqualTo("20"));
        }


    }
}