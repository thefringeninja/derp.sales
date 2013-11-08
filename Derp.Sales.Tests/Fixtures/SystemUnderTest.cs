using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Derp.Sales.Messaging;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;

namespace Derp.Sales.Tests.Fixtures
{
    public class SystemUnderTest
    {
        public const string ViewModelKey = "###ViewModel###";
        public const string BusKey = "###Bus###";
        public const string MessagesKey = "###ResultingIn###";

        private readonly BrowserResponse browserResponse;

        public SystemUnderTest(BrowserResponse response)
        {
            browserResponse = response;
        }

        public SystemResponse Response
        {
            get { return new SystemResponse(browserResponse); }
        }

        private IEnumerable<Message> Dispatched()
        {
            object dispatched;
            Response.Context.Items.TryGetValue(MessagesKey, out dispatched);

            return (dispatched as IEnumerable<Message>) ?? Enumerable.Empty<Message>();
        }

        public bool DidNotChangeAnything()
        {
            return false == Dispatched().Any();
        }

        public bool Does(params Func<object>[] does)
        {
            var shouldHaveDone = does.Select(ToMessage).ToList();
            var did = Dispatched().ToList();
            return shouldHaveDone.SequenceEqual(did, MessageEqualityComparer.Instance);
        }

        private static Message ToMessage(Func<object> factory)
        {
            dynamic messageCandidate = factory();

            try
            {
                return (Command) messageCandidate;
            }
            catch
            {
            }
            try
            {
                return (Event) messageCandidate;
            }
            catch
            {
            }
            return new CouldNotConvertToMessageMessage(messageCandidate);
        }


        public override string ToString()
        {
            var responseBuilder =
                new StringBuilder().Append("HTTP/1.1 ")
                                   .Append((int) browserResponse.StatusCode)
                                   .Append(' ')
                                   .Append(browserResponse.StatusCode.ToString().Titleize())
                                   .AppendLine();
            responseBuilder = browserResponse.Headers.Aggregate(
                responseBuilder,
                (builder, header) =>
                    builder.Append(header.Key)
                           .Append(": ")
                           .Append(header.Value)
                           .AppendLine())
                           .Append(browserResponse.Body.AsString());


            return responseBuilder.ToString();
        }

        #region Nested type: SystemResponse

        public class SystemResponse
        {
            private readonly BrowserResponse browserResponse;

            public SystemResponse(BrowserResponse response)
            {
                browserResponse = response;
            }

            public HttpStatusCode StatusCode
            {
                get { return browserResponse.StatusCode; }
            }

            public IDictionary<string, string> Headers
            {
                get { return browserResponse.Headers; }
            }

            public NancyContext Context
            {
                get { return browserResponse.Context; }
            }

            public TViewModel ViewModel<TViewModel>() where TViewModel : class
            {
                object model;
                browserResponse.Context.Items.TryGetValue(ViewModelKey, out model);
                if (model == null)
                {
                    model = JsonConvert.DeserializeObject<TViewModel>(browserResponse.Body.AsString());
                }
                return model as TViewModel;
            }
        }

        #endregion
    }

    class CouldNotConvertToMessageMessage : Message
    {
        private readonly object messageCandidate;
        public Guid MessageId { get; private set; }

        public CouldNotConvertToMessageMessage(object messageCandidate)
        {
            this.messageCandidate = messageCandidate;
        }

        public override string ToString()
        {
            return String.Format("Could not covert {0} to a message.", messageCandidate);
        }
    }
}
