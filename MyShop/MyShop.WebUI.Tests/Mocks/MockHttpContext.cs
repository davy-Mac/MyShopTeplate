using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase // HttpContextBase used as a base to implement and override its methods
    {
        private MockRequest request;
        private MockResponse response;
        private HttpCookieCollection cookies; // creates a private collection of cookies

        public MockHttpContext() // constructor that initializes objects needed
        {
            cookies = new HttpCookieCollection(); // initializes cookie collection
            this.request = new MockRequest(cookies); // collection of cookies is injected 
            this.response = new MockResponse(cookies);
        }

        public override HttpRequestBase Request
        {
            get { return request; }
        }

        public override HttpResponseBase Response
        {
            get { return response; }
        }
    }

    public class MockRequest : HttpRequestBase // implements HttpRequestBase to override its methods
    {
        private readonly HttpCookieCollection cookies; // creates a private collection of cookies

        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }

    public class MockResponse : HttpResponseBase // implements HttpResponseBase and override its methods
    {
        private readonly HttpCookieCollection cookies; // creates a private collection of cookies

        public MockResponse(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get{
                return cookies;
            }
        }
    }
}
