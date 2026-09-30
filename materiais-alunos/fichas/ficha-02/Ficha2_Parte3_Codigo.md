# Ficha 2, parte 3: Razor Class Library

Código completo da parte 3 da ficha 2 (App Utilidades), pela ordem das alíneas. Cada bloco pode ser copiado tal como está, e cada passo tem uma explicação curta do porquê.

Os três projetos da solução chamam-se `MauiUtilidades`, `BlazorUtilidades` e `UtilidadesRCL`. O código da RCL e da app Web foi compilado e executado com o SDK do .NET 8, sem erros nem avisos. O código do MAUI segue o template do .NET 8 e as partes Razor foram compiladas à parte.

Os campos de `Energia`, `Evento` e `Noticia` e os dados de exemplo de todos os serviços são uma proposta, no mesmo molde da `Temperatura` da solução de referência.

> **Atenção aos nomes dos projetos**
>
> Aqui os projetos chamam-se `MauiUtilidades`, `BlazorUtilidades` e `UtilidadesRCL`. A ficha usa `RCLUtilidades`, e cada um pode ter escolhido outros nomes. Ao copiar código, trocar o nome pelo do vosso projeto em todas as ligações; copiado tal e qual, dá erro:
>
> - `_content/UtilidadesRCL/...` no `index.html` e no `App.razor` (secção 2.3): o nome é o do projeto da RCL; errado, a página fica sem estilos (404 nos `.css`);
> - `@using UtilidadesRCL...` nos `_Imports.razor` e `using UtilidadesRCL...` nos `.cs`: errado, dá CS0246 ou o aviso RZ10012;
> - `namespace UtilidadesRCL...` na primeira linha dos ficheiros novos: o Visual Studio já põe o certo; ao colar código, confirmar essa linha;
> - `typeof(UtilidadesRCL._Imports)` no `Routes.razor` e no `Program.cs` (secção 4.10): errado, não compila.

## Índice

1. [Criar a RCL (alínea 3a)](#1-criar-a-rcl-alínea-3a)
2. [Despir as apps (alínea 3b)](#2-despir-as-apps-alínea-3b)
3. [Dados e serviços (alíneas 3c e 3d)](#3-dados-e-serviços-alíneas-3c-e-3d)
4. [Componentes (alíneas 3e e 3f)](#4-componentes-alíneas-3e-e-3f)
5. [Compilar e correr (alíneas 3g a 3j)](#5-compilar-e-correr-alíneas-3g-a-3j)
6. [Arranque: Program.cs e MauiProgram.cs](#6-arranque-programcs-e-mauiprogramcs)
7. [Demonstração opcional: tempos de vida](#7-demonstração-opcional-tempos-de-vida)
8. [Erros frequentes](#8-erros-frequentes)

---

## 1. Criar a RCL (alínea 3a)

**Porquê:** as duas apps mostram as mesmas páginas. Em vez de manter duas cópias do mesmo código, o que é comum passa para uma biblioteca que as duas usam. Uma Razor Class Library (RCL) é uma biblioteca .NET que, além de classes, pode ter componentes Razor e ficheiros estáticos. Não é uma aplicação: não arranca sozinha e não tem `Program.cs`.

1. Clique direito na solução > **Adicionar** > **Novo Projeto** > **Biblioteca de Classes Razor**.
2. Nome `UtilidadesRCL`, **.NET 8**. Deixar desmarcado **Suportar páginas e vistas**: essa opção é para MVC e Razor Pages, que não usamos; aqui só há componentes.
3. Apagar `Component1.razor`, `Component1.razor.css` e `ExampleJsInterop.cs`: são exemplos do template (Figura 29). O `background.png` e o `exampleJsInterop.js` do `wwwroot` podem ficar, como na figura.
4. Em `MauiUtilidades` e em `BlazorUtilidades`: clique direito em **Dependências** > **Adicionar Referência de Projeto** > marcar `UtilidadesRCL`. Sem a referência, as apps não conhecem as classes da RCL e dá o erro CS0246.

O passo 4 acrescenta ao `.csproj` de cada app:
```xml
<ItemGroup>
  <ProjectReference Include="..\UtilidadesRCL\UtilidadesRCL.csproj" />
</ItemGroup>
```

---

## 2. Despir as apps (alínea 3b)

**Porquê:** tirar de cada app o que passa a vir da RCL, para não haver duas cópias. Em cada app fica só o que é dela: o arranque, o código nativo, a página anfitriã (onde o Blazor arranca) e o layout. O resultado tem de ficar como na Figura 28 da ficha.

### 2.1 O que acontece a cada ficheiro

**MauiUtilidades**

| Ficheiro | O que é | Destino | Porquê |
|---|---|---|---|
| `Platforms/`, `Resources/` | código e recursos nativos | fica | só existem numa app instalada |
| `App.xaml`, `MainPage.xaml` | app nativa e página com a BlazorWebView | fica | são o anfitrião nativo |
| `MauiProgram.cs` | arranque e registo de serviços | fica | cada app tem o seu arranque e o seu contentor |
| `wwwroot/index.html` | página que a BlazorWebView abre | fica | o `MainPage.xaml` aponta para ela |
| `wwwroot/css/`, `wwwroot/favicon.png` | Bootstrap, app.css, ícone | apagar | passam a vir da RCL |
| `Components/Routes.razor`, `Components/_Imports.razor` | router e `@using` | ficam, ajustam-se | são de cada app; passam a conhecer a RCL |
| `Components/Layout/MainLayout.razor` | moldura da página | fica | o layout é de cada app |
| `Components/Layout/NavMenu.razor` e `.razor.css` | menu lateral | mover para `UtilidadesRCL/Shared/` (criar a pasta) | passa a ser o menu partilhado |
| `Components/Pages/Home.razor` | página inicial | fica | a página inicial é de cada app |
| `Components/Pages/Counter.razor`, `Weather.razor` | exemplos | apagar | dão lugar às utilidades |

**BlazorUtilidades**

| Ficheiro | O que é | Destino | Porquê |
|---|---|---|---|
| `Properties/launchSettings.json` | portas e perfis de arranque | fica | é da app Web |
| `appsettings.json` (e `appsettings.Development.json`) | configuração de logging e hosts | apagar | a Figura 28 não o mostra; sem ele valem os valores por omissão |
| `Program.cs` | arranque, serviços e pipeline HTTP | fica | cada app tem o seu arranque |
| `wwwroot/bootstrap/`, `wwwroot/app.css`, `wwwroot/favicon.png` | CSS e ícone | mover para `UtilidadesRCL/wwwroot/` | uma só cópia, partilhada |
| `Components/App.razor` | documento HTML da app | fica | é a página anfitriã da Web |
| `Components/Routes.razor`, `Components/_Imports.razor` | router e `@using` | ficam, ajustam-se | são de cada app |
| `Components/Layout/MainLayout.razor` | moldura da página | fica | o layout é de cada app |
| `Components/Layout/NavMenu.razor` e `.razor.css` | menu lateral | apagar | passa a usar o da RCL (nota da secção 2.4) |
| `Components/Pages/Home.razor` | página inicial | fica | passa a usar o `HomeComponent` |
| `Components/Pages/Counter.razor`, `Weather.razor`, `Error.razor` | exemplos e página de erro | apagar | a Figura 28 só deixa o `Home.razor` |

O CSS isolado acompanha sempre o seu componente: `NavMenu.razor.css` vai com o `NavMenu`, `MainLayout.razor.css` fica com o `MainLayout`.

### 2.2 Mover o wwwroot

**Porquê:** o `wwwroot` é a pasta dos ficheiros estáticos (CSS, imagens, ícones), que chegam ao browser tal como estão. Os do MAUI e os do Web são praticamente iguais, por isso move-se um conjunto para a RCL e apaga-se o outro. Move-se o do Web porque já tem os ficheiros na raiz do `wwwroot`, sem a pasta `css/`. O `index.html` do MAUI fica porque é a página que a BlazorWebView abre.

1. Arrastar `bootstrap/`, `app.css` e `favicon.png` de `BlazorUtilidades/wwwroot/` para `UtilidadesRCL/wwwroot/`. A pasta `wwwroot` do Web fica vazia e pode apagar-se.
2. Em `MauiUtilidades/wwwroot/`, apagar `css/` e `favicon.png`. Fica só o `index.html`.

### 2.3 Acertar os caminhos

**Porquê:** os ficheiros do `wwwroot` de uma biblioteca não se juntam à raiz da app. O build publica-os num caminho próprio, `_content/UtilidadesRCL/`. Se os caminhos ficarem como estavam, o browser pede `app.css`, recebe 404 e a página aparece sem estilos.

Mudam três linhas em cada anfitrião (as que têm `_content/UtilidadesRCL/`). A linha do `{Projeto}.styles.css` não muda: é gerada no build e já importa o CSS isolado da RCL.

**`MauiUtilidades/wwwroot/index.html`**

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover" />
    <title>MauiUtilidades</title>
    <base href="/" />
    <link rel="stylesheet" href="_content/UtilidadesRCL/bootstrap/bootstrap.min.css" />
    <link rel="stylesheet" href="_content/UtilidadesRCL/app.css" />
    <link rel="stylesheet" href="MauiUtilidades.styles.css" />
    <link rel="icon" type="image/png" href="_content/UtilidadesRCL/favicon.png" />
</head>

<body>

    <div class="status-bar-safe-area"></div>

    <div id="app">Loading...</div>

    <div id="blazor-error-ui">
        An unhandled error has occurred.
        <a href="" class="reload">Reload</a>
        <a class="dismiss">🗙</a>
    </div>

    <script src="_framework/blazor.webview.js" autostart="false"></script>

</body>

</html>
```

**`BlazorUtilidades/Components/App.razor`**

```razor
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <base href="/" />
    <link rel="stylesheet" href="_content/UtilidadesRCL/bootstrap/bootstrap.min.css" />
    <link rel="stylesheet" href="_content/UtilidadesRCL/app.css" />
    <link rel="stylesheet" href="BlazorUtilidades.styles.css" />
    <link rel="icon" type="image/png" href="_content/UtilidadesRCL/favicon.png" />
    <HeadOutlet @rendermode="InteractiveServer" />
</head>

<body>
    <Routes @rendermode="InteractiveServer" />
    <script src="_framework/blazor.web.js"></script>
</body>

</html>
```

### 2.4 Um só NavMenu, em `Shared`

1. Na RCL, criar a pasta `Shared`: clique direito em `UtilidadesRCL` > **Adicionar** > **Nova Pasta**. O template da RCL não a traz.
2. Arrastar `NavMenu.razor` e `NavMenu.razor.css` de `MauiUtilidades/Components/Layout/` para `UtilidadesRCL/Shared/`. Entre projetos, o Visual Studio copia; para mover, arrastar com Shift. Confirmar que o original saiu do MAUI e que o `.razor.css` (dentro do `.razor`, na seta) foi junto, senão o menu perde o aspeto.
3. No Web, apagar `NavMenu.razor` e `NavMenu.razor.css` de `Components/Layout/`: as duas apps passam a usar o da RCL.
4. No `NavMenu` da RCL, mudar o texto do cabeçalho (`navbar-brand`) de `MauiUtilidades` para `Utilidades`, porque passa a ser o mesmo nas duas apps.
5. Acrescentar os `@using` abaixo.

**Porquê os dois `@using`:** o namespace de um componente é a pasta onde está. Na pasta nova, o menu passa a ser `UtilidadesRCL.Shared.NavMenu`, e o `MainLayout` de cada app só o encontra com um `@using`. Na RCL, o `NavLink` que o menu usa vive em `Microsoft.AspNetCore.Components.Routing`, que o template da RCL não traz. Sem estes `@using` aparece o aviso RZ10012: o menu fica vazio ou os links não navegam.

**`MauiUtilidades/Components/_Imports.razor e BlazorUtilidades/Components/_Imports.razor`**  
Acrescentar no fim dos dois.

```razor
@using UtilidadesRCL.Shared
```

**`UtilidadesRCL/_Imports.razor`**  
Por agora; o conteúdo final está na secção 4.9.

```razor
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Routing
```

> **Nota: a ficha deixa o `NavMenu` do Web no Web; aqui usa-se um só.**
>
> A ficha manda mover o `NavMenu` do MAUI para a RCL e deixar o do Web onde está, mas os dois ficheiros são iguais. Com um só menu na RCL, as entradas das utilidades escrevem-se uma vez e as duas apps ficam iguais, que é a ideia da RCL. Por isso, no fim desta etapa, o Web fica ligeiramente diferente da Figura 28: sem o `NavMenu` em `Components/Layout`.
>
> Não pode ficar nenhuma cópia do `NavMenu` nas apps. Se ficar (por exemplo, porque o arrastar copiou em vez de mover), o `MainLayout` vê dois e dá o erro RZ9985 ("Multiple components use the tag 'NavMenu'").

### 2.5 Apagar os exemplos

Apagar `Counter.razor` e `Weather.razor` em `MauiUtilidades/Components/Pages/` e em `BlazorUtilidades/Components/Pages/`. No Web, apagar também o `Error.razor` e o `appsettings.json`, que a Figura 28 já não mostra. O `Home.razor` fica nos dois.

O `Program.cs` continua com `UseExceptionHandler("/Error")`. Em desenvolvimento não se nota; em produção, um erro passaria a dar 404 em vez da página de erro.

**Tirar também as entradas do menu.** O menu ainda tem links para `counter` e `weather`, que já não existem: clicar neles dá erro (na Web, um 404). Apagar estes dois blocos em `UtilidadesRCL/Shared/NavMenu.razor`; fica só a entrada do Home. As entradas das utilidades entram na etapa 4 (secção 4.8).

**`Apagar em UtilidadesRCL/Shared/NavMenu.razor`**

```razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="counter">
        <span class="bi bi-plus-square-fill-nav-menu" aria-hidden="true"></span> Counter
    </NavLink>
</div>

<div class="nav-item px-3">
    <NavLink class="nav-link" href="weather">
        <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Weather
    </NavLink>
</div>
```

**`UtilidadesRCL/Shared/NavMenu.razor`**  
Como fica no fim da etapa 2: o cabeçalho diz `Utilidades` e o menu só tem o Home. Não leva `@namespace` nem `@using`: o namespace vem da pasta e o `NavLink` vem do `_Imports.razor` da RCL.

```razor
<div class="top-row ps-3 navbar navbar-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="">Utilidades</a>
    </div>
</div>

<input type="checkbox" title="Navigation menu" class="navbar-toggler" />

<div class="nav-scrollable" onclick="document.querySelector('.navbar-toggler').click()">
    <nav class="flex-column">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="" Match="NavLinkMatch.All">
                <span class="bi bi-house-door-fill-nav-menu" aria-hidden="true"></span> Home
            </NavLink>
        </div>
    </nav>
</div>
```

**Porquê só agora:** até aqui eram a prova de que cada app ainda funcionava. Apagam-se no fim da etapa para haver sempre um ponto de retorno.

### 2.6 Ponto de controlo

Compilar e correr as duas apps. Devem abrir com estilos e menu, só com a página Home (o menu só tem a entrada Home), e a estrutura deve ser a da Figura 28.

**Porquê compilar já:** um erro encontrado aqui é fácil de localizar. Depois de mais vinte ficheiros, já não é.

**Se a barra lateral de uma app aparecer vazia** (sem o cabeçalho `Utilidades` e sem o Home), falta o `@using UtilidadesRCL.Shared` no `Components/_Imports.razor` dessa app. Compila na mesma, só com o aviso RZ10012 ("Found markup element with unexpected name 'NavMenu'") na Lista de Erros, e o `<NavMenu />` do `MainLayout` vai para o browser como uma tag HTML desconhecida, que não mostra nada.

Como devem ficar as duas apps (Figura 28 da ficha). Diferença: aqui o Web já não tem o `NavMenu` em `Components/Layout` (nota da secção 2.4).

![Figura 28: BlazorUtilidades e MauiUtilidades despidos](figura28.png)

---

## 3. Dados e serviços (alíneas 3c e 3d)

**Porquê:** cada utilidade tem três peças, sempre com a mesma forma:

- **DTO:** o formato dos dados (só dados, sem lógica);
- **interface:** o que o serviço promete fazer;
- **serviço:** como o faz. Por agora, com dados escritos no código.

Os componentes só conhecem o DTO e a interface. Assim, quando os dados passarem a vir de uma API (ficha 6), troca-se o serviço e as páginas ficam iguais.

Criar em `UtilidadesRCL` as pastas `Data/DTO`, `Data/Interfaces`, `Data/Services` e `Pages`. Cada pasta é um namespace (`UtilidadesRCL.Data.DTO`, ...); criam-se antes dos ficheiros para o Visual Studio dar logo o namespace certo.

### 3.1 namespace e using

Todos os ficheiros `.cs` desta etapa começam com estas duas coisas.

**namespace: o nome completo de uma classe.** Com `namespace UtilidadesRCL.Data.DTO;`, o record chama-se, por inteiro, `UtilidadesRCL.Data.DTO.Temperatura`. Serve para organizar o código e para duas classes com o mesmo nome poderem conviver em namespaces diferentes. Por convenção segue o projeto e as pastas (o Visual Studio põe-no ao criar o ficheiro), mas para o compilador conta a linha `namespace`, não a pasta. Com ponto e vírgula vale para o ficheiro todo; a forma antiga, com chavetas, aparece no `MauiProgram.cs`.

**using: um atalho para não escrever o nome completo.** Sem `using UtilidadesRCL.Data.DTO;`, a interface teria de escrever `Task<IEnumerable<UtilidadesRCL.Data.DTO.Temperatura>>`. O `using` não traz código: quem torna a RCL disponível para as apps é a referência de projeto (secção 1). Por isso o erro CS0246 tem duas causas possíveis: falta a referência, ou falta o `using`.

**Quando não é preciso `using`:**

- classes do mesmo namespace veem-se diretamente;
- um namespace vê os de cima: o `TemperaturaService` (`UtilidadesRCL.Data.Services`) vê `UtilidadesRCL.Data` e `UtilidadesRCL`, mas não `UtilidadesRCL.Data.DTO` nem `UtilidadesRCL.Data.Interfaces`, daí os seus dois `using`;
- o `.csproj` tem `<ImplicitUsings>enable</ImplicitUsings>`, que já inclui `System`, `System.Linq`, `System.Collections.Generic` e `System.Threading.Tasks`; é por isso que `Task` e `IEnumerable` funcionam sem `using`.

**Nos `.razor`:** `@using` é o mesmo que `using`. Os `.razor` não têm linha `namespace`: recebem o da pasta, e por isso o `NavMenu`, em `Shared`, passou a ser `UtilidadesRCL.Shared.NavMenu`. O `_Imports.razor` junta os `@using` de todos os `.razor` da pasta e subpastas, mas não chega aos `.cs`.

### 3.2 Temperatura

Escrever pela ordem DTO, interface, serviço: com a interface feita, o Visual Studio gera o esqueleto do serviço (Ctrl+. em cima do nome da interface > **Implementar interface**).

**`UtilidadesRCL/Data/DTO/Temperatura.cs`**  
`record` posicional: numa linha, o compilador gera as três propriedades (só se atribuem ao criar o objeto), o construtor e a comparação por valor. Serve bem a um DTO: é só dados e não deve mudar depois de criado.

```csharp
namespace UtilidadesRCL.Data.DTO;

public record Temperatura(string Dia, string TemperaturaC, string Resumo);
```

**`UtilidadesRCL/Data/Interfaces/ITemperaturaService.cs`**  
Diz o que o serviço faz, não como. `Task<...>` porque o resultado chega mais tarde; `IEnumerable<T>` porque só se promete algo que se percorre. Sem `?` a seguir ao `Task`: com ele, o compilador avisa (CS8602) no `await`.

```csharp
using UtilidadesRCL.Data.DTO;

namespace UtilidadesRCL.Data.Interfaces;

public interface ITemperaturaService
{
    Task<IEnumerable<Temperatura>> LoadTemperaturasAsync();
}
```

**`UtilidadesRCL/Data/Services/TemperaturaService.cs`**  
Implementa a interface. O `await Task.Delay(1000)` faz de conta que os dados vêm de uma API lenta, e é por isso que o método é `async`. `new[] { ... }` cria um array com o tipo deduzido dos elementos.

```csharp
using UtilidadesRCL.Data.DTO;
using UtilidadesRCL.Data.Interfaces;

namespace UtilidadesRCL.Data.Services;

public class TemperaturaService : ITemperaturaService
{
    public async Task<IEnumerable<Temperatura>>
        LoadTemperaturasAsync()
    {
        await Task.Delay(1000);

        var temperaturas = new[]
        {
            new Temperatura("Segunda", "24", "Ameno"),
            new Temperatura("Terça", "27", "Quente"),
            new Temperatura("Quarta", "31", "Muito quente"),
            new Temperatura("Quinta", "22", "Ameno"),
            new Temperatura("Sexta", "18", "Fresco"),
        };
        return temperaturas;
    }
}
```

### 3.3 Energia

O mesmo molde da Temperatura: mudam os nomes e os campos.

**`UtilidadesRCL/Data/DTO/Energia.cs`**

```csharp
namespace UtilidadesRCL.Data.DTO;

public record Energia(string Dia, string Periodo, string PrecoKWh);
```

**`UtilidadesRCL/Data/Interfaces/IEnergiaService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;

namespace UtilidadesRCL.Data.Interfaces;

public interface IEnergiaService
{
    Task<IEnumerable<Energia>> LoadEnergiasAsync();
}
```

**`UtilidadesRCL/Data/Services/EnergiaService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;
using UtilidadesRCL.Data.Interfaces;

namespace UtilidadesRCL.Data.Services;

public class EnergiaService : IEnergiaService
{
    public async Task<IEnumerable<Energia>>
        LoadEnergiasAsync()
    {
        await Task.Delay(1000);

        var energias = new[]
        {
            new Energia("Hoje", "Vazio", "0,108 €"),
            new Energia("Hoje", "Cheias", "0,174 €"),
            new Energia("Hoje", "Ponta", "0,215 €"),
            new Energia("Amanhã", "Vazio", "0,105 €"),
            new Energia("Amanhã", "Ponta", "0,221 €"),
        };
        return energias;
    }
}
```

### 3.4 Evento

O mesmo molde da Temperatura: mudam os nomes e os campos.

**`UtilidadesRCL/Data/DTO/Evento.cs`**

```csharp
namespace UtilidadesRCL.Data.DTO;

public record Evento(string Data, string Nome, string Local);
```

**`UtilidadesRCL/Data/Interfaces/IEventoService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;

namespace UtilidadesRCL.Data.Interfaces;

public interface IEventoService
{
    Task<IEnumerable<Evento>> LoadEventosAsync();
}
```

**`UtilidadesRCL/Data/Services/EventoService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;
using UtilidadesRCL.Data.Interfaces;

namespace UtilidadesRCL.Data.Services;

public class EventoService : IEventoService
{
    public async Task<IEnumerable<Evento>>
        LoadEventosAsync()
    {
        await Task.Delay(1000);

        var eventos = new[]
        {
            new Evento("03-10-2026", "Feira do Livro", "Braga"),
            new Evento("10-10-2026", "Concerto", "Figueira"),
            new Evento("17-10-2026", "Maratona", "Lisboa"),
            new Evento("24-10-2026", "Festival Tech", "Porto"),
        };
        return eventos;
    }
}
```

### 3.5 Notícia

O mesmo molde da Temperatura: mudam os nomes e os campos.

**`UtilidadesRCL/Data/DTO/Noticia.cs`**

```csharp
namespace UtilidadesRCL.Data.DTO;

public record Noticia(string Data, string Titulo, string Fonte);
```

**`UtilidadesRCL/Data/Interfaces/INoticiaService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;

namespace UtilidadesRCL.Data.Interfaces;

public interface INoticiaService
{
    Task<IEnumerable<Noticia>> LoadNoticiasAsync();
}
```

**`UtilidadesRCL/Data/Services/NoticiaService.cs`**

```csharp
using UtilidadesRCL.Data.DTO;
using UtilidadesRCL.Data.Interfaces;

namespace UtilidadesRCL.Data.Services;

public class NoticiaService : INoticiaService
{
    public async Task<IEnumerable<Noticia>>
        LoadNoticiasAsync()
    {
        await Task.Delay(1000);

        var noticias = new[]
        {
            new Noticia("29-09-2026", "Novo semestre", "ISEC"),
            new Noticia("28-09-2026", "Luz mais barata", "Lusa"),
            new Noticia("27-09-2026", "Chuva no Centro", "IPMA"),
        };
        return noticias;
    }
}
```

### 3.6 Registo dos serviços

**Porquê:** os componentes não fazem `new TemperaturaService()`; pedem um `ITemperaturaService` e quem o cria é o contentor de serviços da app (injeção de dependências). O contentor só sabe criar o que estiver registado. A RCL não tem arranque nem contentor, por isso o registo faz-se nas duas apps; se faltar numa, a página rebenta ao abrir com "Cannot provide a value for property...".

As mesmas linhas vão para `MauiProgram.cs` e para `Program.cs`, antes do `builder.Build()`: depois dele o contentor fica fechado. `AddScoped` dá um objeto por utilizador (na Web, por separador; no MAUI, a app inteira); para serviços sem estado, como estes, é a escolha segura. Os ficheiros completos estão na secção 6.

```csharp
using UtilidadesRCL.Data.Interfaces;
using UtilidadesRCL.Data.Services;

builder.Services.AddScoped<IEnergiaService, EnergiaService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<INoticiaService, NoticiaService>();
builder.Services.AddScoped<ITemperaturaService, TemperaturaService>();
```

### 3.7 Ponto de controlo

Compilar a solução: não deve haver erros. Ainda não há nada novo no ecrã; os serviços só aparecem quando houver componentes a usá-los.

Se der erro, é quase sempre um `using` em falta (CS0246) ou um nome diferente entre a interface e o serviço. Convém resolver antes de haver componentes a depender deles.

---

## 4. Componentes (alíneas 3e e 3f)

> **Nota: os componentes ficam com erros até à secção 4.9.**
>
> Os componentes não têm `@inject` nem `@using`: os serviços e os DTO vêm do `_Imports.razor` da RCL (secção 4.9, alínea 3f). Até esse passo, o Visual Studio mostra dois erros em cada componente, por exemplo no `TemperaturaComponent`:
>
> - CS0246 em `Temperatura`: o `@using UtilidadesRCL.Data.DTO` ainda não existe;
> - CS0103 em `temperaturaService`: o `@inject ITemperaturaService temperaturaService` ainda não existe.
>
> É normal: desaparecem com a secção 4.9, e só se compila na 3g. Quem preferir pode fazer a secção 4.9 primeiro.

Também não têm `@rendermode`: a interatividade foi escolhida para a app toda (Global) na parte 2. Com `@rendermode` no componente, ele deixava de funcionar no MAUI.

### 4.1 Temperatura

O que convém reparar:

- `@page "/temperaturas"` dá o endereço da página.
- `OnInitializedAsync` corre quando o componente é criado: é o sítio para ir buscar os dados.
- `temperaturas is null`: da primeira vez que o componente desenha, os dados ainda não chegaram (o serviço demora 1 s). Mostra "A carregar..." e, quando o `await` acaba, o Blazor volta a desenhar, já com a tabela.
- `table table-striped` são classes do Bootstrap.

**`UtilidadesRCL/Pages/TemperaturaComponent.razor`**

```razor
@page "/temperaturas"

<h3>Temperaturas para esta semana</h3>

@if (temperaturas is null)
{
    <span>A carregar temperaturas...</span>
}
else
{
    <table class="table table-striped">
        <thead>
            <tr><th>Dia</th><th>Temperatura (°C)</th><th>Resumo</th></tr>
        </thead>
        <tbody>
            @foreach (var t in temperaturas)
            {
                <tr>
                    <td>@t.Dia</td>
                    <td>@t.TemperaturaC</td>
                    <td>@t.Resumo</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private IEnumerable<Temperatura>? temperaturas;

    protected override async Task OnInitializedAsync()
    {
        temperaturas = await temperaturaService
            .LoadTemperaturasAsync();
    }
}
```

### 4.2 Energia

Igual à Temperatura: mudam a rota, o título, o DTO, o serviço e as colunas.

**`UtilidadesRCL/Pages/EnergiaComponent.razor`**

```razor
@page "/energias"

<h3>Preços da Energia</h3>

@if (energias is null)
{
    <span>A carregar preços...</span>
}
else
{
    <table class="table table-striped">
        <thead>
            <tr><th>Dia</th><th>Período</th><th>Preço por kWh</th></tr>
        </thead>
        <tbody>
            @foreach (var e in energias)
            {
                <tr>
                    <td>@e.Dia</td>
                    <td>@e.Periodo</td>
                    <td>@e.PrecoKWh</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private IEnumerable<Energia>? energias;

    protected override async Task OnInitializedAsync()
    {
        energias = await energiaService
            .LoadEnergiasAsync();
    }
}
```

**Alternativa: code-behind com `partial class`.** Na ficha todos os componentes têm o C# no próprio `.razor`, como na Figura 29. Quando o código cresce, pode passar para um ficheiro à parte, `EnergiaComponent.razor.cs`. Não é para fazer agora; é para reconhecerem quando o virem.

Uma `partial class` é uma classe escrita em mais do que um ficheiro. Cada parte leva `partial` e o compilador junta-as numa só; todas têm de ter o mesmo nome, o mesmo namespace e estar no mesmo projeto. Existe sobretudo para juntar código gerado por uma ferramenta com código escrito por nós, sem que um apague o outro.

A partir do `.razor`, o compilador Razor gera uma `partial class EnergiaComponent` que herda de `ComponentBase`, tem as propriedades dos `@inject` do `_Imports.razor` (como o `energiaService`) e o HTML convertido em código. O `.razor.cs` seria a outra parte, com o que está no `@code`, e por isso poderia usar o `energiaService` sem o declarar. O mesmo acontece no MAUI com `MainPage.xaml` e `MainPage.xaml.cs`: o XAML gera o `InitializeComponent()` e um campo para cada controlo com `x:Name`, como o `blazorWebView`.

**`UtilidadesRCL/Pages/EnergiaComponent.razor.cs (alternativa)`**  
Com este ficheiro, o bloco `@code` sai do `.razor`. O `using` é preciso porque o `_Imports.razor` não se aplica aos `.cs`.

```csharp
using UtilidadesRCL.Data.DTO;

namespace UtilidadesRCL.Pages;

public partial class EnergiaComponent
{
    private IEnumerable<Energia>? energias;

    protected override async Task OnInitializedAsync()
    {
        energias = await energiaService
            .LoadEnergiasAsync();
    }
}
```

**Atenção ao namespace:** o `.razor.cs` tem de estar em `UtilidadesRCL.Pages`, o namespace que o `.razor` recebe da pasta. Se não estiver, são duas classes diferentes: dá CS0115 no `OnInitializedAsync` (não há nada para fazer override) e, se se tirar o `override`, CS0103 no `energiaService` e no `energias`.

### 4.3 Evento

Igual à Temperatura: mudam a rota, o título, o DTO, o serviço e as colunas.

**`UtilidadesRCL/Pages/EventoComponent.razor`**

```razor
@page "/eventos"

<h3>Próximos eventos</h3>

@if (eventos is null)
{
    <span>A carregar eventos...</span>
}
else
{
    <table class="table table-striped">
        <thead>
            <tr><th>Data</th><th>Evento</th><th>Local</th></tr>
        </thead>
        <tbody>
            @foreach (var e in eventos)
            {
                <tr>
                    <td>@e.Data</td>
                    <td>@e.Nome</td>
                    <td>@e.Local</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private IEnumerable<Evento>? eventos;

    protected override async Task OnInitializedAsync()
    {
        eventos = await eventoService
            .LoadEventosAsync();
    }
}
```

### 4.4 Notícia

Igual à Temperatura, com os campos das notícias.

**`UtilidadesRCL/Pages/NoticiaComponent.razor`**

```razor
@page "/noticias"

<h3>Últimas notícias</h3>

@if (noticias is null)
{
    <span>A carregar notícias...</span>
}
else
{
    <table class="table table-striped">
        <thead>
            <tr><th>Data</th><th>Título</th><th>Fonte</th></tr>
        </thead>
        <tbody>
            @foreach (var n in noticias)
            {
                <tr>
                    <td>@n.Data</td>
                    <td>@n.Titulo</td>
                    <td>@n.Fonte</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private IEnumerable<Noticia>? noticias;

    protected override async Task OnInitializedAsync()
    {
        noticias = await noticiaService
            .LoadNoticiasAsync();
    }
}
```

### 4.5 MenuComponent

Não tem `@page`: não é uma página, é uma peça que se usa dentro de outros componentes, como uma tag (`<MenuComponent />`). O `NavLink` navega sem recarregar a página; as classes `btn` do Bootstrap dão-lhe aspeto de botão. Faz-se antes do `HomeComponent` porque é ele que o usa.

**`UtilidadesRCL/Pages/MenuComponent.razor`**

```razor
<div class="d-flex flex-wrap gap-2 mt-3">
    <NavLink class="btn btn-outline-primary" href="energias">Energia</NavLink>
    <NavLink class="btn btn-outline-primary" href="eventos">Eventos</NavLink>
    <NavLink class="btn btn-outline-primary" href="noticias">Notícias</NavLink>
    <NavLink class="btn btn-outline-primary" href="temperaturas">Temperaturas</NavLink>
</div>
```

### 4.6 HomeComponent

O conteúdo da página inicial, igual nas duas apps. `[Parameter]` torna `MensagemBody` num atributo que quem usa o componente preenche: é assim que cada app mostra a sua mensagem.

**`UtilidadesRCL/Pages/HomeComponent.razor`**

```razor
<h1>Utilidades</h1>

<p>@MensagemBody</p>

<MenuComponent />

@code {
    [Parameter]
    public string? MensagemBody { get; set; }
}
```

### 4.7 Home de cada app

O `@page "/"` fica em cada app, porque a página inicial é de cada uma; o conteúdo vem da RCL. O `@using UtilidadesRCL.Pages` é preciso porque o `_Imports.razor` da app não conhece essa pasta. O `PageTitle` só está na Web: é o `App.razor` da Web que tem o `HeadOutlet`, que muda o título do separador.

**`MauiUtilidades/Components/Pages/Home.razor`**

```razor
@page "/"
@using UtilidadesRCL.Pages

<HomeComponent MensagemBody="Bem-vindo à versão MAUI das Utilidades." />
```

**`BlazorUtilidades/Components/Pages/Home.razor`**

```razor
@page "/"
@using UtilidadesRCL.Pages

<PageTitle>Utilidades</PageTitle>

<HomeComponent MensagemBody="Bem-vindo à versão Web das Utilidades." />
```

### 4.8 Menus

Uma entrada por utilidade. O `href` tem de ser igual ao `@page` do componente, sem a barra inicial. O `NavLink` acrescenta a classe `active` à entrada da página atual; `NavLinkMatch.All` fica só no Home, senão o `href` vazio coincidia com todas as páginas.

O menu está só na RCL e é usado pelas duas apps, por isso as entradas escrevem-se uma vez:

**`UtilidadesRCL/Shared/NavMenu.razor`**

```razor
<div class="top-row ps-3 navbar navbar-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="">Utilidades</a>
    </div>
</div>

<input type="checkbox" title="Navigation menu" class="navbar-toggler" />

<div class="nav-scrollable" onclick="document.querySelector('.navbar-toggler').click()">
    <nav class="flex-column">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="" Match="NavLinkMatch.All">
                <span class="bi bi-house-door-fill-nav-menu" aria-hidden="true"></span> Home
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="energias">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Energia
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="eventos">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Eventos
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="noticias">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Notícias
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="temperaturas">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Temperaturas
            </NavLink>
        </div>
    </nav>
</div>
```

### 4.9 `_Imports.razor` (alínea 3f)

**Porquê:** os `@using` e `@inject` de um `_Imports.razor` aplicam-se a todos os `.razor` da mesma pasta e subpastas. Com os quatro `@inject` aqui, todos os componentes da RCL recebem os serviços sem os pedirem um a um. Não se aplica aos `.cs` nem a outros projetos.

**`UtilidadesRCL/_Imports.razor`**

```razor
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Routing
@using UtilidadesRCL
@using UtilidadesRCL.Pages
@using UtilidadesRCL.Shared
@using UtilidadesRCL.Data.DTO
@using UtilidadesRCL.Data.Interfaces

@inject IEnergiaService energiaService
@inject IEventoService eventoService
@inject INoticiaService noticiaService
@inject ITemperaturaService temperaturaService
```

**`MauiUtilidades/Components/_Imports.razor`**

```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using MauiUtilidades
@using MauiUtilidades.Components
@using UtilidadesRCL.Shared
```

**`BlazorUtilidades/Components/_Imports.razor`**

```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using BlazorUtilidades
@using BlazorUtilidades.Components
@using UtilidadesRCL.Shared
```

Os das duas apps só mudaram na secção 2.4 (`@using UtilidadesRCL.Shared` no fim).

### 4.10 Router e servidor

**Porquê:** as páginas das utilidades estão agora na RCL, que é outra DLL. O Router só procura componentes com `@page` no assembly da app, por isso é preciso indicar-lhe a RCL, nos dois projetos. Na Web, o `Program.cs` precisa da mesma indicação: o `Routes.razor` trata da navegação com a app aberta, e o `Program.cs` trata do primeiro pedido (escrever o endereço ou recarregar). `typeof(UtilidadesRCL._Imports).Assembly` é só uma forma de chegar à DLL da RCL através de uma classe que lá existe.

**`MauiUtilidades/Components/Routes.razor`**

```razor
<Router AppAssembly="@typeof(MauiProgram).Assembly"
        AdditionalAssemblies="new[] { typeof(UtilidadesRCL._Imports).Assembly }">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="@routeData" Selector="h1" />
    </Found>
</Router>
```

**`BlazorUtilidades/Components/Routes.razor`**

```razor
<Router AppAssembly="typeof(Program).Assembly"
        AdditionalAssemblies="new[] { typeof(UtilidadesRCL._Imports).Assembly }">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

**`BlazorUtilidades/Program.cs`**  
Sem esta linha, recarregar uma página da RCL dá 404. O ficheiro completo está na secção 6.

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(UtilidadesRCL._Imports).Assembly);
```

### 4.11 Ponto de controlo: a RCL completa

Antes de compilar na 3g, comparar a RCL com a Figura 29 da ficha, pasta a pasta. Uma pasta ou um ficheiro em falta encontra-se mais depressa aqui do que pelos erros da compilação.

- `wwwroot`: o que veio do Web e os exemplos do template (`background.png`, `exampleJsInterop.js`);
- `Data`: 4 DTO, 4 interfaces e 4 serviços;
- `Pages`: os seis componentes;
- `Shared`: o `NavMenu` que veio do MAUI;
- `_Imports.razor` com os `@using` e `@inject`;
- sem `Component1` nem `ExampleJsInterop.cs`.

![Figura 29: conteúdo da UtilidadesRCL](figura29.png)

---

## 5. Compilar e correr (alíneas 3g a 3j)

1. **3g:** compilar a solução inteira (Ctrl+Shift+B) e corrigir os erros (secção 8).
2. **3h:** escolher `BlazorUtilidades`, perfil https, e correr.
3. **3i:** escolher `MauiUtilidades` e correr em Windows Machine; depois iniciar o emulador Android, esperar que arranque, recompilar e correr nele. Com o emulador ainda a arrancar, o deploy falha ou demora tanto que parece que nada acontece.
4. **3j:** escolher o device na barra, clique direito na solução > **Configurar Projetos de Inicialização** > **Vários projetos de inicialização** > Ação **Iniciar** em `BlazorUtilidades` e `MauiUtilidades` > F5. O device usado é o que estava selecionado antes de abrir esta janela.

O que deve aparecer: a Home com a mensagem da app e os quatro botões, o menu lateral com as quatro utilidades, e cada página com "A carregar..." seguido da tabela. Na Web, recarregar (F5) em `/temperaturas` confirma que o `Program.cs` conhece as páginas da RCL.

---

## 6. Arranque: Program.cs e MauiProgram.cs

**`Program.cs` (Web)** tem duas fases, separadas pelo `builder.Build()`:

1. **Antes:** registar os serviços, ou seja, o que o contentor sabe criar (o Blazor e os quatro serviços da RCL).
2. **Depois:** montar o pipeline, o caminho que cada pedido HTTP percorre, por esta ordem:
   - `UseExceptionHandler` e `UseHsts`, só em produção: tratam os erros (como a ficha apaga o `Error.razor`, um erro daria 404) e obrigam o browser a usar sempre HTTPS;
   - `UseHttpsRedirection`: um pedido `http://` passa para `https://`;
   - `UseStaticFiles`: serve o `wwwroot` e o `_content/` e responde logo, sem passar pelo Blazor;
   - `UseAntiforgery`: protege os formulários de pedidos forjados por outros sites;
   - `MapRazorComponents`: liga cada endereço à sua página, também as da RCL.
3. **No fim:** `app.Run()` arranca o servidor e fica à espera de pedidos.

A parte 3 acrescentou os `using` da RCL, os quatro `AddScoped` e o `AddAdditionalAssemblies`.

**`BlazorUtilidades/Program.cs`**

```csharp
using BlazorUtilidades.Components;
using UtilidadesRCL.Data.Interfaces;
using UtilidadesRCL.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Serviços: o que o contentor sabe criar
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IEnergiaService, EnergiaService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<INoticiaService, NoticiaService>();
builder.Services.AddScoped<ITemperaturaService, TemperaturaService>();

var app = builder.Build();

// 2. Pipeline: por onde passa cada pedido HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(UtilidadesRCL._Imports).Assembly);

app.Run();
```

**`MauiProgram.cs` (MAUI)** usa o mesmo padrão builder, mas não tem pipeline nem `Run()`, porque não há HTTP:

- quem arranca a app é a plataforma (Windows, Android), que chama `CreateMauiApp()`;
- a seguir, o `App.xaml` cria a janela com a `MainPage`, que abre o `index.html` na BlazorWebView.

No ficheiro:

- `UseMauiApp<App>` indica a classe da app nativa;
- `ConfigureFonts` regista as fontes de `Resources/Fonts`;
- `AddMauiBlazorWebView` regista o que a BlazorWebView precisa;
- `#if DEBUG` liga as ferramentas de programador (F12) e os logs só em debug.

**`MauiUtilidades/MauiProgram.cs`**

```csharp
using Microsoft.Extensions.Logging;
using UtilidadesRCL.Data.Interfaces;
using UtilidadesRCL.Data.Services;

namespace MauiUtilidades
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Serviços da RCL
            builder.Services.AddScoped<IEnergiaService, EnergiaService>();
            builder.Services.AddScoped<IEventoService, EventoService>();
            builder.Services.AddScoped<INoticiaService, NoticiaService>();
            builder.Services.AddScoped<ITemperaturaService, TemperaturaService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
```

---

## 7. Demonstração opcional: tempos de vida

Serve para ver a diferença entre `AddSingleton`, `AddScoped` e `AddTransient`. Não faz parte da ficha. O serviço gera um código aleatório quando é criado: se duas injeções mostram o mesmo código, receberam o mesmo objeto.

**`UtilidadesRCL/Data/Services/CicloVidaService.cs`**

```csharp
namespace UtilidadesRCL.Data.Services;

public class CicloVidaService
{
    public string Id { get; } = Guid.NewGuid().ToString()[..8];
}
```

**`UtilidadesRCL/Pages/CicloVidaComponent.razor`**

```razor
@page "/ciclovida"
@using UtilidadesRCL.Data.Services
@inject CicloVidaService primeiro
@inject CicloVidaService segundo

<h3>Tempos de vida</h3>

<p>Primeira injeção: <strong>@primeiro.Id</strong></p>
<p>Segunda injeção: <strong>@segundo.Id</strong></p>
```

**`BlazorUtilidades/Program.cs`**  
Registar, abrir `/ciclovida` duas vezes, e repetir com `AddScoped` e `AddTransient`.

```csharp
builder.Services.AddSingleton<CicloVidaService>();
```

| Registo | As duas injeções | Outro pedido ou separador |
|---|---|---|
| `AddSingleton` | iguais | igual |
| `AddScoped` | iguais | diferente |
| `AddTransient` | diferentes | diferentes |

---

## 8. Erros frequentes

| Sintoma | Causa provável | Onde corrigir |
|---|---|---|
| Página sem Bootstrap | caminhos sem `_content/UtilidadesRCL/` | `index.html` e `App.razor` |
| CS0246: tipo ou namespace não encontrado | falta a referência de projeto ou um `@using` | `.csproj` da app ou `_Imports.razor` |
| RZ10012 para `NavMenu`; menu vazio | o `NavMenu` mudou de namespace | `@using UtilidadesRCL.Shared` nas duas apps |
| RZ9985: Multiple components use the tag 'NavMenu' | ficou uma cópia do `NavMenu` numa app | apagar o `NavMenu` da app; fica só o da RCL |
| RZ10012 para `NavLink`; links não navegam | falta o Routing na RCL | `_Imports.razor` da RCL |
| Cannot provide a value for property | serviço não registado nessa app | `MauiProgram.cs` ou `Program.cs` |
| Clicar no menu não abre a página | o Router não conhece a RCL | `AdditionalAssemblies` no `Routes.razor` |
| 404 ao recarregar uma página da RCL na Web | o servidor não conhece as rotas da RCL | `AddAdditionalAssemblies` no `Program.cs` |
| Aviso CS8602 no `await` | interface com `Task<...>?` | tirar o `?` da interface |
