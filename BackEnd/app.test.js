const request = require("supertest");
const app = require("./app");

describe("Testes da API", () => {
  const player1 = "Alice";
  const player2 = "Bob";
  const player3 = "Charlie";

  beforeEach(() => {
    app.clearPlayers();
  });

  test("Deve cadastrar um novo player com sucesso", async () => {
    const response = await request(app)
      .post("/score")
      .send({ player: player1, score: 100 });

    expect(response.status).toBe(201);
    expect(response.body).toEqual({ message: "Player cadastrado com sucesso!" });
  });

  test("Deve retornar um erro ao cadastrar player sem nome ou pontuação", async () => {
    const response1 = await request(app)
      .post("/score")
      .send({ player: player1 });

    const response2 = await request(app)
      .post("/score")
      .send({ score: 100 });

    const response3 = await request(app)
      .post("/score")
      .send({ player: player1, score: "pontuação inválida" });

    expect(response1.status).toBe(400);
    expect(response1.body).toEqual({ error: "Nome de jogador ou pontuação inválidos." });

    expect(response2.status).toBe(400);
    expect(response2.body).toEqual({ error: "Nome de jogador ou pontuação inválidos." });

    expect(response3.status).toBe(400);
    expect(response3.body).toEqual({ error: "Nome de jogador ou pontuação inválidos." });
  });

  test("Deve retornar o score de um player específico", async () => {
    app.setPlayerScore(player1, 100);
    const response = await request(app).get(`/score/${player1}`);
    expect(response.status).toBe(200);
    expect(response.body).toEqual({ player: player1, score: 100 });
  });

  test("Deve retornar erro quando tentar obter score de player não encontrado", async () => {
    const response = await request(app).get(`/score/non_existent_player`);
    expect(response.status).toBe(404);
    expect(response.body).toEqual({ error: "Player não encontrado." });
  });

  test("Deve retornar o ranking dos três melhores players", async () => {
    app.setPlayerScore(player1, 100);
    app.setPlayerScore(player2, 150);
    app.setPlayerScore(player3, 50);

    const response = await request(app).get(`/ranking`);

    expect(response.status).toBe(200);
    expect(response.body).toEqual([
      { player: player2, score: 150 },
      { player: player1, score: 100 },
      { player: player3, score: 50 },
    ]);
  });
});
