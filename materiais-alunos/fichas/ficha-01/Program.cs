// Ficha 1 - Exercicio 2
// Cada alinea resolvida de tres formas: foreach (sem LINQ), query syntax e extension methods

string[] ListaUm = { "C#", "Aprender C#", "ASP.NET MVC com C#",
                     "Entity Framework", "Bootstrap", "Identity",
                     "Lambda", "Delegates", "Linq", "POO com C# " };

string[] ListaDois = { "C#", "ASP.NET MVC", "Linq", "Lambda e C#" };

int[] Numeros = { 10, 23, 54, 77, 85, 12, 1, 4, 53 };



// ============================================================================
// a) Elementos da ListaUm ordenados por ordem alfabetica
// ============================================================================

// 1) foreach
// Sort() -> ordena a propria lista (altera-a), por ordem crescente
var aForeach = new List<string>();
foreach (var s in ListaUm) aForeach.Add(s);
aForeach.Sort();

// aForeach.Sort(); 
// aForeach.Reverse(); caso fosse descending, com o sort primeiro

// 2) query syntax
// orderby e ascendente por omissao (equivale a "orderby s ascending")
var aQuery = from s in ListaUm orderby s select s;
// para ordem inversa: from s in ListaUm orderby s descending select s

// 3) extension methods
// s => s e uma expressao lambda: recebe cada elemento (s) e devolve a chave de ordenacao (o proprio s)
var aExt = ListaUm.OrderBy(s => s);
// var aExt = ListaUm.OrderByDescending(s => s) caso fosse descending

Console.WriteLine("============================================================");
Console.WriteLine(" a) Elementos da ListaUm por ordem alfabetica");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var s in aForeach) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var s in aQuery) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var s in aExt) Console.Write(s + " | ");
Console.WriteLine("\n\n");



// ============================================================================
// b) Elementos da ListaUm com menos de seis carateres
// ============================================================================

// 1) foreach
// s.Length -> numero de carateres da string (e uma propriedade, nao leva parenteses)
var bForeach = new List<string>();
foreach (var s in ListaUm)
    if (s.Length < 6)
        bForeach.Add(s);

// 2) query syntax
var bQuery = from s in ListaUm where s.Length < 6 select s;

// 3) extension methods
var bExt = ListaUm.Where(s => s.Length < 6);

Console.WriteLine("============================================================");
Console.WriteLine(" b) Elementos da ListaUm com menos de 6 carateres");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var s in bForeach) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var s in bQuery) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var s in bExt) Console.Write(s + " | ");
Console.WriteLine("\n\n");



// ============================================================================
// c) Numero total de elementos da ListaUm que incluem "C#"
// ============================================================================

// 1) foreach
// s.Contains("C#") -> true se a string incluir o texto "C#" (distingue maiusculas de minusculas)
int cForeach = 0;
foreach (var s in ListaUm)
    if (s.Contains("C#"))
        cForeach++;

// 2) query syntax (Count nao existe em query syntax -> aplica-se ao resultado)
int cQuery = (from s in ListaUm where s.Contains("C#") select s).Count();

// 3) extension methods
// Count aceita diretamente a condicao, dispensando o Where antes: equivale a ListaUm.Where(...).Count()
int cExt = ListaUm.Count(s => s.Contains("C#"));

Console.WriteLine("============================================================");
Console.WriteLine(" c) Numero de elementos da ListaUm que incluem \"C#\"");
Console.WriteLine("============================================================");

Console.WriteLine("  Foreach:           " + cForeach);
Console.WriteLine("  Query syntax:      " + cQuery);
Console.WriteLine("  Extension methods: " + cExt);
Console.WriteLine("\n");



// ============================================================================
// d) Numero de palavras de cada elemento da ListaUm
// ============================================================================

// 1) foreach
// s.Trim()     -> remove os espacos no inicio e no fim da string
//                 (necessario em "POO com C# ": sem o Trim, o espaco final geraria uma "palavra" vazia)
// .Split(' ')  -> divide a string em partes pelo espaco e devolve um array (string[])
// .Length      -> numero de elementos do array, ou seja, o numero de palavras
var dForeach = new List<int>();
foreach (var s in ListaUm)
    dForeach.Add(s.Trim().Split(' ').Length);

// 2) query syntax
// select pode devolver algo diferente do elemento original (aqui um int em vez da string)
var dQuery = from s in ListaUm select s.Trim().Split(' ').Length;

// 3) extension methods
var dExt = ListaUm.Select(s => s.Trim().Split(' ').Length);

Console.WriteLine("============================================================");
Console.WriteLine(" d) Numero de palavras de cada elemento da ListaUm");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var n in dForeach) Console.Write(n + " ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var n in dQuery) Console.Write(n + " ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var n in dExt) Console.Write(n + " ");
Console.WriteLine("\n\n");



// ============================================================================
// e) Media dos elementos de Numeros
// ============================================================================

// 1) foreach
int soma = 0;
foreach (var n in Numeros) soma += n;
// (double) -> converte a soma antes de dividir; sem isto seria divisao inteira e perdiam-se as casas decimais
double eForeach = (double)soma / Numeros.Length;

// 2) query syntax (Average nao existe em query syntax -> aplica-se ao resultado)
double eQuery = (from n in Numeros select n).Average();

// 3) extension methods
double eExt = Numeros.Average();

Console.WriteLine("============================================================");
Console.WriteLine(" e) Media dos elementos de Numeros");
Console.WriteLine("============================================================");

// ToString("F2") -> formata o numero com 2 casas decimais
Console.WriteLine("  Foreach:           " + eForeach.ToString("F2"));
Console.WriteLine("  Query syntax:      " + eQuery.ToString("F2"));
Console.WriteLine("  Extension methods: " + eExt.ToString("F2"));
Console.WriteLine("\n");



// ============================================================================
// f) Valor maximo de Numeros
// ============================================================================

// 1) foreach
int fForeach = Numeros[0];
foreach (var n in Numeros)
    if (n > fForeach)
        fForeach = n;

// 2) query syntax (Max nao existe em query syntax -> aplica-se ao resultado)
int fQuery = (from n in Numeros select n).Max();

// 3) extension methods
int fExt = Numeros.Max();

Console.WriteLine("============================================================");
Console.WriteLine(" f) Valor maximo de Numeros");
Console.WriteLine("============================================================");

Console.WriteLine("  Foreach:           " + fForeach);
Console.WriteLine("  Query syntax:      " + fQuery);
Console.WriteLine("  Extension methods: " + fExt);
Console.WriteLine("\n");



// ============================================================================
// g) Elementos de Numeros no intervalo [1, 25], por ordem decrescente
// ============================================================================

// 1) foreach
var gForeach = new List<int>();
foreach (var n in Numeros)
    if (n >= 1 && n <= 25)
        gForeach.Add(n);
gForeach.Sort();     // ordena por ordem crescente
gForeach.Reverse();  // inverte a lista -> fica por ordem decrescente

// 2) query syntax
var gQuery = from n in Numeros where n >= 1 && n <= 25 orderby n descending select n;

// 3) extension methods
// os metodos encadeiam-se: o OrderByDescending recebe o resultado do Where
var gExt = Numeros.Where(n => n >= 1 && n <= 25).OrderByDescending(n => n);

Console.WriteLine("============================================================");
Console.WriteLine(" g) Numeros no intervalo [1, 25] por ordem decrescente");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var n in gForeach) Console.Write(n + " ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var n in gQuery) Console.Write(n + " ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var n in gExt) Console.Write(n + " ");
Console.WriteLine("\n\n");



// ============================================================================
// h) Intersecao da ListaUm com a ListaDois
// ============================================================================

// 1) foreach
// hForeach.Contains(x) -> aqui e o Contains da List: verifica se o elemento ja esta na lista (evita repetidos)
var hForeach = new List<string>();
foreach (var x in ListaUm)
    foreach (var y in ListaDois)
        if (x == y && !hForeach.Contains(x))
            hForeach.Add(x);

// 2) query syntax (Intersect nao existe em query syntax)
// dois from seguidos -> geram todas as combinacoes (x, y) entre as duas listas, como dois foreach encadeados
// ATENCAO: ao contrario do Intersect, NAO remove repetidos; aqui o resultado e igual
// apenas porque as listas nao tem elementos repetidos (para garantir, acrescentar .Distinct())
var hQuery = from x in ListaUm from y in ListaDois where x == y select x;

// 3) extension methods
// Intersect -> devolve os elementos comuns as duas colecoes, sem repetidos
var hExt = ListaUm.Intersect(ListaDois);

Console.WriteLine("============================================================");
Console.WriteLine(" h) Intersecao da ListaUm com a ListaDois");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var s in hForeach) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var s in hQuery) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var s in hExt) Console.Write(s + " | ");
Console.WriteLine("\n\n");



// ============================================================================
// i) Reuniao da ListaUm com a ListaDois (sem repetidos)
// ============================================================================

// 1) foreach
var iForeach = new List<string>();
foreach (var x in ListaUm)
    if (!iForeach.Contains(x))
        iForeach.Add(x);
foreach (var y in ListaDois)
    if (!iForeach.Contains(y))
        iForeach.Add(y);

// 2) query syntax (Union nao existe em query syntax -> aplica-se aos resultados)
var iQuery = (from x in ListaUm select x).Union(from y in ListaDois select y);

// 3) extension methods
var iExt = ListaUm.Union(ListaDois);

Console.WriteLine("============================================================");
Console.WriteLine(" i) Reuniao da ListaUm com a ListaDois");
Console.WriteLine("============================================================");

Console.Write("  Foreach:           ");
foreach (var s in iForeach) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Query syntax:      ");
foreach (var s in iQuery) Console.Write(s + " | ");
Console.WriteLine();

Console.Write("  Extension methods: ");
foreach (var s in iExt) Console.Write(s + " | ");
Console.WriteLine("\n\n");



// ============================================================================
// j) Agrupar Numeros em pares e impares
// ============================================================================

// 1) foreach
// n % 2 -> resto da divisao por 2: 0 se for par, 1 se for impar
var pares = new List<int>();
var impares = new List<int>();
foreach (var n in Numeros)
{
    if (n % 2 == 0) pares.Add(n);
    else impares.Add(n);
}

// 2) query syntax
// group n by n % 2 -> agrupa os numeros pela chave n % 2 (grupo 0 = pares, grupo 1 = impares)
// o resultado e uma colecao de grupos; cada grupo tem a chave em .Key e percorre-se como uma colecao
// (por isso, ao mostrar, ha um foreach para os grupos e outro, dentro, para os numeros de cada grupo)
// nota: a query termina em group ... by, sem select
var jQuery = from n in Numeros group n by n % 2;

// 3) extension methods
// GroupBy recebe a lambda que calcula a chave de cada elemento
var jExt = Numeros.GroupBy(n => n % 2);

Console.WriteLine("============================================================");
Console.WriteLine(" j) Numeros agrupados em pares e impares");
Console.WriteLine("============================================================");

Console.Write("  Foreach:");
Console.WriteLine();
Console.Write("    Numeros Pares: ");
foreach (var y in pares) Console.Write(y + " ");
Console.WriteLine();
Console.Write("    Numeros Impares: ");
foreach (var y in impares) Console.Write(y + " ");
Console.WriteLine("\n");

Console.Write("  Query syntax:");
// condicao ? valorSeVerdadeiro : valorSeFalso -> operador ternario (um if/else numa expressao)
foreach (var x in jQuery)
{
    Console.WriteLine();
    Console.Write(x.Key == 0 ? "    Numeros Pares: " : "    Numeros Impares: ");
    foreach (var y in x) Console.Write(y + " ");
}
Console.WriteLine("\n");

Console.Write("  Extension methods:");
foreach (var x in jExt)
{
    Console.WriteLine();
    Console.Write(x.Key == 0 ? "    Numeros Pares: " : "    Numeros Impares: ");
    foreach (var y in x) Console.Write(y + " ");
}
Console.WriteLine("\n\n");



// ============================================================================
// k) Produto dos numeros inferiores a trinta
// ============================================================================

// 1) foreach
long kForeach = 1;
foreach (var n in Numeros)
    if (n < 30)
        kForeach *= n;

// 2) query syntax (Aggregate nao existe em query syntax -> aplica-se ao resultado)
// Aggregate((a, b) => a * b) -> percorre os elementos acumulando um resultado:
//   a = valor acumulado ate agora (comeca no 1.o elemento), b = elemento seguinte
//   aqui: 10, 23, 12, 1, 4 -> ((((10 * 23) * 12) * 1) * 4) = 11040
// (long)n -> converte cada numero para long; e uma precaucao, porque um produto cresce depressa
//            e pode ultrapassar o limite do int (overflow). Com estes valores nao chegaria a acontecer
long kQuery = (from n in Numeros where n < 30 select (long)n).Aggregate((a, b) => a * b);

// 3) extension methods
// Select(n => (long)n) -> faz a conversao para long, tal como o select da query
long kExt = Numeros.Where(n => n < 30).Select(n => (long)n).Aggregate((a, b) => a * b);

Console.WriteLine("============================================================");
Console.WriteLine(" k) Produto dos numeros inferiores a 30");
Console.WriteLine("============================================================");

Console.WriteLine("  Foreach:           " + kForeach);
Console.WriteLine("  Query syntax:      " + kQuery);
Console.WriteLine("  Extension methods: " + kExt);
Console.WriteLine("\n");



// ============================================================================
// l) Elementos com "C#": primeira e ultima palavra
// ============================================================================

// 1) foreach
// (string str, string sInicial, string sFinal) -> tuplo com nomes: agrupa 3 valores sem criar uma classe
// palavras[palavras.Length - 1] -> ultimo elemento do array (os indices comecam em 0)
var lForeach = new List<(string str, string sInicial, string sFinal)>();
foreach (var s in ListaUm)
{
    if (s.Contains("C#"))
    {
        var palavras = s.Trim().Split(' ');
        lForeach.Add((s.Trim(), palavras[0], palavras.Length == 1 ? "NAO TEM" : palavras[palavras.Length - 1]));
    }
}

// 2) query syntax
// let palavras = ... -> cria uma variavel intermedia dentro da query, reutilizada nas linhas seguintes
// new { ... }        -> tipo anonimo: objeto com as propriedades indicadas, sem ser preciso declarar uma classe
// First() / Last()   -> primeiro / ultimo elemento de uma colecao
var lQuery = from s in ListaUm
             where s.Contains("C#")
             let palavras = s.Trim().Split(' ')
             select new
             {
                 str = s.Trim(),
                 sInicial = palavras.First(),
                 sFinal = palavras.Length == 1 ? "NAO TEM" : palavras.Last()
             };

// 3) extension methods
// sem o let, o s.Trim().Split(' ') tem de ser repetido em cada propriedade
var lExt = ListaUm.Where(s => s.Contains("C#"))
                  .Select(s => new
                  {
                      str = s.Trim(),
                      sInicial = s.Trim().Split(' ').First(),
                      sFinal = s.Trim().Split(' ').Length == 1 ? "NAO TEM" : s.Trim().Split(' ').Last()
                  });

Console.WriteLine("============================================================");
Console.WriteLine(" l) Elementos com \"C#\": primeira e ultima palavra");
Console.WriteLine("============================================================");

Console.WriteLine("  Foreach:");
foreach (var s in lForeach)
    Console.WriteLine("String: " + s.str + "\n\t Primeira Palavra: "
        + s.sInicial + "\n\t Ultima Palavra: " + s.sFinal);
Console.WriteLine();

Console.WriteLine("  Query syntax:");
foreach (var s in lQuery)
    Console.WriteLine("String: " + s.str + "\n\t Primeira Palavra: "
        + s.sInicial + "\n\t Ultima Palavra: " + s.sFinal);
Console.WriteLine();

Console.WriteLine("  Extension methods:");
foreach (var s in lExt)
    Console.WriteLine("String: " + s.str + "\n\t Primeira Palavra: "
        + s.sInicial + "\n\t Ultima Palavra: " + s.sFinal);
Console.WriteLine();
