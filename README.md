# Bugaboo Test

Esse projeto é um teste prático!

## Tecnologias Usadas
[Unity](https://unity.com/pt) stack:
- [Netcode for GameObjects](https://docs-multiplayer.unity3d.com/) usado para gerenciar o multiplayer.
  [Multiplayer Samples Utilities](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop?path=/Packages/com.unity.multiplayer.samples.coop) usado para complementar o Netcode.
- [ParrelSync](https://github.com/VeriorPies/ParrelSync) usado para facilitar o teste local do multiplayer.
- [2D Sprite](https://docs.unity3d.com/Manual/SpriteEditor.html) usado para criar HUD.

[FrontEnd](https://unity.com/pt) stack:
- [Figma](https://www.figma.com/)
- [Unity UI](https://docs.unity3d.com/Manual/UIToolkits.html)
- [TextMeshPRO](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html)
- [GoogleFonts](https://fonts.google.com/)

[BackEnd](https://unity.com/pt) stack:
- [NodeJS](https://nodejs.org/en)
  [JS](https://developer.mozilla.org/pt-BR/docs/Web/JavaScript)
- [Express](https://expressjs.com/pt-br/)
- [Jest](https://jestjs.io/pt-BR/)
- [supertest](https://www.npmjs.com/package/supertest)

## Setup

1. Clone o repositório
```bash
git clone https://github.com/irahel/BugabooTest.git
```
2. Entre no diretório
```bash
cd BugabooTest
```
### BackEnd Setup

3. Entre no diretório do BackEnd
```bash
cd BackEnd
```
4. Instale as dependências
```bash
npm install
```
5. Execute os testes
```bash
npm test
```
6. Execute a aplicação
```bash
npm start
```

### BackEnd notation
#### Rotas:
```bash
POST /score
```
- Recebe um json do formato:
```json
{
  "player": <PlayerName>,
  "score": <PlayerScore>
}
```
- Retorna 201 caso sucesso e 400 caso contrário

```bash
GET /score
```
- Retorna 200 caso sucesso e uma lista de objetos do formato:
```json
{
  "player1": <score1>,
  "player2": <score2>,
  ...
}
```
- Caso contrário é retornado um 400

```bash
GET /score/:<PlayerName>
```
- Retorna 200 caso sucesso e um json do formato:
```json
{
  "player": <PlayerName>,
  "score": <PlayerScore>
}
```
- Caso contrário é retornado um 404

```bash
GET /ranking
```
- Retorna 200 caso sucesso e um json do formato, com o top 3 jogadores:
```json
{
  "player": <PlayerName>,
  "score": <PlayerScore>
}
```
- Caso contrário é retornado um 404

#### Especificações

A porta padrão do BackEnd é 6161.
O protocolo padrão é http.
O banco utilizado é uma simulação em memória em visão do tempo.
O Jest irá testar o seguinte:
    √ Deve cadastrar um novo player com sucesso                                                       
    √ Deve retornar um erro ao cadastrar player sem nome ou pontuação
    √ Deve retornar o score de um player específico                                               
    √ Deve retornar erro quando tentar obter score de player não encontrado                                
    √ Deve retornar o ranking dos três melhores players

### Unity notation
3. Abra o projeto via Unity Hub
```bash
Unity Hub > Open Project From Disk > BugabooTest
```
4. Espere a instalação de dependências do projeto
5. Crie uma nova instância do ParrelSync
```bash
Na barra de menus superior:
ParrelSync > ClonesManager > Create new clone
```
6. Abra o clone em outro editor
```bash
Na janela aberto do ParrelSync:
Open in New Editor
```
7. Execute o jogo no Editor Principal e no Editor Secundário
8. Divirta-se!

#### Fluxo de Gameplay:

1. Avance da tela inicial com Enter
<img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/1.png?raw=true">
2. No menu principal selecione uma opção
  2.1. Clicar em "Criar a partida" iniciará um Host
  2.2. Clicar em "Entrar na partida" iniciará um Client
  Atenção: Inicie 1 jogador primeiro como Host e logo após 1 como Client
<img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/2.png?raw=true">
3. Informe um nome, que será usado para o ranqueamento global no BackEnd
<img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/3.png?raw=true">
4. Para o Host o jogo irá esperar o outro jogador entrar
5. Para o Client o jogo irá iniciar assim que entrar
<img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/4.png?raw=true">
6. Caixas irão spawnar aleatoriamente pelo mapa sincronizadas
  6.1. Colidir com uma caixa irá destruí-la e o jogador ganhará 100 pontos
 <img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/5.png?raw=true">
8. O jogo se encerrará ao decorrer de 5 minutos
9. Será mostrado uma tela informando o vencedor, assim como o ranking global
<img width = "100%" src="https://github.com/irahel/BugabooTest/blob/master/Capturas/6.png?raw=true">

![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)
