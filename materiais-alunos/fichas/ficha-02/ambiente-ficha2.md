# Preparar o ambiente para a Ficha 2

Programação Web, 2026/27. Complemento à ficha 2 (App Utilidades).

A ficha usa .NET 8 e Visual Studio 2022. As versões recentes do Visual Studio (17.14) instalam o .NET 9 e não instalam o .NET 8, pelo que há alguns passos a fazer manualmente.

Este guia cobre dois cenários:

- PC com Windows em processador Intel ou AMD (x64)
- Windows em máquina virtual, num Mac com processador Apple Silicon (Arm64)

Quem estiver no segundo caso deve ler a secção [Windows em máquina virtual no Mac](#windows-em-maquina-virtual-no-mac) antes de instalar seja o que for.

## 1. Verificar o que já está instalado

No PowerShell (tecla Windows, escrever "PowerShell"):

```powershell
dotnet --list-sdks
dotnet workload list
```

O primeiro comando lista os SDK instalados. O segundo lista os workloads, que são extensões do SDK para tipos específicos de aplicação.

Para a ficha 2 são necessários:

| Componente | Para quê |
|---|---|
| SDK 8.0.x | versão usada na ficha |
| Workloads `android`, `ios`, `maccatalyst` e `maui-windows` na linha `8.0.100` | criar e compilar o projeto MAUI Blazor |
| Workload "ASP.NET e desenvolvimento Web" | templates Blazor Web App e Biblioteca de Classes Razor |

Estando tudo presente, passar diretamente à [verificação final](#6-verificacao-final).

## 2. Workloads do Visual Studio

No Visual Studio Installer, botão Modificar, confirmar que estão selecionados:

- ASP.NET e desenvolvimento Web
- Desenvolvimento do .NET Multi-platform App UI

O workload "Processamento e armazenamentos de dados" não é necessário nesta ficha, mas será a partir da ficha 3, com SQL Server e Entity Framework.

Aplicar as alterações e aguardar que a instalação termine.

## Windows em máquina virtual no Mac

Num Mac com Apple Silicon (M1 a M4), o Windows da máquina virtual é Arm64. Todos os instaladores descarregados têm de ser das versões Arm64.

Instalando por engano a versão x64, o instalador conclui sem erros, mas os ficheiros ficam em `C:\Program Files\dotnet\x64\`, onde o comando `dotnet` não os procura. O resultado é uma instalação que aparentemente funcionou mas que o sistema não reconhece.

Para confirmar a arquitetura:

```powershell
dotnet --info
```

Na secção do host, a linha `Architecture` indica `x64` ou `arm64`.

## 3. Instalar o SDK do .NET 8

1. Aceder a <https://dotnet.microsoft.com/download/dotnet/8.0>
2. Na coluna SDK 8.0.4xx, linha Windows, coluna Installers, escolher **x64** ou **Arm64** conforme a máquina. A coluna Binaries não serve, porque contém ficheiros comprimidos sem instalador.
3. Fechar o Visual Studio e executar o instalador.
4. Abrir uma janela nova do PowerShell. As janelas abertas antes da instalação não reconhecem a nova versão.

```powershell
dotnet --list-sdks
```

O resultado esperado são duas versões na mesma pasta:

```
8.0.425 [C:\Program Files\dotnet\sdk]
9.0.318 [C:\Program Files\dotnet\sdk]
```

Não aparecendo a versão 8, consultar a [resolução de problemas](#resolucao-de-problemas).

## 4. Instalar os workloads do MAUI para o .NET 8

Os workloads instalados pelo Visual Studio estão associados ao SDK 9. Para compilar em .NET 8 são necessários os da linha 8.

O comando `dotnet workload install` instala sempre para o SDK ativo. Como o SDK mais recente é o 9, é preciso forçar o 8, o que se faz com um ficheiro `global.json` numa pasta temporária.

No PowerShell aberto como administrador:

```powershell
mkdir C:\temp\net8
cd C:\temp\net8
dotnet new globaljson --sdk-version 8.0.425
dotnet --version
```

O último comando tem de responder `8.0.425`. Respondendo 9.x, não avançar: significa que o `global.json` não está a ser lido, e convém confirmar que a pasta atual é mesmo `C:\temp\net8`.

Dentro da mesma pasta:

```powershell
dotnet workload install maui
```

A instalação demora vários minutos e descarrega várias centenas de MB. Não deve ser interrompida.

Para confirmar:

```powershell
dotnet workload list
```

Os manifestos da linha 8 devem estar presentes:

```
android        34.0.154/8.0.100
ios            18.0.8319/8.0.100
maccatalyst    18.0.8319/8.0.100
maui           8.0.100/8.0.100
maui-windows   8.0.100/8.0.100
```

A pasta `C:\temp\net8` pode ser apagada no fim. Os workloads ficam instalados na máquina.

Surgindo a mensagem "The machine has a pending reboot", reiniciar antes de abrir o Visual Studio.

## 5. Criar o projeto

No Visual Studio, Criar um novo projeto, filtrando por `Blazor Hybrid`. Aparecem dois templates:

| Template | Utilizar |
|---|---|
| Aplicativo .NET MAUI Blazor Hybrid | Sim. Corresponde ao "Aplicativo Blazor .NET MAUI" da ficha; o nome mudou no .NET 9. |
| Aplicativo Web e .NET MAUI Blazor Hybrid | Não. Cria automaticamente os três projetos com a biblioteca partilhada, que é precisamente o trabalho das etapas 3 e 4 da ficha. |

No ecrã seguinte, no campo Estrutura, selecionar .NET 8.0 (Suporte de Longo Prazo).

### Fixar a versão do SDK na solução

Na pasta da solução, criar um ficheiro `global.json` com o seguinte conteúdo:

```json
{
  "sdk": {
    "version": "8.0.425",
    "rollForward": "latestFeature"
  }
}
```

Assim a solução compila sempre com o SDK 8, independentemente das versões instaladas em cada máquina.

## 6. Verificação final

```powershell
dotnet --list-sdks
dotnet --info
```

A verificação decisiva, porém, é prática: criar o projeto MAUI Blazor em .NET 8, selecionar Windows Machine na barra de execução e compilar com F5. Abrindo uma janela com o menu lateral e as páginas Home, Counter e Weather, o ambiente está pronto.

## Emuladores Android

O Gestor de Dispositivos Android abre-se pelo ícone na barra de ferramentas ou pelo menu Ferramentas.

Em PC Windows x64, criar um dispositivo com imagem x86_64, que é a mais rápida. Em máquina virtual num Mac com Apple Silicon, a imagem tem de ser arm64-v8a, porque as imagens x86_64 não arrancam em Windows on Arm.

Três regras evitam a maior parte dos problemas:

1. Iniciar o emulador primeiro e esperar que o Android arranque por completo. Só depois compilar e executar.
2. Selecionar o emulador na lista de destinos, no mesmo local onde estava "Windows Machine".
3. Não interromper o primeiro deploy, que é significativamente mais lento do que os seguintes.

### Quando o emulador não arranca

| Sintoma | Causa provável |
|---|---|
| Erro de virtualização ou de Hyper-V | virtualização desativada na BIOS, ou Hyper-V e Windows Hypervisor Platform por ativar |
| O emulador não consta da lista de destinos | não foi criado, ou a imagem é da arquitetura errada |
| Falta de espaço durante o download | cada imagem ocupa vários GB |
| Emulador bloqueado no arranque | fechar e, no Gestor, executar Cold Boot |

Em máquina virtual há ainda a questão da virtualização aninhada, cujo suporte varia consoante o Parallels, VMware Fusion ou UTM.

Alternativa que costuma resolver: ligar um telemóvel Android por USB, com a depuração USB ativa. O dispositivo passa a constar da lista de destinos e é normalmente mais rápido do que um emulador.

## Resolução de problemas

### O SDK 8 não aparece depois de instalado

Por ordem de probabilidade:

1. A janela do PowerShell foi aberta antes da instalação. Fechar e abrir outra.
2. Foi instalada a arquitetura errada. O comando `where.exe dotnet` mostra os executáveis encontrados. Aparecendo dois caminhos, por exemplo `C:\Program Files\dotnet\dotnet.exe` e `C:\Program Files\dotnet\x64\dotnet.exe`, foi instalada a versão x64 num Windows Arm64. Desinstalar e instalar a versão Arm64.
3. A instalação falhou. Executar novamente o instalador: aparecendo "Modificar Instalação", usar o botão Reparar.

Para ver o que existe efetivamente em disco:

```powershell
dir "C:\Program Files\dotnet\sdk"
```

### A opção `--sdk-version` dá erro

Foi removida nas versões recentes do comando `dotnet workload install`. A alternativa é o método do `global.json` descrito no [ponto 4](#4-instalar-os-workloads-do-maui-para-o-net-8).

### O `dotnet workload list` mostra resultados diferentes conforme a pasta

É o comportamento esperado. O comando mostra os workloads do SDK ativo, e o SDK ativo depende do `global.json` da pasta atual. Numa pasta fixada no 8 aparecem os manifestos `/8.0.100`; fora dela, os `/9.0.100`.

### Workloads instalados pela Consola do Package Manager ficam na linha errada

Essa consola corre no contexto do Visual Studio e usa o SDK do Visual Studio. Estes comandos devem ser executados no PowerShell.

### `dotnet workload update`

Convém não executar este comando sem necessidade. Os workloads instalados pelo Visual Studio identificam-se pela coluna "Origem da Instalação", com valores do tipo `VS 17.14...`, e atualizá-los pela linha de comandos pode dessincronizá-los do que o Visual Studio espera. Havendo necessidade de atualizar, fazê-lo pelo Visual Studio Installer.

## Notas finais

Ter o .NET 8 e o .NET 9 instalados em simultâneo não é problema: as versões coexistem e cada projeto usa a indicada no respetivo `TargetFramework`.

O SDK 9 compila sem dificuldade projetos em .NET 8. A instalação do SDK 8 serve sobretudo para que a opção ".NET 8.0" fique disponível no assistente de criação de projetos e para instalar os workloads correspondentes.

O suporte do .NET 8 termina em novembro de 2026. Realizar a ficha em .NET 9, caso o docente o autorize, implica apenas escolher outra versão no assistente.
