using Calculadora.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// prepara a Calculadora para ser apresentada, pela primeira vez, ao utilizador
        /// </summary>
        /// <returns></returns>
        [HttpGet]  // este anotador é facultativo
        public IActionResult Index()
        {

            // preparar os dados a serem enviados para a View na primeira interação
            ViewBag.Visor = "0";

            return View();
        }


        /// <summary>
        /// prepara a Calculadora na segunda interação, e seguintes
        /// </summary>
        /// <param name="botao">valor do botão pressionado pelo utilizador, ao usar a calculadora</param>
        /// <param name="visor">valor do Visor da calculadora</param>
        /// <param name="primeiroOperando">na operação algébrica, valor do 1º operando</param>
        /// <param name="operador">operação a ser executada</param>
        /// <param name="limpaEcra">indica se o Visor deve ser, ou não, reiniciado</param>
        /// <returns></returns>
        [HttpPost] // só qd o formulário for submetido em 'post' ele será acionado
        public IActionResult Index(
           string botao,
           string visor,
           string primeiroOperando,
           string operador,
           string limpaEcra
           )
        {

            // testar valor do 'botao'
            switch (botao)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    // pressionei um algarismo
                    if (limpaEcra == "sim" || visor == "0") { visor = botao; }
                    else { visor = visor + botao; }
                    limpaEcra = "nao";
                    // desafio: fazer em modo algébrico esta operação...
                    break;

                case ",":
                    // foi pressionado ','
                    if (!visor.Contains(',')) visor += ',';
                    break;

                case "+/-":
                    // vamos inverter o valor do 'visor'
                    if (visor.StartsWith('-')) visor = visor.Substring(1);
                    else visor = '-' + visor;
                    // sugestao: fazer de forma algebrica
                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                    // foi pressionado um operador
                    if (!string.IsNullOrEmpty(operador))
                    {
                        // como já é, pelo menos, a segunda vez que pressionamos um operador
                        // agora, temos mesmo de fazer a operação algébrica
                        double operandoUm = Convert.ToDouble(primeiroOperando);
                        double operandoDois = Convert.ToDouble(visor);

                        // var. auxiliar
                        double resultado = 0;

                        switch (operador)
                        {
                            case "+":
                                resultado = operandoUm + operandoDois;
                                break;
                            case "-":
                                resultado = operandoUm - operandoDois;
                                break;
                            case "x":
                                resultado = operandoUm * operandoDois;
                                break;
                            case ":":
                                resultado = operandoUm / operandoDois;
                                break;
                        }

                        visor = resultado + "";
                    }

                    primeiroOperando = visor;
                    operador = botao;
                    limpaEcra = "sim";

                    break;

                case "=":
                    // agora, temos mesmo de fazer a operação algébrica
                    double op1 = Convert.ToDouble(primeiroOperando);
                    double op2 = Convert.ToDouble(visor);

                    //var auxiliar
                    double result = 0;

                    switch (operador)
                    {
                        case "+":
                            result = op1 + op2;
                            break;
                        case "-":
                            result = op1 - op2;
                            break;
                        case "x":
                            result = op1 * op2;
                            break;
                        case ":":
                            result = op1 / op2;
                            break;
                    }

                    visor = result + "";
                    primeiroOperando = visor;
                    operador = "";
                    limpaEcra = "sim";
                    break;
                case "C":
                    visor = "0";
                    primeiroOperando = "";
                    operador = "";
                    limpaEcra = "sim";
                    break;

            }

            // preparar dados para serem enviados à View
            ViewBag.Visor = visor;
            ViewBag.PrimeiroOperando = primeiroOperando;
            ViewBag.Operador = operador;
            ViewBag.LimpaEcra = limpaEcra;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}