# Teste técnico - Banco Master - Calculadora de valor de viagens

Projeto desenvolvido como parte do teste técnico do processo seletivo do banco master para desenvolvedor pleno.

Problema - Dado um n número de conexões para viagens, a aplicação deverá indicar qual é o caminho mais barato para realizar essa viagem, independente do número de conexões.

Solução - Criando dentro de Domain.Structures as estruturas de dados de nodos e grafos, foi possível implementar o algoritmo de Djikstra, que utiliza a iteração por cada nodo ou vértice do grafo, testando se utilizando a aresta ou ligação conseguirá um valor de caminho menor do que o inicial ou algum que já foi encontrado anteriormente.

Após buscar a lista de locais relacionados aos pontos de partida e destino, nodos do grafo e o grafo são criados utilizando um motor.

e um buscador específico para viagens (CheapestTravelFinder) inicia a sua busca pelo caminho mais barato.

## Viagens Padrão pré configuradas
O readme fornecido para o teste consistia das seguintes viagens que deveriam estar pré configuradas
```
Origem: GRU, Destino: BRC, Valor: 10
Origem: BRC, Destino: SCL, Valor: 5
Origem: GRU, Destino: CDG, Valor: 75
Origem: GRU, Destino: SCL, Valor: 20
Origem: GRU, Destino: ORL, Valor: 56
Origem: ORL, Destino: CDG, Valor: 5
Origem: SCL, Destino: ORL, Valor: 20
```

## Interface Rest
    A interface Rest deverá suportar o CRUD de rotas:
    - Manipulação de rotas, dados podendo ser persistidos em arquivo, bd local, etc...
    - Consulta de melhor rota entre dois pontos.
	
### Exemplo:
```
Consulte a rota: GRU-CGD
Resposta: GRU - BRC - SCL - ORL - CDG ao custo de $40
  
Consulte a rota: BRC-SCL
Resposta: BRC - SCL ao custo de $5
```

## Rodando a aplicação
A aplicação possui suporte ao docker e arquivos docker-compose, permitindo assim utilizar a ferramente da orquestrção de containers para ser iniciada.
A aplicação está mapeada para rodar em http no port http://localhost:5001/ e https em https://localhost:5002/ -> estes ports podem ser alterados no docker-compose.override.yml
Alternativamente navegar até a pasta /src/TechTest.BancoMaster.Travels.Api e executar o comando a seguir via terminal também rodará a aplicação localmente (Necessário Runtime do .NET instalado na versão 6.0)

```
    dotnet watch run
```

## Endpoints 
É possível acessar o swagger da aplicação descrevendo cada um dos endpoints através do index da aplicação.

- Calcular o menor valor de viagem entre duas localizações
GET /api/travels/cheapest/from/{from}/to/{to}

- Retorna todas as viagems - possui parametros de paginação através da query da url.
GET /api/travels 

- Retorna todas as viagens de um ponto de partida.
GET /api/travels/startingPoint/{startingPoint} 

- Retorna todas as viagens de um ponto de destino.
GET /api/travels/destination/{destination} 

- Adiciona uma nova rota de viagem e seu custo.
POST /api/travels/

- Permite atualizar o valor de uma viagem.
PATCH /api/travels/

- Deleta uma viagem (necessário conhecer o ID da viagem)
DELETE /api/travels/{travelId}

## Técnologias utilizadas

- .NET 6.0
- Asp.Net 6.0

## Bibliotecas Utilizadas

- Awarean.Sdk.Result -> Biblioteca autoral para trabalhar com resultados de operações (Sucesso ou falha).
- Awarean.Sdk.ValueObjects -> Biblioteca autoral para trabalhar com objetos de valor, abstraindo classicas validas (valor monetario positivo, por exemplo);
- Awarean.Sdk.SharedKernel -> Biblioteca autoral contendo coisas comuns de projetos como interfaces para entidades, repositorios, serviços.

- Microsoft.Extensions.DependencyInjection -> Usada para trabalhar o conceito de inversão de controle e injetar dependências automaticamente.
- Microsoft.Extensions.Logging -> Usada para trabalhar adicionar Loggers no serviço (Não implementado atualmente, necessário configurar no Startup da API).

## Melhorias possíveis.

- Utilizar um verdadeiro banco de dados (pessoalmente utilizaria o mongoDB ou dynamoDB) para persistir os dados da aplicação em vez de um dicionário em memória. (utilizado para brevidade).
- Adicionar mais testes específicos para cada componente (serviços e repositórios).
- Diminuir necessidades de tantos loops independentes na implementação do algoritimo de Djikstra para encontrar o menor valor.
- Criar objetos específicos de request e response para os endpoints.
- Adicionar um verdadeiro mecanismo de Logs utilizando Serilog (Enviar logs para o ELK, LogStash, PostreSQL)
- Adicionar um Cache Distribuído (Redis, Memcached etc)
- Criar uma pipeline para executar testes e build automaticamente no github actions (Já possuo os jobs feitos)
- Abstrair entidades de dominio do serviço e da API (Camadas mais externas) utilizando DTO's (melhorando conceitos de clean archtecture).
- Adicionar mensagens de erro não mapeadas em classes específicas para erros (vide: ApplicationErrors)