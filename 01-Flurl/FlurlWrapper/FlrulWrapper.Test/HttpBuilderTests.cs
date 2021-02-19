using FluentAssertions;
using Flurl;
using System.Collections.Generic;
using Xunit;

namespace FlrulWrapper.Test
{
    public class HttpBuilderTests
    {
        private const string UrlBase = "https://www.minhaurl.com.br";

        [Fact]
        public void DeveCriarUrlAdicionandoCaminho()
        {
            const string UrlFinal = UrlBase + "/meucaminho";

            var url = UrlBase.AppendPathSegment("meucaminho");

            url.ToString().Should().Be(UrlFinal);
        }

        [Fact]
        public void DeveCriarUrlAdicionandoCaminhosPorLista()
        {
            var caminhosUrl = new List<string>
            {
                "painelPrincipal",
                "centralDoCliente",
                "minhasConfiguracoes"
            };

            const string UrlFinal = UrlBase + "/painelPrincipal/centralDoCliente/minhasConfiguracoes";

            var url = UrlBase.AppendPathSegments(caminhosUrl);

            url.ToString().Should().Be(UrlFinal);
        }

        [Fact]
        public void DeveCriarUrlComParametrosDeSelecao()
        {
            const string UrlFinal = UrlBase + "?FiltroPrincipal=Nome&PessoaAtiva=Sim";

            var url = UrlBase.SetQueryParams(new
            {
                FiltroPrincipal = "Nome",
                PessoaAtiva = "Sim"
            });

            url.ToString().Should().Be(UrlFinal);
        }

        [Theory()]
        [InlineData("/ger%C3%A1l", "gerál")]
        [InlineData("/inicio%20trabalho", "inicio trabalho")]
        public void DeveCriarUrlComCaminhoEncoded(string encodedPart, string toEncodePart)
        {
            var url = UrlBase.AppendPathSegment(toEncodePart, true);

            url.ToString().Should().Be(UrlBase + encodedPart);
        }

        [Fact]
        public void DeveCriarUrlComCaminhoEParametrosDeSelecao()
        {
            const string UrlFinal = UrlBase + "/painelPrincipal/?Filtro=Todos";

            var url = UrlBase.AppendPathSegments("painelPrincipal", "")
                             .SetQueryParam("Filtro", "Todos");

            url.ToString().Should().Be(UrlFinal);
        }

        [Fact]
        public void DeveCriarUrlComFragmento()
        {
            const string UrlFinal = UrlBase + "/textoLivre.md#modern-implementation";

            var url = UrlBase.AppendPathSegment("textoLivre.md")
                             .SetFragment("modern-implementation");

            url.ToString().Should().Be(UrlFinal);
        }

        [Fact]
        public void DeveDecodificarUrl()
        {
            const string UrlReferencia = "https://user:pass@www.mysite.com:1234/with/path?x=1&y=2#foo";

            var url = new Url(UrlReferencia);

            url.Scheme.Should().Be("https");
            url.UserInfo.Should().Be("user:pass");
            url.Host.Should().Be("www.mysite.com");
            url.Port.Should().Be(1234);
            url.Authority.Should().Be("user:pass@www.mysite.com:1234");
            url.Root.Should().Be("https://user:pass@www.mysite.com:1234");
            url.Path.Should().Be("/with/path");
            url.Query.Should().Be("x=1&y=2");
            url.Fragment.Should().Be("foo");
        }
    }
}