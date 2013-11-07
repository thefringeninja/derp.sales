using System;
using System.Linq;
using System.Text;
using Nancy.Helpers;
using Nancy.Testing;

namespace Derp.Sales.Tests.Fixtures
{
    public abstract class UserAgent
    {
        protected readonly string Path;
        public Action<BrowserContext> OnContext;

        protected UserAgent(string path)
        {
            Path = path;
        }

        public static UserAgent Delete(string path)
        {
            return new DeleteRequest(path);
        }

        public static UserAgent Put(string path)
        {
            return new PutRequest(path);
        }

        public static UserAgent Post(string path)
        {
            return new PostRequest(path);
        }

        public static UserAgent Get(string path)
        {
            return new GetRequest(path);
        }

        public BrowserResponse Execute(Browser browser)
        {
            try
            {
                var executeInternal = ExecuteInternal(browser);
                return executeInternal;
            }
            catch
            {
                
            }
            return new BrowserResponse(null, browser);
        }

        protected abstract BrowserResponse ExecuteInternal(Browser browser);

        public override string ToString()
        {
            var context = new BrowserContext();

            var onContext = OnContext;
            if (onContext != null)
            {
                onContext(context);
            }

            IBrowserContextValues values = context;

            var builder = new StringBuilder()
                .Append(GetMethod()).Append(" ").Append(Path);

            if (false == String.IsNullOrEmpty(values.QueryString))
            {
                builder.Append(values.QueryString);
            }

            builder.AppendLine();

            builder = values.Headers.Aggregate(
                builder, (sb, header) => header.Value.Aggregate(
                    sb, (b, value) => b.Append(header.Key).Append(": ").Append(value).AppendLine()));

            if (false == String.IsNullOrEmpty(values.FormValues))
            {
                var body = HttpUtility.ParseQueryString(values.FormValues);
                builder = body.AllKeys.Aggregate(
                    builder.AppendLine(),
                    (sb, key) => sb.Append(key).Append(": ").Append(body[key]).AppendLine());
            }
            else
            {
                builder.Append(values.BodyString);
            }

            return builder.ToString();
        }

        private string GetMethod()
        {
            var name = GetType().Name;
            var method = name.Substring(0, name.Length - "Request".Length).ToUpper();
            return method;
        }

        #region Nested type: DeleteRequest

        private class DeleteRequest : UserAgent
        {
            public DeleteRequest(string path)
                : base(path)
            {
            }

            protected override BrowserResponse ExecuteInternal(Browser browser)
            {
                return browser.Delete(Path, OnContext);
            }
        }

        #endregion

        #region Nested type: GetRequest

        private class GetRequest : UserAgent
        {
            public GetRequest(string path)
                : base(path)
            {
            }

            protected override BrowserResponse ExecuteInternal(Browser browser)
            {
                return browser.Get(Path, OnContext);
            }
        }

        #endregion

        #region Nested type: PostRequest

        private class PostRequest : UserAgent
        {
            public PostRequest(string path) : base(path)
            {
            }

            protected override BrowserResponse ExecuteInternal(Browser browser)
            {
                return browser.Post(Path, OnContext);
            }
        }

        #endregion

        #region Nested type: PutRequest

        private class PutRequest : UserAgent
        {
            public PutRequest(string path)
                : base(path)
            {
            }

            protected override BrowserResponse ExecuteInternal(Browser browser)
            {
                return browser.Put(Path, OnContext);
            }
        }

        #endregion
    }
}