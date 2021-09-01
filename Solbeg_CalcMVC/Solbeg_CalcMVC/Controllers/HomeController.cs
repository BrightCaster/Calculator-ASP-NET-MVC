using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Solbeg_CalcMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Solbeg_CalcMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        public IActionResult Result()
        {
            return View("Index");
        }
        
        [HttpPost]
        public IActionResult Result(string arg, string Reset)
        {
            

            if (Reset != "C") // Если нажата кнопка Cancel
            {
                if (!String.IsNullOrEmpty(arg))// проверка на пустую строчку
                {
                    ModelsCalc.Arg = arg;
                    string result = null;
                    char operations = '0';
                    int indexOperations = 0;
                    double final;
                    for (int i = 0; i < arg.Length; i++)// поиск индекса и тип оператора в какой либо позиции
                    {
                        if (arg[i] == '+' | arg[i] == '-' | arg[i] == '*' | arg[i] == '/')
                        {
                            operations = arg[i];// оператор
                            indexOperations = i;// индекс
                            break;// как только нашли, выходим из дальнейнего поиска (это на случай когда попадётся
                                  // 4+/+12)
                        }
                    }
                    if (indexOperations == 0) indexOperations = arg.Length;// Для вывода числа, если не был введён оператор
                    for (int i = 0; i < indexOperations; i++)
                    {
                        result += arg[i];// подсчёт числа стоящего до оператора
                    }
                    final = Convert.ToDouble(result);// запись
                    result = null;
                    for (int i = arg.Length - 1; i > indexOperations; i--)//подсчёт числа задом наперёд второго числа после оператора
                    {
                        if (operations == '0') break;// если нету оператора, выходим (для числа без оператора)
                        result += arg[i];
                    }
                    if (result != null) result = new string(result.Reverse<char>().ToArray());// Если нет оператора, то просто число
                    //если есть число, то переворачиваем массив в правильном положении
                    else goto Next;
                    string result1 = null;
                    for(int i = 0; i < result.Length; i++)// из за повторов операторов, мы включаем здесь такой отсев лишних операторов
                    {
                        if (result[i] == '+' | result[i] == '-' | result[i] == '*' | result[i] == '/') continue;
                        else result1 += result[i];
                    }
                    result = result1;
                    
                    switch (operations)
                    {
                        case '+':
                            final = final + Convert.ToDouble(result);
                            break;
                        case '-':
                            final = final - Convert.ToDouble(result);
                            break;
                        case '*':
                            final = final * Convert.ToDouble(result);
                            break;
                        case '/':
                            final = final / Convert.ToDouble(result);
                            break;
                    }
                    Next:
                    ModelsCalc.Arg = final.ToString();
                    return View("Index", ModelsCalc.Arg);
                }
                else return View("Index", ModelsCalc.Arg);
            }
            else
            {
                ModelsCalc.Arg = "";
                return View("Index", ModelsCalc.Arg);
            }
            
        }
        [HttpPost]
        public IActionResult Result1(string digit)
        {
            ModelsCalc.Arg += digit.ToString();// для вывода чисел и знаков(обычная вставка в виде строки)
           
            return View("Index", ModelsCalc.Arg);
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
